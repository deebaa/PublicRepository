using System;
using System.Configuration;

namespace PublicRepository
{
    public class ApplicationConfig
    {
        public string apiURL { get; }
        public string employeeConnection { get; set; }

        public ApplicationConfig()
        {
            apiURL = ConfigurationManager.AppSettings["apiURL"] ??"";
            employeeConnection = ConfigurationManager.ConnectionStrings["employeeConnection"].ConnectionString??"";
        }    
    }
}
