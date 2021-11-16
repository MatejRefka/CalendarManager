using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using CalendarManagerLibrary;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using CalendarManager.Data;

namespace CalendarManager.Controllers {

    /// <summary>
    /// Helper Controller handling OAuth 2.0 authorization protocol.
    /// Documentation: https://developers.google.com/identity/protocols/oauth2/web-server
    /// </summary>


    public class AuthorizationController : Controller {

        #region Helper fields
        private IConfiguration config;
        private StateToken stateToken;
        private IWebHostEnvironment environment;
        private ApplicationDbContext context;
        #endregion Helper fields

        #region Constructor
        public AuthorizationController(IConfiguration config, IWebHostEnvironment environment, StateToken stateToken, ApplicationDbContext context) {

            this.environment = environment;
            this.config = config;
            this.stateToken = stateToken;
            this.context = context;
        }
        #endregion Constructor


        #region Token request to Google Authorization Server
        //On user sign in, send a request token for authorization and user authentication to Google Authorization Server.
        public IActionResult RequestToken() {

            String requestTokenRedirectURL =
                "https://accounts.google.com/o/oauth2/v2/auth?" +
                "scope=https://www.googleapis.com/auth/calendar.events&" +
                "access_type=offline&" +
                "include_granted_scopes=true&" + 
                "response_type=code&" +
                "state=" + stateToken.GenerateToken() + "&" +
                "redirect_uri=https://localhost:5001/Authorization/RedirectFromAuthorizationServer&" +
                "client_id=" + config["Authentication:Google:ClientId"];
            
            return Redirect(requestTokenRedirectURL);
        }
        #endregion Token request to Google Authorization Server

        #region Response from Google Authorization Server 
        //Upon successfull or unsucessfull authorization, the action captures and processes authorization code, error and state token.
        public IActionResult RedirectFromAuthorizationServer(String code, String error, String state) {

            //if the code parameter is non-null then the authentication is sucessfull and the authorization code is valid
            if (code!=null && state==stateToken.GetTokenValue()){

                return ExchangeRequest(code);
                //return RedirectToAction("GetEvents", "GoogleAPI");
            }

            //otherwise the user authentication or permission authorization is unsuccessful 
            else {

                //ERROR
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion Response from Google Authorization Server

        #region Exhange authorization code for access token and refresh token from Google API Exchange Server
        //posting data to a Web server: https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-send-data-using-the-webrequest-class
        public IActionResult ExchangeRequest(String code) {

            String exchangeRequestURL = "https://oauth2.googleapis.com/token";
            
            //WebRequest class is needed to capture the JSON response
            WebRequest request = WebRequest.Create(exchangeRequestURL);

            request.Method = "POST";

            String postData =
                "code=" + code + "&" +
                "client_id=" + config["Authentication:Google:ClientId"].ToString() + "&" +
                "client_secret=" + config["Authentication:Google:ClientSecret"].ToString() + "&" +
                "redirect_uri=https://localhost:5001/Authorization/RedirectFromAuthorizationServer&" +
                "grant_type=authorization_code";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try {

                WebResponse response = request.GetResponse();

                //read the response data into a string
                Stream responseDataStream = response.GetResponseStream();
                
                StreamReader reader = new StreamReader(responseDataStream);
                string jsonResponse = reader.ReadToEnd();

                response.Close();

                //collect the access token and refresh token
                JObject json = JObject.Parse(jsonResponse);
                string accessToken = (string)json.SelectToken("access_token");
                string refreshToken = (string)json.SelectToken("refresh_token");


                
                System.IO.File.WriteAllText(Path.Combine(environment.ContentRootPath,"Files","AccessToken.txt"), accessToken);
                
                /*if (refreshToken != null) { 
                System.IO.File.WriteAllText(Path.Combine(environment.ContentRootPath,"Files","RefreshToken.txt"), refreshToken);
                }
                */

                TempData["tempAccessToken"] = accessToken;
                TempData["tempRefreshToken"] = refreshToken;

                return RedirectToAction("Index", "Calendar");

            }
            catch (Exception) {

                
            }

            //If bad request error is caught, return Error View.
            return RedirectToAction("Index", "Home");

        }
        #endregion Exhange authorization code for access token and refresh token from Google API Exchange Server


        

    }
}
