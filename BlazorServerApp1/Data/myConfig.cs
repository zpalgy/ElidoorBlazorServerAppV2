using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlazorServerApp1
{
    // this replaces web.config Appsettings section
    //  we use teh project's appsettings.json for that.
    // ref : https://stackoverflow.com/questions/43757189/how-to-get-value-from-appsettings-json/43770342 
    //   I used the answer that got 35 points by PinoyDev (Nov 6 2018 at 17:10 
    //   Here below I created the class myConfigClass .
    //   I use it in PrApiCalls.cs : public static void initRestClient(RestClient restClient) 
    //  the 1st overload at line 61  


    public class RestApiSettings
    {
        //public const string RestApi = "RestApi";

        public string Host { get; set; }
        public string ApiPart { get; set; }
        public string ENV { get; set; }
        public string INI { get; set; }
        public string APIUser { get; set; }
        public string APIPWD { get; set; }
    }

    public class myConfigClass
    {
        private IConfiguration _configuration;
        public myConfigClass(IConfiguration iconfig)
        {
            _configuration = iconfig;
        }
        public string configVal(string configKey)
        {
            return _configuration.GetValue<string>(configKey);
        }
    }
}
