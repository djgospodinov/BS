using BS.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BS.Common.Interfaces
{
    public interface IVariablesService
    {
        List<LicenseVariableModel> GetLookupVariables();

        bool CreateVariable(string name, string type = null);

        List<LicenseVariableModel> GetVariables(string licenseId);

        void CreateVariables(string licenseId, Dictionary<string, object> values);

        void UpdateVariables(string licenseId, Dictionary<string, object> values);

        void DeleteVariables(string licenseId, List<string> variables);
    }
}
