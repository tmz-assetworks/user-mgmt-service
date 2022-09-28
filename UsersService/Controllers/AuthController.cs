using Azure.Core;
using Azure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.ExternalConnectors;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using UsersService.Api.DataModel;
using UsersService.Api.Model;
using UsersService.Api.Responce;
using UsersService.Application.Commands.AuthPassword;
using UsersService.Application.Commands.Users;
using UsersService.Application.Queries;
using UsersService.Core.Entities;
using UsersService.Core.Response;
using UsersService.Responses.Users;
using VerifyAssetWorksAzureAD.Model;
using ClientCredential = Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential;

namespace UsersService.Api.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IConfiguration _configuration;

        private readonly IConfiguration _baseconfiguration;
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;
        private static Random random = new Random();
        string JSONString = String.Empty;
        string getjson(object res)
        {
            string JSONString = String.Empty;
            if (res != null)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var data = System.Text.Json.JsonSerializer.Serialize(res, options);

                JSONString = "{\n  \"StatusCode\" : " + (int)HttpStatusCode.OK + ",\n  \"StatusMessage\" : \"Record found\",\n  \"data\" : " + data + " \n}";
            }
            else
            {
                JSONString = "{\n  \"StatusCode\" : " + (int)HttpStatusCode.NotFound + ",\n  \"StatusMessage\" : \"Record not found\",\n  \"data\" : " + null + " \n}";
            }
            return JSONString;
        }
        public AuthController(IConfiguration configuration, IMediator mediator)
        {
           
            _baseconfiguration = configuration;
            _configuration=configuration;
            _mediator = mediator;
            Dictionary<string, string> myConfiguration = new Dictionary<string, string>
                {
                    {"AzureAd:clientId", Environment.GetEnvironmentVariable("AZUREAD_CID")},
                    {"AzureAd:TenantId", Environment.GetEnvironmentVariable("AZUREAD_TID")},
                    {"AzureAd:clientSecret",Environment.GetEnvironmentVariable("AZUREAD_CLIENT_SECRET")},
                    {"AzureAd:Instance",Environment.GetEnvironmentVariable("AZUREAD_INSTANCE")},
                    {"EncryptDecryptkey",Environment.GetEnvironmentVariable("DECRYPTKEY")},
                    {"Jwt:Key",Environment.GetEnvironmentVariable("JWT_KEY")},
                    {"Jwt:Issuer", Environment.GetEnvironmentVariable("JWT_ISSUER")},
                    {"Jwt:Audience", Environment.GetEnvironmentVariable("JWT_AUD")},
                };
            _baseconfiguration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout(string emailid)
        {
            string str = "";
            if (string.IsNullOrEmpty(emailid))
            {
                str = @"{""StatusCode"":""400"",""Message"":""Bad Request""}";
                return BadRequest(str);
            }
            try
            {
                HttpClient httpClient = new HttpClient();
            StringContent stringContent = new StringContent(JsonSerializer.Serialize<String>(""), Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAdminAccessToken()));
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", emailid + "/revokeSignInSessions"), stringContent);

             str = await httpResponseMessage.Content.ReadAsStringAsync();
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                //String hardCodedJson = @"{""StatusCode"":""200"",""Message"":""token successfuly generated""}";

                return Ok(str);
            }
            else
            {

                    str = @"{""StatusCode"":""" + ((int)httpResponseMessage.StatusCode).ToString() + @""",""Message"":""User not found""}";
                    return StatusCode((int)httpResponseMessage.StatusCode, str);

                }
            }
            catch (Exception ex)
            {

                str = @"{""StatusCode"":""400"",""Message"":""Bad Request"",""Data"":""" + ex.Message + @"""}";
                return BadRequest(str);
            }

        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ResetPassword resetPassword)
        {
            string str = "";
            if (string.IsNullOrEmpty(resetPassword.emailid) || string.IsNullOrEmpty(resetPassword.password))
            {
                 str = @"{""StatusCode"":""400"",""Message"":""Bad Request""}";
                return BadRequest( str);
            }
            try
            {
            UserNModel userN = new UserNModel();
            PasswordProfile passwordProfile = new PasswordProfile();
            passwordProfile.ForceChangePasswordNextSignIn=false;
            passwordProfile.Password= EncryptDecrypt.Decrypt(resetPassword.password, _baseconfiguration["EncryptDecryptkey"]);

            userN.passwordProfile = passwordProfile;
            HttpClient httpClient = new HttpClient();
            StringContent stringContent = new StringContent(JsonSerializer.Serialize<UserNModel>(userN), Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization=(new AuthenticationHeaderValue("Bearer", GetAdminAccessToken()));
            HttpResponseMessage httpResponseMessage = await httpClient.PatchAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", resetPassword.emailid), stringContent);
            
            str = await httpResponseMessage.Content.ReadAsStringAsync();
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                str = @"{""StatusCode"":""200"",""Message"":""Password Reset Success"", ""Data"":""" + str + @"""}";

                return Ok(str);
            }
            else
            {
                str = @"{""StatusCode"":"""+ ((int)httpResponseMessage.StatusCode).ToString() + @""",""Message"":""Password Reset error"",""Data"":"""+ str +@"""}";
                return StatusCode((int)httpResponseMessage.StatusCode,str);
            }
            }
            catch (Exception ex)
            {

                str = @"{""StatusCode"":""400"",""Message"":""Bad Request"",""Data"":""" + ex.Message + @"""}";
                return BadRequest(str);
            }

        }
        [HttpGet]
        [Route("VerifyUserByOTP")]
        public async Task<IActionResult> VerifyUserByOTP(string emailid,string OTP)
        {
            
            string responce = string.Empty;
            if (string.IsNullOrEmpty(emailid) || string.IsNullOrEmpty(OTP))
            {
                responce = @"{""StatusCode"":""400"",""Message"":""" + "Username or otp blank" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);

            }
            try
            {
                if (OTP == "123456")
                {

                    HttpClient httpClient = new HttpClient();
                    // StringContent stringContent = new StringContent(JsonSerializer.Serialize<UserNModel>(userN), Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAccessToken()));
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", emailid));

                    Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection myContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection { System.Net.Mime.MediaTypeNames.Application.Json };
                    string str = await httpResponseMessage.Content.ReadAsStringAsync();
                
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        responce = @"{""StatusCode"":""200"",""Message"":""Otp Validated Success""}";
                    
                         return Ok(responce);
                    }
                    else
                    {
                        responce = @"{""StatusCode"":"""+ ((int)httpResponseMessage.StatusCode).ToString() + @""",""Message"":""User not found""}";
                        return StatusCode((int)httpResponseMessage.StatusCode, responce);
                    }
                }
                else
                {
                    responce = @"{""StatusCode"":""400"",""Message"":""Otp not Valid""}";
                    return BadRequest(responce);
                }
            }
            catch (Exception ex)
            {

                responce = @"{""StatusCode"":""400"",""Message"":""" + "Bad Request" + @"""}";
                return BadRequest(responce);
            }

        }
        [HttpGet]
        [Route("VerifyUser")]
        public async Task<IActionResult> VerifyUser(string emailid)
        {
            string responce = string.Empty;
            if (string.IsNullOrEmpty(emailid))
            {
                responce = @"{""StatusCode"":""400"",""Message"":""" + "Username blank" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);

            }
            try
            {
                MailRequest request = new MailRequest();
                request.ToEmail = "ashu.setiya@assetworks.com";
                request.frommail = "mamta.mishra@assetworks.com";

                //request.ToEmail = "vijay.mani@tekmindz.com";
                //request.frommail = "ashu.setiya@assetworks.com";


                request.Body = "Your OTP Is:" + " " + "123456";
                request.Subject = "OTP";

                UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(_configuration);
                await mailService.SendEmailAsync(request);
                HttpClient httpClient = new HttpClient();
           // StringContent stringContent = new StringContent(JsonSerializer.Serialize<UserNModel>(userN), Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAccessToken()));
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", emailid));
            
            Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection myContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection { System.Net.Mime.MediaTypeNames.Application.Json };
             responce = await httpResponseMessage.Content.ReadAsStringAsync();
            if (httpResponseMessage.IsSuccessStatusCode)
            {
               
                responce = @"{""StatusCode"":""200"",""Message"":""Verify Success"",""data"":" + responce + "}";

                return Ok(responce);
            }
            else
            {
                    responce = @"{""StatusCode"":""400"",""Message"":""User not found"",""data"":" + responce + "}";

                    return BadRequest(responce);
            }
            }
            catch (Exception ex)
            {

                responce = @"{""StatusCode"":""400"",""Message"":""" + "Bad Request ," + ex.Message.ToString()  + @"""}";
                return BadRequest(responce);
            }

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] AuthModel authparam)
        {

            string responce = string.Empty;
            if (string.IsNullOrEmpty(authparam.username) || string.IsNullOrEmpty(authparam.password))
            {
               
                responce = @"{""StatusCode"":""400"",""Message"":""" + "UserName or Password are blank" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);

            }
            try
            {
                string clientid = this._baseconfiguration["AzureAd:clientId"];
                string TenantId = this._baseconfiguration["AzureAd:TenantId"];
                string clientsecret = this._baseconfiguration["AzureAd:clientSecret"];
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", this.GetAccessToken()));
                string str1 = string.Concat("https://login.microsoftonline.com/", TenantId, "/oauth2/token?api-version=1.0");
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("resource", "https://graph.microsoft.com"));
                list.Add(new KeyValuePair<string, string>("client_id", clientid));
                list.Add(new KeyValuePair<string, string>("client_secret", clientsecret));
                list.Add(new KeyValuePair<string, string>("grant_type", "password"));
                list.Add(new KeyValuePair<string, string>("username", authparam.username));
                list.Add(new KeyValuePair<string, string>("password", EncryptDecrypt.Decrypt(authparam.password, _baseconfiguration["EncryptDecryptkey"])));
                list.Add(new KeyValuePair<string, string>("scope", "openid profile User.Read"));
                 //list.Add(new KeyValuePair<string, string>("scope", "openid,api://7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed/access_as_user"));
                List<KeyValuePair<string, string>> list1 = list;
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(list1);
                HttpResponseMessage result = httpClient.PostAsync(str1, formUrlEncodedContent).Result;

                if (result.IsSuccessStatusCode)
                {
                   
                    responce = result.Content.ReadAsStringAsync().Result;

                    JObject jObj = JObject.Parse(responce);
                    string id_token_AD = jObj["id_token"].ToString();
                    var tokenString = GenerateJSONWebToken( id_token_AD);
                    
                    AuthResponce res= new AuthResponce() {
                       StatusCode = 200,
                       Message= "Login Success",
                       data=new List<AuthData>() { 
                       new AuthData() { 
                       access_token=jObj["access_token"].ToString(),
                       id_token=tokenString,
                       refresh_token=jObj["refresh_token"].ToString(),
                       resource="tokenString",
                       token_type="Bearer"
                       }
                       }

                    };

                    //response = jObj["access_token"].ToString();
                   // responce = @"{""StatusCode"":""200"",""Message"":""Login Success"",""Token"":"""+ tokenString + @""",""data"":" + responce + "}";

                    return Ok(res);
                }
                else
                {

                    responce = result.Content.ReadAsStringAsync().Result;
                    string errorMessage = "";
                    try
                    {
                        errorMessage = JObject.Parse(responce)["error_codes"].ToString();
                    }
                    catch (Exception ex) { }

                    if (errorMessage.Contains("50126"))
                    {
                        errorMessage = "Invalid password";

                    }
                    else if (errorMessage.Contains("50034"))
                    {
                        errorMessage = "Invalid Username";

                    }
                    responce = @"{""StatusCode"":""" + result.StatusCode.ToString() + @""",""Message"":""" + errorMessage + @""",""data"":" + responce + "}";

                    return StatusCode((int)result.StatusCode, responce);
                }
            }
            catch (Exception ex)
            {

                responce = @"{""StatusCode"":""400"",""Message"":""" + "Bad Request" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);
            }

        }

        // POST api/<AuthController>
        [HttpGet]
        [Route("AuthRefresh")]
        public async Task<IActionResult> AuthRefresh(string refreshToken)
        {
            string responce = string.Empty;
            if (string.IsNullOrEmpty(refreshToken))
            {
                responce = @"{""StatusCode"":""400"",""Message"":""" + "refreshToken blank" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);

            }
            try
            { 
                string clientid = this._baseconfiguration["AzureAd:clientId"];
                string TenantId = this._baseconfiguration["AzureAd:TenantId"];
                string clientsecret = this._baseconfiguration["AzureAd:clientSecret"];
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", this.GetAccessToken()));
                string str1 = string.Concat("https://login.microsoftonline.com/", TenantId, "/oauth2/token?api-version=1.0");
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("resource", "https://graph.microsoft.com"));
                list.Add(new KeyValuePair<string, string>("client_id", clientid));
                list.Add(new KeyValuePair<string, string>("client_secret", clientsecret));
                list.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
                list.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));
                list.Add(new KeyValuePair<string, string>("scope", "openid"));
                List<KeyValuePair<string, string>> list1 = list;
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(list1);
                HttpResponseMessage result = httpClient.PostAsync(str1, formUrlEncodedContent).Result;

                if (result.IsSuccessStatusCode)
                {
                    //responce = result.Content.ReadAsStringAsync().Result;
                    //responce = @"{""StatusCode"":""200"",""Message"":""Refresh token Success"",""data"":" + responce + "}";
                    //return Ok(responce);
                        responce = result.Content.ReadAsStringAsync().Result;

                        JObject jObj = JObject.Parse(responce);
                        string id_token_AD = jObj["id_token"].ToString();
                        var tokenString = GenerateJSONWebToken( id_token_AD);

                        AuthResponce res = new AuthResponce()
                        {
                            StatusCode = 200,
                            Message = "Login Success",
                            data = new List<AuthData>() {
                       new AuthData() {
                       access_token=jObj["access_token"].ToString(),
                       id_token=tokenString,
                       refresh_token=jObj["refresh_token"].ToString(),
                       resource="tokenString",
                       token_type="Bearer"
                       }
                       }

                        };

                        //response = jObj["access_token"].ToString();
                        // responce = @"{""StatusCode"":""200"",""Message"":""Login Success"",""Token"":"""+ tokenString + @""",""data"":" + responce + "}";

                        return Ok(res);
                    
                }
                else
                {
                    responce = result.Content.ReadAsStringAsync().Result;
                    return StatusCode((int)result.StatusCode, responce);
                }
            }
            catch (Exception ex)
            {

                responce = @"{""StatusCode"":""400"",""Message"":""" + "Bad Request" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);
            }

        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public string GetAccessToken()
        {
            string accessToken;
            string context = "https://login.microsoftonline.com/744aa8b0-bb99-4982-903f-52328216b4be";
            string resource = "https://graph.microsoft.com";
            string clientId = _baseconfiguration["AzureAd:clientId"];
            
            ClientCredential clientCredential = new ClientCredential(clientId, this._baseconfiguration["AzureAd:clientSecret"]);
            AuthenticationContext authenticationContext = new AuthenticationContext(context);
            try
            {
                string accessToken1 = authenticationContext.AcquireTokenAsync(resource, clientCredential).Result.AccessToken;
                accessToken = accessToken1;
            }
            catch (Exception exception)
            {
                authenticationContext = new AuthenticationContext(context);
                accessToken = this.GetAccessToken();
            }
            return accessToken;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        private string GenerateJSONWebToken(string id_token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_baseconfiguration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(id_token);
            var objectrioleid = jwtSecurityToken.Claims.First(claim => claim.Type == "oid").Value;
            GetUserobjectbyidDT res = _mediator.Send(new GetbyUserobjectID(objectrioleid)).Result;
            
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,  jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value),
                new Claim(JwtRegisteredClaimNames.Email, jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value),
                new Claim("roles",res.data.Rolename.ToString()),
                new Claim("cid",res.data.cID.ToString()),
                new Claim("unique_name", jwtSecurityToken.Claims.First(claim => claim.Type == "unique_name").Value),
                new Claim("oid", jwtSecurityToken.Claims.First(claim => claim.Type == "oid").Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
                      
            var token = new JwtSecurityToken(_baseconfiguration["Jwt:Issuer"],
                _baseconfiguration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [ApiExplorerSettings(IgnoreApi =true)]
        [NonAction]
        public string GetAdminAccessToken()
        {
            string item = this._baseconfiguration["AzureAd:clientId"];
            string str = this._baseconfiguration["AzureAd:TenantId"];
            string item1 = this._baseconfiguration["AzureAd:clientSecret"];
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", this.GetAccessToken()));
            string str1 = string.Concat("https://login.microsoftonline.com/", str, "/oauth2/token?api-version=1.0");
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            list.Add(new KeyValuePair<string, string>("resource", "https://graph.microsoft.com"));
            list.Add(new KeyValuePair<string, string>("client_id", item));
            list.Add(new KeyValuePair<string, string>("client_secret", item1));
            list.Add(new KeyValuePair<string, string>("grant_type", "password"));
            list.Add(new KeyValuePair<string, string>("username", "superadmin@devopstekmindz.onmicrosoft.com"));
            list.Add(new KeyValuePair<string, string>("password", "Vpm@2820"));
            //list.Add(new KeyValuePair<string, string>("scope", "openid"));
            list.Add(new KeyValuePair<string, string>("scope", item+ "/.default"));
            List<KeyValuePair<string, string>> list1 = list;
            FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(list1);
            HttpResponseMessage result = httpClient.PostAsync(str1, formUrlEncodedContent).Result;
            string response = string.Empty;
            if (result.IsSuccessStatusCode)
            {
                response = result.Content.ReadAsStringAsync().Result;
                //JValue jal = new JValue(response);
                JObject jObj = JObject.Parse(response);
                response = jObj["access_token"].ToString();

            }
            string str2 = response;

            return str2;
        }

       

    }

}
