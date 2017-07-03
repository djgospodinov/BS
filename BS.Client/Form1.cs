using BS.Api.Models;
using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BS.Common;

namespace BS.Client
{
    public partial class Form1 : Form
    {
        protected readonly string _apiUrl = ConfigurationManager.AppSettings["apiUrl"].ToString();
        protected readonly HttpClient _client = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtLicenseId.Text = "9e35c57e-eecc-4123-a5dc-f914ccb89545".Replace("-", "");

            foreach (LicenseModules m in Enum.GetValues(typeof(LicenseModules)))
            {
                checkedListModules.Items.Add(new ListBoxItem() { Value = (int)m, Name = m.Description() });
            }

            _client.BaseAddress = new Uri(_apiUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var licenseId = txtLicenseId.Text;
            if (string.IsNullOrEmpty(licenseId)) 
            {
                MessageBox.Show("Enter license Id.");
                return;
            }

            LicenseModel result = null;
            HttpResponseMessage response = await _client.GetAsync(string.Format("/api/license/{0}", licenseId));
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsAsync<LicenseModel>();

                txtInfo.Text = result.ToString();
            }
            else 
            {
                txtInfo.Text = "Грешка!";
            }
        }

        private async void btnCreateLicense_Click(object sender, EventArgs e)
        {
            var modules = new List<LicenseModules>();
            foreach (ListBoxItem m in checkedListModules.CheckedItems)
            {
                modules.Add((LicenseModules)m.Value);
            }

            var result = new LicenseModel
            {
                ValidTo = DateTime.Now.AddMonths(3),
                IsDemo = true,
                Modules = modules,
                User = new LicenserInfoModel() 
                {
                    Name = txtName.Text,
                    Phone = txtPhone.Text,
                    Email = txtEmail.Text,
                    ConactPerson = txtContactPerson.Text,
                    IsCompany = chkCompany.Checked
                }
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/license", result);
            if (response.IsSuccessStatusCode)
            {
                string id = await response.Content.ReadAsStringAsync();

                txtCreatedLicenseId.Text = id.Replace("\"", "");
            }
            else
            {
                txtCreatedLicenseId.Text = "Грешка!";
            }
        }
    }
}
