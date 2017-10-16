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
using Newtonsoft.Json;
using System.Net;

namespace BS.Client
{
    public partial class Form1 : Form
    {
        protected static readonly string _apiUrl = ConfigurationManager.AppSettings["apiUrl"].ToString();
        protected HttpClient _client = CreateClient();
        private string _token = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Authenticate();

            txtRequest.Text = 
@"{  
   ""ValidTo"":""2018-01-20"",
   ""SubscribedTo"":""2018-01-20"",
   ""IsDemo"":false,
   ""User"":{  
      ""PostCode"":0,
      ""RegistrationAddress"":null,
      ""PostAddress"":null,
      ""MOL"":null,
      ""AccountingPerson"":null,
      ""DDSRegistration"":false,
      ""Name"":""1"",
      ""IsCompany"":true,
      ""Phone"":""1"",
      ""Email"":""1"",
      ""ConactPerson"":""1"",
      ""CompanyId"":""123"",
      ""EGN"":null
   },
   ""Modules"":[  
      1, 2, 3, 4 , 5 ,6
   ],
   ""Type"":1,
   ""Enabled"":false
}";
        }

        private static HttpClient CreateClient(bool useHttps = false)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(useHttps ? _apiUrl.Replace("http", "https") : _apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();

            return client;
        }

        private async void Authenticate()
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", "bsadmin"),
                new KeyValuePair<string, string>("password", "bsadmin!")
            });

            _client = CreateClient();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

            HttpResponseMessage response = await _client.PostAsync("/api/token", content);
            var result = await response.Content.ReadAsAsync<Dictionary<string, string>>();
            _token = result["access_token"];

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await GetLicenseInfo();
        }

        private async Task GetLicenseInfo()
        {
            var licenseId = txtLicenseId.Text;
            if (string.IsNullOrEmpty(licenseId))
            {
                MessageBox.Show("Enter license Id.");
                return;
            }

            LicenseModel result = null;

            if (_client != null)
            {
                HttpResponseMessage response = await _client.PostAsync(string.Format("/api/verifylicense/{0}", licenseId), null);
                if (response.IsSuccessStatusCode)
                {
                    var responseResult = await response.Content.ReadAsAsync<Dictionary<string, string>>();
                    var privateKey = StringCipher.Decrypt(responseResult["Key"].ToString(), Common.Constants.PublicKey);
                    var json = StringCipher.Decrypt(responseResult["License"].ToString(), privateKey);
                    result = JsonConvert.DeserializeObject<LicenseModel>(json);

                    var encryptedResult = await response.Content.ReadAsStringAsync();
                    txtRealOutput.Text = JsonHelper.FormatJson(encryptedResult);
                    txtJson.Text = JsonHelper.FormatJson(json);
                    txtInfo.Text = result.ToString();
                }
                else
                {
                    txtInfo.Text = "Грешка!";
                }
            }
        }

        private async void btnCreateLicense_Click(object sender, EventArgs e)
        {
            var result = JsonConvert.DeserializeObject<LicenseModel>(txtRequest.Text);

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

        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            Authenticate();
        }

        private void txtJson_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            await GetLicenseInfo();
        }
    }

    public class JsonHelper
    {
        private const string INDENT_STRING = "    ";
        public static string FormatJson(string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }
    }

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}
