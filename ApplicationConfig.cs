using System;
using System.Configuration;

namespace PublicRepository
{
    internal class ApplicationConfig
    {
        public string apiURL { get; }
        public string username { get; }
        public string token { get; }

        public ApplicationConfig()
        {
            apiURL = ValidateKeys(ConfigurationManager.AppSettings["apiURl"]);
            token = ValidateKeys(ConfigurationManager.AppSettings["token"]);
            username = ValidateKeys(ConfigurationManager.AppSettings["username"]);
        }
        private string ValidateKeys(string key)
        {
            if (string.IsNullOrEmpty(key)) { return ""; }
            else { return key; }
        }        
    }
}
