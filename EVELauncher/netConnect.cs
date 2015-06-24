using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Net;
using System.Web;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Xml;
using System.Threading;

namespace EVELauncher
{
    class netConnect
    {
        //发送两次登陆请求
        public string getAccessToken(string userName, string userPass)
        {
            try
            {
                //第一个是POST请求，用于提交表单设置Cookie
                string loginPostString = "UserName=" + WebUtility.UrlEncode(userName) + "&Password=" + WebUtility.UrlEncode(userPass) + "&CaptchaToken=&Captcha=";
                byte[] loginPostByte = Encoding.UTF8.GetBytes(loginPostString);
                string firstLoginResponse;
                string accessToken;
                CookieContainer userCookieContainer = new CookieContainer();
                HttpWebRequest loginPostRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://auth.eve-online.com.cn/Account/LogOn?ReturnUrl=%2Foauth%2Fauthorize%3Fclient_id%3Deveclient%26scope%3DeveClientLogin%26response_type%3Dtoken%26redirect_uri%3Dhttps%253A%252F%252Fauth.eve-online.com.cn%252Flauncher%253Fclient_id%253Deveclient%26lang%3Dzh%26mac%3DNone"));
                loginPostRequest.CookieContainer = userCookieContainer;
                loginPostRequest.Method = "POST";
                loginPostRequest.ContentType = "application/x-www-form-urlencoded";
                loginPostRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                loginPostRequest.Headers.Add("Cache-Control", "max-age=0");
                loginPostRequest.Headers.Add("Origin", "https://auth.eve-online.com.cn");
                loginPostRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.15 Safari/535.11";
                loginPostRequest.Headers.Add("Refer", "https://auth.eve-online.com.cn/Account/LogOn?ReturnUrl=%2foauth%2fauthorize%3fclient_id%3deveclient%26scope%3deveClientLogin%26response_type%3dtoken%26redirect_uri%3dhttps%253A%252F%252Fauth.eve-online.com.cn%252Flauncher%253Fclient_id%253Deveclient%26lang%3dzh%26mac%3dNone");
                loginPostRequest.ContentLength = loginPostByte.Length;
                Stream loginPostStream = loginPostRequest.GetRequestStream();
                loginPostStream.Write(loginPostByte, 0, loginPostByte.Length);
                loginPostStream.Close();
                HttpWebResponse loginResponse = (HttpWebResponse)loginPostRequest.GetResponse();
                StreamReader loginResponseStreamReader = new StreamReader(loginResponse.GetResponseStream(), Encoding.UTF8);
                firstLoginResponse = loginResponseStreamReader.ReadToEnd();
                loginResponseStreamReader.Close();
                loginResponse.Close();
                loginPostStream.Close();
                //第二个是Get请求，用于获取Access-Token。
                HttpWebRequest loginGetRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://auth.eve-online.com.cn/oauth/authorize?client_id=eveclient&scope=eveClientLogin&response_type=token&redirect_uri=https%3A%2F%2Fauth.eve-online.com.cn%2Flauncher%3Fclient_id%3Deveclient&lang=zh&mac=None"));
                loginGetRequest.CookieContainer = userCookieContainer;
                loginGetRequest.Method = "GET";
                loginGetRequest.Accept = "text/html, application/xhtml+xml, */*";
                loginGetRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
                loginGetRequest.AllowAutoRedirect = false;
                loginGetRequest.Referer = "https://auth.eve-online.com.cn/Account/LogOn?ReturnUrl=%2foauth%2fauthorize%3fclient_id%3deveclient%26scope%3deveClientLogin%26response_type%3dtoken%26redirect_uri%3dhttps%253A%252F%252Fauth.eve-online.com.cn%252Flauncher%253Fclient_id%253Deveclient%26lang%3dzh%26mac%3dNone&client_id=eveclient&scope=eveClientLogin&response_type=token&redirect_uri=https%3A%2F%2Fauth.eve-online.com.cn%2Flauncher%3Fclient_id%3Deveclient&lang=zh&mac=None";
                HttpWebResponse getResponse = (HttpWebResponse)loginGetRequest.GetResponse();
                StreamReader secondLoginResponseStreamReader = new StreamReader(getResponse.GetResponseStream(), Encoding.UTF8);
                string secondLoginResponse = secondLoginResponseStreamReader.ReadToEnd();
                secondLoginResponseStreamReader.Close();
                getResponse.Close();
                Match accessTokenMatch = Regex.Match(secondLoginResponse, @"access.token.(.*).amp.token.type");
                accessToken = accessTokenMatch.Groups[1].Value;
                return accessToken;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "netErr";
            }
        }

        //获取XML
        public string getApiXML(string URL)
        {
            try
            {
                HttpWebRequest GetRequest = (HttpWebRequest)WebRequest.Create(new Uri(URL));
                GetRequest.Method = "GET";
                GetRequest.Accept = "text/html, application/xhtml+xml, */*";
                GetRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko";
                HttpWebResponse getResponse = (HttpWebResponse)GetRequest.GetResponse();
                StreamReader ResponseStreamReader = new StreamReader(getResponse.GetResponseStream(), Encoding.UTF8);
                string XMLResponse = ResponseStreamReader.ReadToEnd();
                ResponseStreamReader.Close();
                getResponse.Close();
                return XMLResponse;
            }
            catch (Exception)
            {
                return "netErr";
            }
        }
    }
}
