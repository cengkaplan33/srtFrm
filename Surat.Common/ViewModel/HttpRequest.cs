using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Surat.Common.ViewModel
{
    public class HttpRequestView
    {
        #region Browser
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string BrowserBetaVersion { get; set; }
        public string BrowserSupportCookies { get; set; }
        public string BrowserGatewayVersion { get; set; }
        public string BrowserInputType { get; set; }
        public string BrowserIsMobileDevice { get; set; }
        public string BrowserJavaApplets { get; set; }
        public string BrowserMobileDeviceManufacturer { get; set; }
        public string BrowserMobileDeviceModel { get; set; }
        public string BrowserPlatform { get; set; }
        public string BrowserRequestEncoding { get; set; }
        public string BrowserResponseEncoding { get; set; }
        public string BrowserScreenBitDepth { get; set; }
        public string BrowserScreenPixelHeight { get; set; }
        public string BrowserScreenPixelWidth { get; set; }
        public string BrowserWin32 { get; set; }
        #endregion
        
        #region Url
        public string UrlAbsolutePath {get;set;}
        public string UrlAbsoluteUri {get;set;}
        public string UrlAuthority {get;set;}
        public string UrlDnsSafeHost {get;set;}
        public string UrlFragment {get;set;}
        public string UrlHost {get;set;}
        public string UrlLocalPath {get;set;}
        public string UrlPort {get;set;}
        public string UrlQuery {get;set;}
        public string [] UrlSegments {get;set;}

        public string UrlReferrerAbsolutePath { get; set; }
        public string UrlReferrerAbsoluteUri { get; set; }
        public string UrlReferrerAuthority { get; set; }
        public string UrlReferrerDnsSafeHost { get; set; }
        public string UrlReferrerFragment { get; set; }
        public string UrlReferrerHost { get; set; }
        public string UrlReferrerLocalPath { get; set; }
        public string UrlReferrerPort { get; set; }
        public string UrlReferrerQuery { get; set; }
        public string[] UrlReferrerSegments { get; set; }
        #endregion
        
        #region User
        public string UserAgent { get; set; }
        public string UserHostAddress { get; set; }
        public string UserHostName { get; set; }
        public string[] UserLanguages { get; set; }
        #endregion

        #region Other
        public string[] HttpCookieCollection { get; set; }
        public string HttpMethod { get; set; }
        public string IsAuthenticated { get; set; }
        public string IsSecureConnection { get; set; }
        public string SessionKeyValue { get; set; }
        public string Path { get; set; }
        public string PathInfo { get; set; }
        public string RawUrl { get; set; }
        public string RequestType { get; set; }
        #endregion

        public HttpRequestView()
        {
        }

        public HttpRequestView(HttpRequest request)
        {
            #region Browser
            this.BrowserName = request.Browser.Browser;
            this.BrowserVersion = request.Browser.Version;
            this.BrowserBetaVersion = request.Browser.Beta.ToString();
            this.BrowserSupportCookies = request.Browser.Cookies.ToString();
            this.BrowserGatewayVersion = request.Browser.GatewayVersion;
            this.BrowserInputType = request.Browser.InputType;
            this.BrowserIsMobileDevice = request.Browser.IsMobileDevice.ToString();
            this.BrowserJavaApplets = request.Browser.JavaApplets.ToString();
            this.BrowserMobileDeviceManufacturer = request.Browser.MobileDeviceManufacturer;
            this.BrowserMobileDeviceModel = request.Browser.MobileDeviceModel;
            this.BrowserPlatform = request.Browser.Platform;
            this.BrowserRequestEncoding = request.Browser.PreferredRequestEncoding;
            this.BrowserResponseEncoding = request.Browser.PreferredResponseEncoding;
            this.BrowserScreenBitDepth = request.Browser.ScreenBitDepth.ToString();
            this.BrowserScreenPixelHeight = request.Browser.ScreenPixelsHeight.ToString();
            this.BrowserWin32 = request.Browser.Win32.ToString();
            #endregion

            #region Log Cookie
            List<string> tempCookieKeyValue = new List<string>();
            for (int i = 0; i < request.Cookies.AllKeys.Count(); i++)
            {
                var cookie = request.Cookies[request.Cookies.AllKeys[i]];
                tempCookieKeyValue.Add(string.Format("CookieName:{0}::CookieValue:{1}",cookie.Name,cookie.Value));
            }
            this.HttpCookieCollection = tempCookieKeyValue.ToArray();
            #endregion

            #region Url
            this.UrlAbsolutePath = request.Url.AbsolutePath;
            this.UrlAbsoluteUri = request.Url.AbsoluteUri;
            this.UrlAuthority = request.Url.Authority;
            this.UrlDnsSafeHost = request.Url.DnsSafeHost;
            this.UrlFragment = request.Url.Fragment;
            this.UrlHost = request.Url.Host;
            this.UrlLocalPath = request.Url.LocalPath;
            this.UrlPort = request.Url.Port.ToString();
            this.UrlQuery = request.Url.Query;
            this.UrlSegments = request.Url.Segments;

            if (request.UrlReferrer != null)
            {
                this.UrlReferrerAbsolutePath = request.UrlReferrer.AbsolutePath;
                this.UrlReferrerAbsoluteUri = request.UrlReferrer.AbsoluteUri;
                this.UrlReferrerAuthority = request.UrlReferrer.Authority;
                this.UrlReferrerDnsSafeHost = request.UrlReferrer.DnsSafeHost;
                this.UrlReferrerFragment = request.UrlReferrer.Fragment;
                this.UrlReferrerHost = request.UrlReferrer.Host;
                this.UrlReferrerLocalPath = request.UrlReferrer.LocalPath;
                this.UrlReferrerPort = request.UrlReferrer.Port.ToString();
                this.UrlReferrerQuery = request.UrlReferrer.Query;
                this.UrlReferrerSegments = request.UrlReferrer.Segments;
            }
            
            #endregion

            #region User
            this.UserAgent = request.UserAgent;
            this.UserHostAddress = request.UserHostAddress;
            this.UserHostName = request.UserHostName;
            this.UserLanguages = request.UserLanguages;
            #endregion

            #region Other
            this.HttpMethod = request.HttpMethod;
            this.IsAuthenticated = request.IsAuthenticated.ToString();
            this.IsSecureConnection = request.IsSecureConnection.ToString();
            this.Path = request.Path;
            this.PathInfo = request.PathInfo;
            this.RawUrl = request.RawUrl;
            this.RequestType = request.RequestType;
            #endregion
        }
    }
}
