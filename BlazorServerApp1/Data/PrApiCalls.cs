using System;
using System.Linq;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace BlazorServerApp1.Data
{
    public static class PrApiCalls
    {

        //public static DataTable MeagedFields = new DataTable();  // it's a table that is common to all users !
        //public static DoorConfig doorConfig = new DoorConfig();
        public static DataTable dtMeagedFields;
        public static DataTable dtDecorSideFlds;
        public static DataTable dtConfFields;
        public static DataTable dtDefaults;

        public static List<PART_Class> lstParts = new List<PART_Class>();
        public static DataTable dtParts = new DataTable();
        public static List<TRSH_COLOR_Class> lstColors = new List<TRSH_COLOR_Class>();
        public static List<TRSH_COLOR_Class> lstGlassColors4Diamond = new List<TRSH_COLOR_Class>();
        public static List<TRSH_LOCKHINGE_DRILH_Class> lstLock_Hinge_Dril_Heights = new List<TRSH_LOCKHINGE_DRILH_Class>();  // table 230
        public static DataTable dtLock_Hinge_Dril_Heights = new DataTable();
        public static List<TRSH_HARDWARE_Class> lstHardwares = new List<TRSH_HARDWARE_Class>();
        public static DataTable dtHardwares = new DataTable();
        public static List<DRIL4HW_Class> lstDril4Hw = new List<DRIL4HW_Class>();
        public static DataTable dtDril4Hws = new DataTable();
        public static List<CYLINDER_Class> lstCylinders = new List<CYLINDER_Class>();
        public static List<TRSH_LOCK_Class> lstLocks = new List<TRSH_LOCK_Class>();
        public static DataTable dtLocks = new DataTable();

        public static List<TILETYPE_Class> lstTileTypes = new List<TILETYPE_Class>();
        public static List<RAW4CPLATES_Class> lstRaw4CPlates = new List<RAW4CPLATES_Class>();
        public static List<WINDOWWIDTH_Class> lstWindowWidths = new List<WINDOWWIDTH_Class>();
        public static DataTable dtWindowWidths = new DataTable();
        public static List<WINDOWHEIGHT_Class> lstWindowHeights = new List<WINDOWHEIGHT_Class>();
        public static DataTable dtWindowHeights = new DataTable();
        public static List<PROFILE4WINDOW_Class> lstProfiles4Windows = new List<PROFILE4WINDOW_Class>();
        public static List<GLASS4WINDOW_Class> lstGlasses4Windows = new List<GLASS4WINDOW_Class>();
        public static List<GRID_Class> lstGrids = new List<GRID_Class>();
        public static List<VITRAGE_Class> lstVitrages = new List<VITRAGE_Class>();
        public static List<VITRAGE4DIAMOND_Class> lstVitrages4Diamond = new List<VITRAGE4DIAMOND_Class>();
        public static List<GRID4HT1084_Class> lstGrid4HT1084 = new List<GRID4HT1084_Class>();
        public static List<HANDLE_Class> lstHandles = new List<HANDLE_Class>();
        public static List<HANDLE4DIAMOND_Class> lstHandles4Diamond = new List<HANDLE4DIAMOND_Class>();

        public static string[] D60DataSource = new string[] { "ללא", "חוץ", "פנים", "דו צדדי" };
        public static string[] D60No2DidesDataSource = new string[] { "ללא", "חוץ", "פנים" };

        //public static RestClient restClient = new RestClient();  - this is page instance specific we can't make it application specific
        static string certAlert = "Pls check whether the SSL certificate of the Default Web Site on the web server has expired";



        public static void initRestClient(RestClient restClient)
        {
            try
            {
                RestApiSettings rest = new RestApiSettings();
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false).Build();
                myConfigClass myConfig = new myConfigClass(config);
                string host = myConfig.configVal("RestApi:Host");
                string apiPart = myConfig.configVal("RestApi:ApiPart");
                string INI = myConfig.configVal("RestApi:INI");
                string ENV = myConfig.configVal("RestApi:ENV");
                string ApiUser = myConfig.configVal("RestApi:APIUser");
                string APIPWD = myConfig.configVal("RestApi:APIPWD");

                /* Tropical TEST SERVER */
                restClient.BaseUrl = new Uri(string.Format("{0}/{1}/{2}/{3}", host, apiPart, INI, ENV));

                //myLogger1.WriteLog(string.Format("restClient.BaseUrl = {0}", restClient.BaseUrl));
                //restClient.Authenticator = new HttpBasicAuthenticator(System.Configuration.ConfigurationManager.AppSettings["APIUser"],
                //                                 System.Configuration.ConfigurationManager.AppSettings["APIPWD"]);
                restClient.Authenticator = new HttpBasicAuthenticator(ApiUser, APIPWD);
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static void initRestClient(RestClient restClient, string company)
        {
            try
            {
                /* Tropical TEST SERVER */
                restClient.BaseUrl = new Uri(string.Format("{0}/{1}/{2}/{3}", System.Configuration.ConfigurationManager.AppSettings["Host"],
                                    System.Configuration.ConfigurationManager.AppSettings["ApiPart"],
                                    System.Configuration.ConfigurationManager.AppSettings["INI"],
                                             company));
                //myLogger1.WriteLog(string.Format("restClient.BaseUrl = {0}", restClient.BaseUrl));
                restClient.Authenticator = new HttpBasicAuthenticator(System.Configuration.ConfigurationManager.AppSettings["APIUser"],
                                                 System.Configuration.ConfigurationManager.AppSettings["APIPWD"]);
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static string getReference(string reference, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "REFERENCE";
                request.Resource = string.Format("TRSH_DOORCONFIG?$filter=REFERENCE eq '{0}'&$select={1}", reference, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDoorConfig val = JsonConvert.DeserializeObject<ValuesDoorConfig>(response.Content);
                    if (val.value.Count > 0)
                        return val.value[0].REFERENCE;
                    else
                        return string.Empty;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return string.Empty;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<AGENT_Class> getAgents(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "AGENT,AGENTCODE,AGENTNAME";
                request.Resource = string.Format("AGENTS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesAGENT_Class val = JsonConvert.DeserializeObject<ValuesAGENT_Class>(response.Content);
                    List<AGENT_Class> val1 = new List<AGENT_Class>();  //val.value;
                    AGENT_Class emptyAgent = new AGENT_Class();
                    emptyAgent.AGENTCODE = " ";
                    emptyAgent.AGENTNAME = " ";
                    val1.Add(emptyAgent);
                    foreach (AGENT_Class ag in val.value)
                    {
                        val1.Add(ag);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<CUSTOMER_Class> getCustomers(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "CUST,CUSTNAME,CUSTDES";  //,ADDRESS,ADDRESS2,ADDRES3";
                request.Resource = string.Format("CUSTOMERS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesCUSTOMER_Class val = JsonConvert.DeserializeObject<ValuesCUSTOMER_Class>(response.Content);
                    List<CUSTOMER_Class> val1 = new List<CUSTOMER_Class>();  //val.value;
                    CUSTOMER_Class emptyCust = new CUSTOMER_Class();
                    emptyCust.CUSTNAME = " ";
                    emptyCust.CUSTDES = " ";
                    val1.Add(emptyCust);
                    foreach (CUSTOMER_Class cu in val.value)
                    {
                        val1.Add(cu);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static CUSTOMER_Class getCustomer(int CUST, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "CUST,CUSTNAME,CUSTDES,ADDRESS,ADDRESS2,ADDRESS3,TRSH_LOGO";
                request.Resource = string.Format("CUSTOMERS?$filter=CUST eq {0}&$select={1}", CUST, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesCUSTOMER_Class val = JsonConvert.DeserializeObject<ValuesCUSTOMER_Class>(response.Content);
                    CUSTOMER_Class val1 = val.value[0];
                    return val1;

                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<PART_Class> getAllParts(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string filter = "TYPE eq 'P' and ISMPART ne 'Y' and MPARTNAME ne ''";
                string fields = "PARTNAME,PARTDES,MPARTNAME,MPARTDES,FAMILYDES";

                request.Resource = string.Format("LOGPART?$filter={0}&$select={1}", filter, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesPART_Class val = JsonConvert.DeserializeObject<ValuesPART_Class>(response.Content);
                    List<PART_Class> val1 = new List<PART_Class>();  //val.value;
                    PART_Class emptyPart = new PART_Class();
                    emptyPart.PARTNAME = " ";
                    val1.Add(emptyPart);
                    foreach (PART_Class part in val.value)
                    {
                        val1.Add(part);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static FamilyOfPart_Class getFamilyOfPart(string PARTNAME, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PARTNAME,FAMILYNAME,FAMILYDES";
                request.Resource = string.Format("LOGPART?$filter=PARTNAME eq '{0}'&$select={1}", PARTNAME, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesFamilyOfPart_Class val = JsonConvert.DeserializeObject<ValuesFamilyOfPart_Class>(response.Content);
                    //List<PART_Class> val1 = new List<PART_Class>();  //val.value;
                    FamilyOfPart_Class emptyFamily = new FamilyOfPart_Class();
                    emptyFamily.FAMILYNAME = "";
                    emptyFamily.FAMILYDES = "הפריט לא שוייך למשפחה";
                    if (val == null || val.value == null)
                        return emptyFamily;
                    else if (val != null && val.value != null && val.value.Count == 0)
                    {
                        myLogger.log.Error(string.Format("PARTNAME {0} has no FAMILY", PARTNAME));
                        return emptyFamily;
                    }
                    return val.value[0];
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                return null;
            }
        }

        //  API query example: https://prio.mishol-it.com/odata/Priority/tabula.ini/demo/FAMILY_LOG?$select=FAMILYNAME,FAMILYDESC
        public static List<FAMILY_Class> getFamilies(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "FAMILY,FAMILYNAME,FAMILYDESC";
                request.Resource = string.Format("FAMILY_LOG?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesFAMILY_Class val = JsonConvert.DeserializeObject<ValuesFAMILY_Class>(response.Content);
                    List<FAMILY_Class> val1 = new List<FAMILY_Class>();  //val.value;
                    FAMILY_Class emptyFam = new FAMILY_Class();
                    emptyFam.FAMILYNAME = " ";
                    val1.Add(emptyFam);
                    foreach (FAMILY_Class fam in val.value)
                    {
                        val1.Add(fam);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static int getFAMILY(string FAMILYNAME, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "FAMILY,FAMILYNAME,FAMILYDESC";
                request.Resource = string.Format("FAMILY_LOG?$filter=FAMILYNAME eq '{0}'&$select={1}", FAMILYNAME, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesFAMILY_Class val = JsonConvert.DeserializeObject<ValuesFAMILY_Class>(response.Content);
                    return val.value[0].FAMILY;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes somewhere !";
                        myLogger.log.Error(errMsg);
                        return -1;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }


        public static List<PART_Class> getFamilyParts(int family, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PART,PARTNAME,PARTDES";
                request.Resource = string.Format("FAMILY_LOG?$filter=FAMILY eq {0}&$select=FAMILYNAME&$expand=FAMILY_LOGPART_SUBFORM($select={1};$orderby=PARTNAME)"
                    , family, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    string jsonValue = response.Content;
                    FamilyParts_Class val = JsonConvert.DeserializeObject<FamilyParts_Class>(response.Content);
                    List<PART_Class> result = (List<PART_Class>)val.value[0].FAMILY_LOGPART_SUBFORM;
                    PART_Class emptyPart = new PART_Class();
                    emptyPart.PARTNAME = " ";
                    result.Insert(0, emptyPart);

                    return result;

                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }


        public static int getDoorHwCatCode(string PARTNAME, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PARTNAME,TRSH_DOOR_HWCATCODE";
                request.Resource = string.Format("LOGPART?$filter=PARTNAME eq '{0}'&$select={1}", PARTNAME, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesPART_Class val = JsonConvert.DeserializeObject<ValuesPART_Class>(response.Content);
                    if (val == null || val.value == null)
                        return 0;
                    else if (val != null && val.value != null && val.value.Count == 0)
                    {
                        myLogger.log.Error(string.Format("PARTNAME {0} has no TRSH_DOOR_HWCATCODE", PARTNAME));
                        return 0;
                    }
                    return val.value[0].TRSH_DOOR_HWCATCODE;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return 0;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return 0;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                return 0;
            }
        }

        #region get tables data from priority
        public static void InitInMemoryDataSources(ref string errMsg)
        {
            try
            {
                dtMeagedFields = getAllMeagedFields(ref errMsg);
                if (dtMeagedFields == null)
                    return;  // abort 
                dtDecorSideFlds = getDecorSideFlds(ref errMsg);
                if (dtDecorSideFlds == null)
                    return;  //abort
                dtConfFields = getConfFields(ref errMsg);
                if (dtConfFields == null)
                    return;
                dtDefaults = getDefaults(ref errMsg);
                lstParts = getAllParts(ref errMsg);
                dtParts = lstParts.ToDataTable<PART_Class>();
                lstColors = getColors(ref errMsg);
                lstGlassColors4Diamond = getGlassColors4Diamond(ref errMsg);
                lstLock_Hinge_Dril_Heights = getLockHingeDrilHeights(ref errMsg);
                dtLock_Hinge_Dril_Heights = lstLock_Hinge_Dril_Heights.ToDataTable<TRSH_LOCKHINGE_DRILH_Class>();
                lstHardwares = getHardwares(ref errMsg);
                dtHardwares = lstHardwares.ToDataTable<TRSH_HARDWARE_Class>();
                lstDril4Hw = getDril4Hws(ref errMsg);
                dtDril4Hws = lstDril4Hw.ToDataTable<DRIL4HW_Class>();
                lstCylinders = getCylinders(ref errMsg);
                lstLocks = getLocks(ref errMsg);
                dtLocks = lstLocks.ToDataTable<TRSH_LOCK_Class>();
                lstWindowWidths = getWindowWidths(ref errMsg);
                dtWindowWidths = lstWindowWidths.ToDataTable<WINDOWWIDTH_Class>();
                lstWindowHeights = getWindowHeights(ref errMsg);
                dtWindowHeights = lstWindowHeights.ToDataTable<WINDOWHEIGHT_Class>();
                lstVitrages4Diamond = getVitrages4Diamond(ref errMsg);
                lstVitrages = getVitrages(ref errMsg);
                lstTileTypes = getTileTypes(ref errMsg);
                lstRaw4CPlates = getRaw4CPlates(ref errMsg);
                lstProfiles4Windows = getProfiles4Windows(ref errMsg);
                lstHandles = getHandles(ref errMsg);
                lstHandles4Diamond = getHandles4Diamond(ref errMsg);
                lstGrid4HT1084 = getGrid4HT1084(ref errMsg);
                lstGrids = getGrids(ref errMsg);
                lstGlasses4Windows = getGlasses4Windows(ref errMsg);
                //UiLogic.initTabNames();  -> done in TabContro.AddPage() 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<TRSH_COLOR_Class> getColors(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_COLORID,TRSH_COLORNAME,TRSH_COLORDES";
                request.Resource = string.Format("TRSH_COLORS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_COLOR_Class val = JsonConvert.DeserializeObject<ValuesTRSH_COLOR_Class>(response.Content);
                    List<TRSH_COLOR_Class> val1 = new List<TRSH_COLOR_Class>();  //val.value;
                    TRSH_COLOR_Class emptyColor = new TRSH_COLOR_Class();
                    emptyColor.TRSH_COLORNAME = " ";
                    emptyColor.TRSH_COLORDES = " ";
                    val1.Add(emptyColor);
                    foreach (TRSH_COLOR_Class clr in val.value)
                    {
                        val1.Add(clr);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<TRSH_COLOR_Class> getGlassColors4Diamond(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_COLORID,TRSH_COLORNAME,TRSH_COLORDES";
                request.Resource = string.Format("TRSH_GLASSCLRS4DMND?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_COLOR_Class val = JsonConvert.DeserializeObject<ValuesTRSH_COLOR_Class>(response.Content);
                    List<TRSH_COLOR_Class> val1 = new List<TRSH_COLOR_Class>();  //val.value;
                    TRSH_COLOR_Class emptyColor = new TRSH_COLOR_Class();
                    emptyColor.TRSH_COLORNAME = " ";
                    emptyColor.TRSH_COLORDES = " ";
                    val1.Add(emptyColor);
                    foreach (TRSH_COLOR_Class clr in val.value)
                    {
                        val1.Add(clr);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<TRSH_LOCKHINGE_DRILH_Class> getLockHingeDrilHeights(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_LOCKHINGE_DRILH,TRSH_DOOR_HWCATCODE,DOORHEIGHTMIN,DOORHEIGHTMAX,MEASURENAME,LOCKDRILHEIGHT,BACKPINHEIGHT,HINGESNUM,HINGE1HEIGHT,"
                                    + "HINGE2HEIGHT,HINGE3HEIGHT,HINGE4HEIGHT,HINGE5HEIGHT";
                request.Resource = string.Format("TRSH_LOCKHINGE_DRILH?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_LOCKHINGE_DRILH_Class val = JsonConvert.DeserializeObject<ValuesTRSH_LOCKHINGE_DRILH_Class>(response.Content);
                    List<TRSH_LOCKHINGE_DRILH_Class> val1 = new List<TRSH_LOCKHINGE_DRILH_Class>();  //val.value;
                    TRSH_LOCKHINGE_DRILH_Class emptyLockHingeDrilH = new TRSH_LOCKHINGE_DRILH_Class();
                    emptyLockHingeDrilH.TRSH_DOOR_HWCATCODE = 0;
                    val1.Add(emptyLockHingeDrilH);
                    foreach (TRSH_LOCKHINGE_DRILH_Class lhd in val.value)
                    {
                        val1.Add(lhd);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //getHardwares
        public static List<TRSH_HARDWARE_Class> getHardwares(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_HARDWARE,TRSH_DOOR_HWCATCODE,HARDWAREDES,DRIL4HW,DRIL4HWDES";
                request.Resource = string.Format("TRSH_HARDWARE?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_HARDWARE_Class val = JsonConvert.DeserializeObject<ValuesTRSH_HARDWARE_Class>(response.Content);
                    List<TRSH_HARDWARE_Class> val1 = new List<TRSH_HARDWARE_Class>();  //val.value;
                    TRSH_HARDWARE_Class emptyHw = new TRSH_HARDWARE_Class();
                    emptyHw.TRSH_HARDWARE = 0;
                    emptyHw.HARDWAREDES = " ";
                    val1.Add(emptyHw);
                    foreach (TRSH_HARDWARE_Class hw in val.value)
                    {
                        val1.Add(hw);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<TRSH_HARDWARE_Class> getPartHWs(int TRSH_DOOR_HWCATCODE, ref string errMsg)
        {
            try
            {
                DataRow[] rowsArray;
                string query = string.Format("TRSH_DOOR_HWCATCODE = '{0}'", TRSH_DOOR_HWCATCODE);
                rowsArray = PrApiCalls.dtHardwares.Select(query);
                if (rowsArray.Length > 0)
                {
                    List<TRSH_HARDWARE_Class> lstParHWs = new List<TRSH_HARDWARE_Class>();
                    TRSH_HARDWARE_Class emptyHW = new TRSH_HARDWARE_Class();
                    emptyHW.TRSH_HARDWARE = 0;
                    emptyHW.HARDWAREDES = " ";
                    lstParHWs.Add(emptyHW);
                    for (int r = 0; r < rowsArray.Length; r++)
                    {
                        TRSH_HARDWARE_Class Hw1 = new TRSH_HARDWARE_Class();
                        Hw1.TRSH_HARDWARE = int.Parse(rowsArray[r]["TRSH_HARDWARE"].ToString());
                        Hw1.HARDWAREDES = rowsArray[r]["HARDWAREDES"].ToString();
                        lstParHWs.Add(Hw1);
                    }
                    return lstParHWs;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<DRIL4HW_Class> getDril4Hws(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "DRIL4HW,DRIL4HWDES";
                request.Resource = string.Format("TRSH_DRIL4HW?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDRIL4HW_Class val = JsonConvert.DeserializeObject<ValuesDRIL4HW_Class>(response.Content);
                    List<DRIL4HW_Class> val1 = new List<DRIL4HW_Class>();  //val.value;
                    DRIL4HW_Class emptyDril4Hw = new DRIL4HW_Class();
                    emptyDril4Hw.DRIL4HWDES = " ";
                    val1.Add(emptyDril4Hw);
                    foreach (DRIL4HW_Class dril4Hw in val.value)
                    {
                        val1.Add(dril4Hw);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        public static List<CYLINDER_Class> getCylinders(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "CYLINDERNAME,CYLINDERDES";
                request.Resource = string.Format("TRSH_CYLINDERS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesCYLINDER_Class val = JsonConvert.DeserializeObject<ValuesCYLINDER_Class>(response.Content);
                    List<CYLINDER_Class> val1 = new List<CYLINDER_Class>();  //val.value;
                    CYLINDER_Class emptyCylinder = new CYLINDER_Class();
                    emptyCylinder.CYLINDERNAME = " ";
                    emptyCylinder.CYLINDERDES = " ";
                    val1.Add(emptyCylinder);
                    foreach (CYLINDER_Class cyl in val.value)
                    {
                        val1.Add(cyl);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //getLocks" 
        public static List<TRSH_LOCK_Class> getLocks(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_LOCK,TRSH_DOOR_HWCATCODE,TRSH_LOCKNAME,TRSH_LOCKDES";
                request.Resource = string.Format("TRSH_LOCKS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTRSH_LOCK_Class val = JsonConvert.DeserializeObject<ValuesTRSH_LOCK_Class>(response.Content);
                    List<TRSH_LOCK_Class> val1 = new List<TRSH_LOCK_Class>();  //val.value;
                    TRSH_LOCK_Class emptyLock = new TRSH_LOCK_Class();
                    emptyLock.TRSH_LOCKDES = " ";
                    emptyLock.TRSH_LOCKNAME = " ";
                    val1.Add(emptyLock);
                    foreach (TRSH_LOCK_Class lck in val.value)
                    {
                        val1.Add(lck);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        public static List<TRSH_LOCK_Class> getPartLocks(int TRSH_DOOR_HWCATCODE, ref string errMsg)
        {
            try
            {
                DataRow[] rowsArray;
                string query = string.Format("TRSH_DOOR_HWCATCODE = '{0}'", TRSH_DOOR_HWCATCODE);
                if (dtLocks == null)
                {
                    errMsg = "dtLocks is null, maybe caused by page refresh, recreating it";
                    myLogger.log.Error(errMsg);
                    lstLocks = getLocks(ref errMsg);
                    dtLocks = lstLocks.ToDataTable<TRSH_LOCK_Class>();

                }
                rowsArray = PrApiCalls.dtLocks.Select(query);
                if (rowsArray.Length > 0)
                {
                    List<TRSH_LOCK_Class> lstPartLocks = new List<TRSH_LOCK_Class>();
                    TRSH_LOCK_Class emptyLock = new TRSH_LOCK_Class();
                    emptyLock.TRSH_LOCKNAME = " ";
                    emptyLock.TRSH_LOCKDES = " ";
                    lstPartLocks.Add(emptyLock);
                    for (int r = 0; r < rowsArray.Length; r++)
                    {
                        TRSH_LOCK_Class lock1 = new TRSH_LOCK_Class();
                        lock1.TRSH_LOCK = int.Parse(rowsArray[r]["TRSH_LOCK"].ToString());
                        lock1.TRSH_LOCKNAME = rowsArray[r]["TRSH_LOCKNAME"].ToString();
                        lock1.TRSH_LOCKDES = rowsArray[r]["TRSH_LOCKDES"].ToString();
                        lstPartLocks.Add(lock1);
                    }
                    return lstPartLocks;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_TILETYPES        -  70
        public static List<TILETYPE_Class> getTileTypes(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TILENAME,TILEDES";
                request.Resource = string.Format("TRSH_TILETYPES?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesTILETYPE_Class val = JsonConvert.DeserializeObject<ValuesTILETYPE_Class>(response.Content);
                    List<TILETYPE_Class> val1 = new List<TILETYPE_Class>();  //val.value;
                    TILETYPE_Class emptyTile = new TILETYPE_Class();
                    emptyTile.TILENAME = " ";
                    emptyTile.TILEDES = " ";
                    val1.Add(emptyTile);
                    foreach (TILETYPE_Class tile in val.value)
                    {
                        val1.Add(tile);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_RAW4CPLATES      - 110  ? (/HT = /HighTech) 
        public static List<RAW4CPLATES_Class> getRaw4CPlates(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "RAW4CPLATES,RAW4CPLATESNAME";
                request.Resource = string.Format("TRSH_RAW4CPLATES?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesRAW4CPLATES_Class val = JsonConvert.DeserializeObject<ValuesRAW4CPLATES_Class>(response.Content);
                    List<RAW4CPLATES_Class> val1 = new List<RAW4CPLATES_Class>();  //val.value;
                    RAW4CPLATES_Class emptyRaw4CP = new RAW4CPLATES_Class();
                    emptyRaw4CP.RAW4CPLATESNAME = " ";
                    val1.Add(emptyRaw4CP);
                    foreach (RAW4CPLATES_Class raw4cp in val.value)
                    {
                        val1.Add(raw4cp);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_WINDOWWIDTH      - 150
        public static List<WINDOWWIDTH_Class> getWindowWidths(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "WINDOWWIDTH_ID,PARTNAME,MINDOORWIDTH,MAXDOORWIDTH,WINDOWWIDTH";
                request.Resource = string.Format("TRSH_WINDOWWIDTH?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesWINDOWWIDTH_Class val = JsonConvert.DeserializeObject<ValuesWINDOWWIDTH_Class>(response.Content);
                    List<WINDOWWIDTH_Class> val1 = new List<WINDOWWIDTH_Class>();  //val.value;
                    WINDOWWIDTH_Class emptyV = new WINDOWWIDTH_Class();
                    //emptyV.PARTNAME = " ";
                    //val1.Add(emptyV);
                    foreach (WINDOWWIDTH_Class wwidth in val.value)
                    {
                        val1.Add(wwidth);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_WINDOWHEIGHT     - 140
        public static List<WINDOWHEIGHT_Class> getWindowHeights(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "WINDOWHEIGHT_ID,PARTNAME,MINDOORHEIGHT,MAXDOORHEIGHT,MEASURENAME,WINDOWHEIGHT,HEIGHT4DRIL";
                request.Resource = string.Format("TRSH_WINDOWHEIGHT?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesWINDOWHEIGHT_Class val = JsonConvert.DeserializeObject<ValuesWINDOWHEIGHT_Class>(response.Content);
                    List<WINDOWHEIGHT_Class> val1 = new List<WINDOWHEIGHT_Class>();  //val.value;
                                                                                     // WINDOWHEIGHT_Class emptyV = new WINDOWHEIGHT_Class();
                                                                                     // emptyV.WINDOWHEIGHTNAME = " ";
                                                                                     // emptyV.VITRAGEDES = " ";
                                                                                     // val1.Add(emptyV);
                    foreach (WINDOWHEIGHT_Class wh in val.value)
                    {
                        val1.Add(wh);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_PROFILE4WINDOW   - 160
        public static List<PROFILE4WINDOW_Class> getProfiles4Windows(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PROFILE4WINDOWNAME,PROFILE4WINDOWDES";
                request.Resource = string.Format("TRSH_PROFILE4WINDOW?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesPROFILE4WINDOW_Class val = JsonConvert.DeserializeObject<ValuesPROFILE4WINDOW_Class>(response.Content);
                    List<PROFILE4WINDOW_Class> val1 = new List<PROFILE4WINDOW_Class>();  //val.value;
                    PROFILE4WINDOW_Class emptyP = new PROFILE4WINDOW_Class();
                    emptyP.PROFILE4WINDOWNAME = " ";
                    emptyP.PROFILE4WINDOWDES = " ";
                    val1.Add(emptyP);
                    foreach (PROFILE4WINDOW_Class p4w in val.value)
                    {
                        val1.Add(p4w);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_GLASS4WINDOW     - 170
        public static List<GLASS4WINDOW_Class> getGlasses4Windows(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "GLASS4WINDOWID,GLASS4WINDOWDES,GLASS4WINDOWKITNAME";
                request.Resource = string.Format("TRSH_GLASS4WINDOW?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesGLASS4WINDOW_Class val = JsonConvert.DeserializeObject<ValuesGLASS4WINDOW_Class>(response.Content);
                    List<GLASS4WINDOW_Class> val1 = new List<GLASS4WINDOW_Class>();  //val.value;
                    GLASS4WINDOW_Class emptyG4w = new GLASS4WINDOW_Class();
                    emptyG4w.GLASS4WINDOWDES = " ";
                    emptyG4w.GLASS4WINDOWKITNAME = " ";
                    val1.Add(emptyG4w);
                    foreach (GLASS4WINDOW_Class g4w in val.value)
                    {
                        val1.Add(g4w);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_GRID             - 180
        public static List<GRID_Class> getGrids(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "GRID_ID,GRIDNAME,GRIDDES";
                request.Resource = string.Format("TRSH_GRID?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesGRID_Class val = JsonConvert.DeserializeObject<ValuesGRID_Class>(response.Content);
                    List<GRID_Class> val1 = new List<GRID_Class>();  //val.value;
                    GRID_Class emptyGrd = new GRID_Class();
                    emptyGrd.GRID_ID = 0;
                    emptyGrd.GRIDNAME = " ";
                    emptyGrd.GRIDDES = " ";
                    val1.Add(emptyGrd);
                    foreach (GRID_Class grd in val.value)
                    {
                        val1.Add(grd);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_VITRAGE          -  90
        public static List<VITRAGE_Class> getVitrages(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "VITRAGENAME,VITRAGEDES";
                request.Resource = string.Format("TRSH_VITRAGE?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesVITRAGE_Class val = JsonConvert.DeserializeObject<ValuesVITRAGE_Class>(response.Content);
                    List<VITRAGE_Class> val1 = new List<VITRAGE_Class>();  //val.value;
                    VITRAGE_Class emptyV = new VITRAGE_Class();
                    emptyV.VITRAGENAME = " ";
                    emptyV.VITRAGEDES = " ";
                    val1.Add(emptyV);
                    foreach (VITRAGE_Class vitr in val.value)
                    {
                        val1.Add(vitr);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }
        //TRSH_VITRAGE4DIAMOND  - 250
        public static List<VITRAGE4DIAMOND_Class> getVitrages4Diamond(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "VITRAGE4DIAMONDNAME,VITRAGE4DIAMONDDES";
                request.Resource = string.Format("TRSH_VITRAGE4DIAMOND?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesVITRAGE4DIAMOND_Class val = JsonConvert.DeserializeObject<ValuesVITRAGE4DIAMOND_Class>(response.Content);
                    List<VITRAGE4DIAMOND_Class> val1 = new List<VITRAGE4DIAMOND_Class>();  //val.value;
                    VITRAGE4DIAMOND_Class emptyV4D = new VITRAGE4DIAMOND_Class();
                    emptyV4D.VITRAGE4DIAMONDNAME = " ";
                    emptyV4D.VITRAGE4DIAMONDDES = " ";
                    val1.Add(emptyV4D);
                    foreach (VITRAGE4DIAMOND_Class v4d in val.value)
                    {
                        val1.Add(v4d);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_GRID4HT1084      - 370
        public static List<GRID4HT1084_Class> getGrid4HT1084(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "GRID4HT1084NAME,GRID4HT1084DES";
                request.Resource = string.Format("TRSH_GRID4HT1084?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesGRID4HT1084_Class val = JsonConvert.DeserializeObject<ValuesGRID4HT1084_Class>(response.Content);
                    List<GRID4HT1084_Class> val1 = new List<GRID4HT1084_Class>();  //val.value;
                    GRID4HT1084_Class empty1084 = new GRID4HT1084_Class();
                    empty1084.GRID4HT1084NAME = " ";
                    empty1084.GRID4HT1084DES = " ";
                    val1.Add(empty1084);
                    foreach (GRID4HT1084_Class g1084 in val.value)
                    {
                        val1.Add(g1084);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }

        //TRSH_HANDLES   - 350
        public static List<HANDLE_Class> getHandles(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "HANDLENAME,HANDLEDES";
                request.Resource = string.Format("TRSH_HANDLES?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesHANDLE_Class val = JsonConvert.DeserializeObject<ValuesHANDLE_Class>(response.Content);
                    List<HANDLE_Class> val1 = new List<HANDLE_Class>();  //val.value;
                    HANDLE_Class emptyHandle = new HANDLE_Class();
                    emptyHandle.HANDLENAME = " ";
                    emptyHandle.HANDLEDES = " ";
                    val1.Add(emptyHandle);
                    foreach (HANDLE_Class handle in val.value)
                    {
                        val1.Add(handle);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }


        //TRSH_HANDLE4DIAMOND   - 360
        public static List<HANDLE4DIAMOND_Class> getHandles4Diamond(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "HANDLE4DIAMONDNAME,HANDLE4DIAMONDDES";
                request.Resource = string.Format("TRSH_HANDLE4DIAMOND?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesHANDLE4DIAMOND_Class val = JsonConvert.DeserializeObject<ValuesHANDLE4DIAMOND_Class>(response.Content);
                    List<HANDLE4DIAMOND_Class> val1 = new List<HANDLE4DIAMOND_Class>();  //val.value;
                    HANDLE4DIAMOND_Class emptyHandle = new HANDLE4DIAMOND_Class();
                    emptyHandle.HANDLE4DIAMONDNAME = " ";
                    emptyHandle.HANDLE4DIAMONDDES = " ";
                    val1.Add(emptyHandle);
                    foreach (HANDLE4DIAMOND_Class handle in val.value)
                    {
                        val1.Add(handle);
                    }
                    return val1;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                throw ex;
            }
        }




        #endregion get tables data from priority
        #region form fields
        public static DataTable getAllMeagedFields(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "MEAGEDNAME,CONFIG_THNAME,CONFIG_TDNAME,FIELDCODE,CONFIG_FIELDNAME";
                request.Resource = string.Format("TRSH_MEAGEDFIELDS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesMeagedFields_Class val = JsonConvert.DeserializeObject<ValuesMeagedFields_Class>(response.Content);
                    List<MeagedFields_Class> val1 = new List<MeagedFields_Class>();  //val.value;
                    foreach (MeagedFields_Class fld in val.value)
                    {
                        val1.Add(fld);
                    }
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<MeagedFields_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    errMsg = "Check whether you are connected to Elidoor VPN";
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg += "\n response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        public static DataTable getDecorSideFlds(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "DECORSIDECODE,DECORSIDE,CONFIG_THNAME,CONFIG_TDNAME,FIELDCODE,CONFIG_FIELDNAME,SHOW";
                request.Resource = string.Format("TRSH_DECORSIDE_FLDS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDecorSideFlds_Class val = JsonConvert.DeserializeObject<ValuesDecorSideFlds_Class>(response.Content);
                    List<DecorSideFlds_Class> val1 = new List<DecorSideFlds_Class>();  //val.value;
                    foreach (DecorSideFlds_Class fld in val.value)
                    {
                        val1.Add(fld);
                    }
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<DecorSideFlds_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }

        public static DataTable getConfFields(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "FIELDID,FIELDCODE,CONFIG_THNAME,CONFIG_TDNAME,CONFIG_FIELDNAME,FIELDNAME,FIELDDES,FIELDDATATYPE,CONFIG_SUBFORM";
                request.Resource = string.Format("TRSH_CONF_FIELDS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesConfField_Class val = JsonConvert.DeserializeObject<ValuesConfField_Class>(response.Content);
                    List<ConfField_Class> val1 = new List<ConfField_Class>();  //val.value;
                    foreach (ConfField_Class fld in val.value)
                    {
                        val1.Add(fld);
                    }
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<ConfField_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        public static DataTable getDefaults(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "LINENUM,TRSH_NUM,PARTNAME,FIELDCODE,CONFIG_FIELDNAME,FIELDNAME,CONFIG_TDNAME,FIELDDATATYPE,DEFVAL,VAL_LOCKED,WRONGVAL";
                request.Resource = string.Format("TRSH_DEFAULTS?$select={0}", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDefaults_Class val = JsonConvert.DeserializeObject<ValuesDefaults_Class>(response.Content);
                    List<Defaults_Class> val1 = new List<Defaults_Class>();  //val.value;
                    foreach (Defaults_Class fld in val.value)
                    {
                        val1.Add(fld);
                    }
                    DataTable dt = new DataTable();
                    dt = val1.ToDataTable<Defaults_Class>();  //return val1;
                    return dt;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    myLogger.log.Error(errMsg);
                    return null;
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        public static PART_Class getPart(string PARTNAME, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PART,PARTNAME,PARTDES,MPARTNAME,FAMILYNAME,FAMILYDES,TRSH_DOOR_HWCATCODE";
                request.Resource = string.Format("LOGPART?$filter=PARTNAME eq '{0}'&$select={1}", PARTNAME, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesPART_Class val = JsonConvert.DeserializeObject<ValuesPART_Class>(response.Content);
                    //List<PART_Class> val1 = new List<PART_Class>();  //val.value;
                    return val.value[0];
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return null;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return null;
                }
            }
            catch (Exception ex)
            {

                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return null;
            }
        }
        public static string getMeagedOfPart(string PARTNAME, ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "PARTNAME,MPARTNAME";
                request.Resource = string.Format("LOGPART?$filter=PARTNAME eq '{0}'&$select={1}", PARTNAME, fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesPART_Class val = JsonConvert.DeserializeObject<ValuesPART_Class>(response.Content);
                    //List<PART_Class> val1 = new List<PART_Class>();  //val.value;
                    return val.value[0].MPARTNAME;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return string.Empty;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {

                errMsg = string.Format("Unexpected error: {0} - check whether you can connect to Priority server.  Stacktrace : {1}", ex.Message, ex.StackTrace);
                myLogger.log.Error(errMsg);
                return string.Empty;
            }
        }
        #endregion form fields
        #region get last TRSH_DOORCONFIG
        public static string getLastREFERENCE(ref string errMsg)
        {
            try
            {
                RestClient restClient = new RestClient();
                initRestClient(restClient);
                RestRequest request = new RestRequest();
                string fields = "TRSH_DOORCONFIG,REFERENCE,FORMDATE,FORMFILLER,CUSTDES,PARTNAME";
                request.Resource = string.Format("TRSH_DOORCONFIG?$orderby=TRSH_DOORCONFIG desc&$select={0}&$top=1", fields);
                IRestResponse response = restClient.Execute(request);
                if (response.IsSuccessful)
                {
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Include,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    ValuesDoorConfig val = JsonConvert.DeserializeObject<ValuesDoorConfig>(response.Content);
                    if (val == null || val.value == null)
                        return string.Empty;
                    else if (val != null && val.value != null && val.value.Count == 0)
                    {
                        myLogger.log.Error(string.Format("TRSH_DOORCONFIG is empty !"));
                        return string.Empty;
                    }
                    return val.value[0].REFERENCE;
                }
                else
                {
                    if (response.StatusDescription.ToLower() == "not found")
                    {
                        errMsg = "response.StatusDescription = 'Not Found' - check the restClient.BaseUrl - maybe it's wrong, e.g. double slashes or extra spaces somewhere !";
                        myLogger.log.Error(errMsg);
                        return string.Empty;
                    }
                    errMsg = string.Format("Priority Web API error : {0} \n {1}", response.StatusDescription, response.Content);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                myLogger.log.Error(string.Format("Unexpected error: {0}", ex.Message));
                return string.Empty;
            }
        }

        #endregion get last TRSH_DOORCONFIG

        public static string JsonSerializer<T>(T t)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, t);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return jsonString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static IRestResponse SendToPriority(string form, DoorConfig doorConfig, ref string errMsg)
        //HttpRequest httpRequest)
        {
            try
            {
                myLogger.log.Info("SendToPriority called");

                RestClient restClient = new RestClient();
                initRestClient(restClient);

                RestRequest request = new RestRequest();
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.Resource = form; //TRSH_DOORCONFIG   name of the form to populate

                //doorConfig.FORMDATE = "2022-02-24";  // just for test 

                string payload = JsonSerializer<DoorConfig>(doorConfig);


                //{"REFERENCE":null,"FORMDATE":"24-02-2022",  wrong date format - fails
                //"REFERENCE":null,"FORMDATE":"2022-02-24","FORMFILLER":null,"AGENT":0,"CUST":1, - works


                request.AddParameter("application/json", payload, ParameterType.RequestBody);

                IRestResponse response = restClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    errMsg = response.Content; //response.StatusDescription;
                    myLogger.log.Info(string.Format(" PrApiCalls : Api call failed : {0} , Payload : {1}, errorMessaage : {2} ; {3}",
                                   response.ResponseStatus, payload, response.ErrorMessage, response.ErrorException));
                    myLogger.log.Info(string.Format("REQUEST sent : Method : {0}, Resource: {1}, Parameters[0] :{2}",
                                   request.Method, request.Resource, request.Parameters[0]));

                }
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}