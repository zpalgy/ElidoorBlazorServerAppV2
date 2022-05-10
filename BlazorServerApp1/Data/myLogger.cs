using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Net;

namespace BlazorServerApp1.Data
{
    public class myLogger
    {
        public enum BrowserTypeEnum
        {
            IE,
            Chrome,
            Firefox,
            Unknown,
        }

        public static ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string DetermineCompName(string IP)
        {
            try
            {
                IPAddress myIP = IPAddress.Parse(IP);
                IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
                List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
                return compName.First();
            }
            catch (Exception ex)
            {
                return "**Unknown to DNS**";
            }
        }

        public static string DetermineCompName(HttpRequest Request)
        {
            try
            {
                string IP = Request.Host.ToString();//Request.UserHostName;
                string compName = DetermineCompName(IP);
                return compName;
            }
            catch (Exception ex)
            {
                return "**Unknown to DNS**";
            }
        }

        public static BrowserTypeEnum getBrowserType(HttpRequest Request)
        {
            //System.Web.HttpBrowserCapabilities browser = Request.Browser;

            //string browserType = browser.Type.ToLower();
            //if (browserType.StartsWith("chrome"))
            //    return BrowserTypeEnum.Chrome;
            //else if (browserType.StartsWith("internetexplorer"))
            //    return BrowserTypeEnum.IE;
            //else if (browserType.StartsWith("firefox"))
            //    return BrowserTypeEnum.Firefox;
            //else
            //    return BrowserTypeEnum.Unknown;

            string userAgent = "firefox";//Request.UserAgent; //browser.Type.ToLower();
            if (userAgent.ToLower().Contains("edge") || userAgent.ToLower().Contains("internetexplorer"))
                return BrowserTypeEnum.IE;
            else if (userAgent.ToLower().Contains("firefox"))
                return BrowserTypeEnum.Firefox;
            else if (userAgent.ToLower().Contains("chrome") && !userAgent.ToLower().Contains("edge"))
                return BrowserTypeEnum.Chrome;
            else
                return BrowserTypeEnum.Unknown;

        }

        public static void logCompName(HttpRequest Request)
        {
            string IP = Request.Host.ToString();   //UserHostName;
            string compName = DetermineCompName(IP);
            myLogger.log.Info(string.Format("copmuter name = {0}", compName));
        }
        public static string ClientInfo(HttpRequest Request)
        {
            try
            {
                string IP = Request.Host.ToString(); //Request.UserHostName;
                string compName = myLogger.DetermineCompName(IP);
                string URL = Request.Path.ToString();   //HttpContext.Current.Request.Url.AbsolutePath;
                string[] urlArr = URL.Split('/');
                URL = urlArr[urlArr.Length - 1];
                BrowserTypeEnum browserType = getBrowserType(Request);
                //return string.Format("SessionID= {0}, Client: {1} , URL : {2} Browser : {3} - ",
                //             HttpContext.Current.Session.SessionID, compName, URL, browserType.ToString());
                return string.Format("Client: {0} , URL : {1} Browser : {2} - ",
                              compName, URL, browserType.ToString());
            }
            catch (Exception ex)
            {
                return "*Client-Unknown-to-DNS*";
            }
        }
        //public static void ListActiveSessions()
        //{
        //    string outStr = " \r\n List of Active Sessions \r\n --------------------------- \n\r ";
        //    foreach (SessionInfo sessInfo in Global.Sessions)
        //    {
        //        outStr += string.Format(" \r\n {0} : {1} \r\n ", sessInfo.SessionId, sessInfo.compName);
        //    }
        //    myLogger.log.Info(string.Format("{0} \r\n --------------------------- \r\n ", outStr));
        //}
    }
}