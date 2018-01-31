using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using BS.Admin.Web.Filters;
using BS.Admin.Web.Models;
using BS.Admin.Web.Db;

namespace BS.Admin.Web.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private readonly BSAdminDbEntities _db = new BSAdminDbEntities();

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToAction("Index", "License");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Потребителят или паролата не са правилни.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register

        //[AllowAnonymous]
        public ActionResult Register()
        {
            if (!RolesManager.IsAdministrator(User.Identity)) 
            {
                return RedirectToAction("UnAuthorized", "Error");
            }
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (!RolesManager.IsAdministrator(User.Identity))
            {
                return RedirectToAction("UnAuthorized", "Error");
            }

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    Roles.AddUsersToRole(new[] { model.UserName }, 
                        roleName: model.IsSuperUser ? Const.SuperUserRoleName : Const.NormalUserRoleName);

                    ViewBag.Message = "Потребителя е създаден успешно!";
                    return RedirectToAction("Index");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var result = _db.UserProfiles.First(x => x.UserId == id);
            var model = new EditAccountModel();
            model.UserId = id;
            model.UserName = result.UserName;
            model.RoleId = result.webpages_Roles.FirstOrDefault()?.RoleId ?? 2;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EditAccountModel model)
        {
            if (!RolesManager.IsAdministrator(User.Identity))
            {
                return RedirectToAction("UnAuthorized", "Error");
            }

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    var result = _db.UserProfiles.FirstOrDefault(x => x.UserId == model.UserId);
                    if (result != null)
                    {
                        if (!string.IsNullOrEmpty(model.NewPassword))
                        {
                            WebSecurity.ChangePassword(result.UserName, model.NewPassword, model.ConfirmPassword);
                        }

                        string roleName = null;
                        switch (model.RoleId)
                        {
                            case 1:
                                roleName = "Administrator";
                                break;
                            case 2:
                                roleName = "SuperUser";
                                break;
                            case 3:
                                roleName = "NormalUser";
                                break;
                        }

                        if (!Roles.IsUserInRole(result.UserName, roleName))
                        {
                            var roles = Roles.GetRolesForUser(result.UserName);
                            if (roles.Length > 0)
                            {
                                Roles.RemoveUserFromRoles(result.UserName, roles);
                            }
                            
                            Roles.AddUserToRole(result.UserName, roleName);
                        }


                        result.UserName = model.UserName;
                        _db.SaveChanges();
                    }

                    ViewBag.Message = "Потребителя е редактиран успешно!";
                    return RedirectToAction("Index");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Cancel()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Data(LicenseFilterGridModel filter)
        {
            var dbModel = _db.UserProfiles
                .ToList()
                .Select(x => new
            {
                Id = x.UserId,
                Name = x.UserName,
                Role = x.webpages_Roles.FirstOrDefault()?.RoleName ?? "NormalUser",
                EditUrl = string.Format("../Account/Edit/{0}", x.UserId)
            }).ToList();

            //if (filter.UserId.HasValue)
            //{
            //    dbModel = dbModel.Where(x => x.User.Id == filter.UserId.Value)
            //        .ToList();
            //}

            //var data = dbModel
            //    .Skip((filter.PageIndex - 1) * filter.PageSize)
            //    .Take(filter.PageSize)
            //    .Select(x => new
            //    {
            //        Id = string.Format("{0}...", x.Id.ToString().Substring(0, 20)),
            //        ValidTo = x.ValidTo.ToShortDateString(),
            //        Demo = x.IsDemo,
            //        UserName = x.User.Name,
            //        Created = x.Created.ToShortDateString(),
            //        Activated = x.IsActivated,
            //        Enabled = x.Enabled,
            //        Type = (int)x.Type,
            //        EditUrl = RolesManager.CanCreateLicense(User.Identity)
            //        ? string.Format("../License/Edit/{0}", x.Id)
            //        : string.Empty
            //    }).ToArray();

            var dataResult = new
            {
                data = dbModel,
                itemsCount = dbModel.Count
            };

            return Json(dataResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Паролата Ви беше променена."
                : message == ManageMessageId.SetPasswordSuccess ? "Паролата Ви беше създадена."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", String.Format("Unable to create local account. An account with the name \"{0}\" may already exist.", User.Identity.Name));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Потребител с това име вече съществува. Изберете друго име.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
