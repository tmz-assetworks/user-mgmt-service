using Azure;
using Azure.Identity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using UsersService.Api.DataModel;
using UsersService.Api.Model;
using UsersService.Api.Responce;
using UsersService.Application.Commands.AuthPassword;
using UsersService.Application.Queries;
using UsersService.Core.Response;
using UsersService.Infrastructure.Helpers;
using VerifyAssetWorksAzureAD.Model;
using ClientCredential = Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential;
using Extensions = UsersService.Infrastructure.Helpers.Extensions;

namespace UsersService.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IConfiguration _baseconfiguration;
        private readonly IMediator _mediator;
        protected readonly UsersService.Infrastructure.DBContext.DBContextCore _dbContext;
        private static Random random = new Random();
        string JSONString = String.Empty;
        private readonly IConfidentialClientApplication _capp;

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
        public AuthController(IConfiguration configuration, IMediator mediator, UsersService.Infrastructure.DBContext.DBContextCore dbContext)
        {
            _dbContext = dbContext;
            _baseconfiguration = configuration;
            _configuration = configuration;
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
                    {"flag:Emailflag", Environment.GetEnvironmentVariable("FLAG_EMAILFLAG")},
                    {"MailSettings:UserName", Environment.GetEnvironmentVariable("MAIL_USERNAME")},
                    {"MailSettings:Password", Environment.GetEnvironmentVariable("MAIL_PASSWORD")},
                    {"MailSettings:Host", Environment.GetEnvironmentVariable("MAIL_HOST")},
                    {"MailSettings:Port", Environment.GetEnvironmentVariable("MAIL_PORT")},
                    {"BaseUrl:fronendurl", Environment.GetEnvironmentVariable("BASEURL_FRONTED")},
                };
            if (Environment.GetEnvironmentVariable("AZUREAD_CID") != null)
                _baseconfiguration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();

            var authority = $"https://login.microsoftonline.com/{this._baseconfiguration["AzureAd:TenantId"]}/v2.0";
            _capp = ConfidentialClientApplicationBuilder
            .Create(_baseconfiguration["AzureAd:clientId"])
            .WithClientSecret(_baseconfiguration["AzureAd:clientSecret"])
            .WithAuthority(authority)
            .Build();
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
                var userprincipal = await Username(emailid);
                HttpClient httpClient = new HttpClient();
                StringContent stringContent = new StringContent(System.Text.Json.JsonSerializer.Serialize<String>(""), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAdminAccessToken().Result));
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", userprincipal.useremail + "/revokeSignInSessions"), stringContent);

                str = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseMessage.IsSuccessStatusCode)
                {
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
            var otptoken = resetPassword.accesstoken;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_baseconfiguration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(otptoken);
            var objemail = jwtSecurityToken.Claims.First(claim => claim.Type == "email").Value;
            var objiss = jwtSecurityToken.Claims.First(claim => claim.Type == "iss").Value;
            var objexp = jwtSecurityToken.Claims.First(claim => claim.Type == "exp").Value;
            var objaud = jwtSecurityToken.Claims.First(claim => claim.Type == "aud").Value;

            long timestamp = Convert.ToInt64(objexp);
            DateTime compareTo = DateTime.UtcNow;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            int result = DateTime.Compare(dateTimeOffset.UtcDateTime, compareTo);
            string str = "";
            if (string.IsNullOrEmpty(resetPassword.emailid) || string.IsNullOrEmpty(resetPassword.password))
            {
                str = @"{""StatusCode"":""400"",""Message"":""Bad Request""}";
                return BadRequest(str);
            }
            try
            {
                if (result >= 0)
                {
                    UserNModel userN = new UserNModel();
                    PasswordProfile passwordProfile = new PasswordProfile();
                    passwordProfile.ForceChangePasswordNextSignIn = false;
                    passwordProfile.Password = EncryptDecrypt.Decrypt(resetPassword.password, _baseconfiguration["EncryptDecryptkey"]);

                    userN.passwordProfile = passwordProfile;
                    HttpClient httpClient = new HttpClient();
                    StringContent stringContent = new StringContent(System.Text.Json.JsonSerializer.Serialize<UserNModel>(userN), Encoding.UTF8, "application/json");
                    httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAdminAccessToken().Result));
                    HttpResponseMessage httpResponseMessage = await httpClient.PatchAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", objemail), stringContent);

                    str = await httpResponseMessage.Content.ReadAsStringAsync();
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        str = @"{""StatusCode"":""200"",""Message"":""Password Reset Success"", ""Data"":""" + str + @"""}";

                        return Ok(str);
                    }
                    else
                    {
                        str = @"{""StatusCode"":""" + ((int)httpResponseMessage.StatusCode).ToString() + @""",""Message"":""Password Reset error"",""Data"":""" + str + @"""}";
                        return StatusCode((int)httpResponseMessage.StatusCode, str);
                    }
                }
                else
                {
                    str = @"{""StatusCode"":""400"",""Message"":""Bad Request"",""Data"":""" + "Token expire" + @"""}";
                    return BadRequest(str);
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                str = @"{""StatusCode"":""400"",""Message"":""Bad Request"",""Data"":""" + ex.Message + @"""}";
                return BadRequest(str);
            }
        }
        [HttpGet]
        [Route("VerifyUserByOTP")]
        public async Task<IActionResult> VerifyUserByOTP(string emailid, string OTP)
        {
            OtpResponse OtpResponse = new OtpResponse();
            OtpResponse.resettoken = null;
            var userprincipal = await Username(emailid);
            var result = await _mediator.Send(new otpverifyQuery(userprincipal.useremail, OTP));
            string responce = string.Empty;
            if (string.IsNullOrEmpty(emailid) || string.IsNullOrEmpty(OTP))
            {
                OtpResponse.StatusCode = 400;
                OtpResponse.Message = "Username or otp blank";
                return BadRequest(OtpResponse);
            }
            try
            {
                if (OTP == result.otp)
                {
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAccessToken().Result));
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", userprincipal.useremail));
                    Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection myContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection { System.Net.Mime.MediaTypeNames.Application.Json };
                    string str = await httpResponseMessage.Content.ReadAsStringAsync();

                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        OtpResponse.StatusCode = 200;
                        OtpResponse.Message = "Otp Validated Success";
                        OtpResponse.resettoken = GenerateotpToken(userprincipal.useremail);
                        return Ok(OtpResponse);
                    }
                    else
                    {
                        OtpResponse.StatusCode = (int)httpResponseMessage.StatusCode;
                        OtpResponse.Message = "User not found";
                        return StatusCode((int)httpResponseMessage.StatusCode, OtpResponse);
                    }
                }
                else
                {
                    OtpResponse.StatusCode = 400;
                    OtpResponse.Message = "Otp not Valid";
                    return BadRequest(OtpResponse);
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                OtpResponse.StatusCode = 400;
                OtpResponse.Message = "Bad Request";
                return BadRequest(OtpResponse);
            }

        }
        //[HttpGet]
        //[Route("VerifyUser")]
        //public async Task<IActionResult> VerifyUser(string emailid)
        //{

        //    string callingMethod = APIConstant.CreateNotificationWithoutToken;
        //    TaskNotificationRequest taskNotificationRequest = new TaskNotificationRequest();
        //    MailResponse mailresponse = new MailResponse();
        //    var userprincipal = await Username(emailid);
        //    string responce = string.Empty;
        //    string rotp = Extensions.Getrandomnumber().ToString();
        //    var result = await _mediator.Send(new UpdateOtpQuery(userprincipal.useremail, rotp));
        //    if (string.IsNullOrEmpty(emailid))
        //    {
        //        responce = @"{""StatusCode"":""400"",""Message"":""" + "Username blank" + @""",""data"": [{""token"":""" + null + @"""}]}";
        //        return BadRequest(responce);

        //    }
        //    try
        //    {
        //        UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(_baseconfiguration, _dbContext);
        //        mailresponse = mailService.SendEmailUserVerify(emailid, rotp, userprincipal.name);
        //        HttpClient httpClient = new HttpClient();
        //        // StringContent stringContent = new StringContent(JsonSerializer.Serialize<UserNModel>(userN), Encoding.UTF8, "application/json");
        //        httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAccessToken()));
        //        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", userprincipal.useremail));
        //        //------insert notification-------------
        //        taskNotificationRequest.category = "Email";
        //        taskNotificationRequest.messagetype = mailresponse.Subject;
        //        taskNotificationRequest.content = mailresponse.Body;
        //        taskNotificationRequest.ipaddress = "192.186.178.07";
        //        taskNotificationRequest.userId = userprincipal.objid;

        //        Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection myContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection { System.Net.Mime.MediaTypeNames.Application.Json };
        //        responce = await httpResponseMessage.Content.ReadAsStringAsync();
        //        if (httpResponseMessage.IsSuccessStatusCode)
        //        {

        //            responce = @"{""StatusCode"":""200"",""Message"":""Verify Success"",""data"":" + responce + "}";
        //            try
        //            {
        //                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(taskNotificationRequest), Encoding.UTF8, "application/json");
        //                HttpResponseMessage response = await Helper.GetCallAssetWithBody1APIAsync(callingMethod, httpContent);
        //            }
        //            catch (Exception ex)
        //            {
        //                Log.Information("error occurred :" + ex.Message);
        //                return Ok(responce);
        //            }

        //            return Ok(responce);
        //        }
        //        else
        //        {
        //            responce = @"{""StatusCode"":""400"",""Message"":""User not found"",""data"":" + responce + "}";

        //            return BadRequest(responce);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        Log.Information("error occurred :" + ex.Message);
        //        responce = @"{""StatusCode"":""400"",""Message"":""" + "Bad Request ," + ex.Message.ToString() + @"""}";
        //        return BadRequest(responce);
        //    }

        //}

        [HttpGet]
        [Route("VerifyUser")]
        public async Task<IActionResult> VerifyUser(string emailid)
        {

            string callingMethod = APIConstant.CreateNotificationWithoutToken;
            TaskNotificationRequest taskNotificationRequest = new TaskNotificationRequest();
            MailResponse mailresponse = new MailResponse();
            string responce = string.Empty;
            if (string.IsNullOrEmpty(emailid))
            {
                responce = @"{""StatusCode"":""400"",""Message"":""" + "Emailid blank" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);

            }
            var userprincipal = await Username(emailid);
            if(string.IsNullOrEmpty(userprincipal.objid))
            {
                responce = @"{""StatusCode"":""400"",""Message"":""" + "Username blank" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);
            }
            string rotp = Extensions.Getrandomnumber().ToString();
            var result = await _mediator.Send(new UpdateOtpQuery(userprincipal.useremail, rotp));
            try
            {
                UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(_baseconfiguration, _dbContext);
                mailresponse = mailService.SendEmailUserVerify(emailid, rotp, userprincipal.name);
                HttpClient httpClient = new HttpClient();
                // StringContent stringContent = new StringContent(JsonSerializer.Serialize<UserNModel>(userN), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAccessToken().Result));
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", userprincipal.useremail));
                //------insert notification-------------
                taskNotificationRequest.category = "Email";
                taskNotificationRequest.messagetype = mailresponse.Subject;
                taskNotificationRequest.content = mailresponse.Body;
                taskNotificationRequest.ipaddress = "192.186.178.07";
                taskNotificationRequest.userId = userprincipal.objid;

                Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection myContentTypes = new Microsoft.AspNetCore.Mvc.Formatters.MediaTypeCollection { System.Net.Mime.MediaTypeNames.Application.Json };
                responce = await httpResponseMessage.Content.ReadAsStringAsync();
                if (httpResponseMessage.IsSuccessStatusCode)
                {

                    responce = @"{""StatusCode"":""200"",""Message"":""Verify Success"",""data"":" + responce + "}";
                    try
                    {
                        StringContent httpContent = new StringContent(JsonConvert.SerializeObject(taskNotificationRequest), Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await Helper.GetCallAssetWithBody1APIAsync(callingMethod, httpContent);
                    }
                    catch (Exception ex)
                    {
                        Log.Information("error occurred :" + ex.Message);
                        return Ok(responce);
                    }

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
                Log.Information("error occurred :" + ex.Message);
                responce = @"{""StatusCode"":""400"",""Message"":""" + "Bad Request ," + ex.Message.ToString() + @"""}";
                return BadRequest(responce);
            }

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] AuthModel authparam)
        {
            authparam.password = EncryptDecrypt.Decrypt(authparam.password, _baseconfiguration["EncryptDecryptkey"]);
            return await Auth(authparam);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task ChangeUserAppRole()
        {
            // Azure AD App details
            var utenantId = "";
            var uclientId = "";
            var uclientSecret = "";

            var appId = ""; // Service principal ID
            var userId = ""; // Azure AD user object ID
            var newAppRoleId = ""; // The GUID of the "Admin" app role

            var credential = new ClientSecretCredential(
            utenantId, uclientId, uclientSecret
        );

            var graphClient = new GraphServiceClient(credential);

            // 1. Remove existing app role assignments
            var existingAssignments = await graphClient.Users[userId].AppRoleAssignments
                .Request()
                .Filter($"resourceId eq {appId}")
                .GetAsync();

            foreach (var assignment in existingAssignments)
            {
                await graphClient.Users[userId].AppRoleAssignments[assignment.Id]
                    .Request()
                    .DeleteAsync();
            }

            // 2. Assign new role
            var newAssignment = new AppRoleAssignment
            {
                PrincipalId = Guid.Parse(userId),
                ResourceId = Guid.Parse(appId),
                AppRoleId = Guid.Parse(newAppRoleId)
            };

            await graphClient.Users[userId].AppRoleAssignments
                .Request()
                .AddAsync(newAssignment);

            Console.WriteLine("User's app role changed successfully.");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Auth")]
        public async Task<IActionResult> Auth([FromBody] AuthModel authparam)
        {
            var userprincipal = await Username(authparam.username);
            string responce = string.Empty;
            if (string.IsNullOrEmpty(authparam.username) || string.IsNullOrEmpty(authparam.password))
            {
                responce = @"{""StatusCode"":""400"",""Message"":""" + "UserName or Password are blank" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);
            }
            try
            {
                if (userprincipal.isActive == true)
                {
                    string clientid = this._baseconfiguration["AzureAd:clientId"];
                    string TenantId = this._baseconfiguration["AzureAd:TenantId"];
                    string clientsecret = this._baseconfiguration["AzureAd:clientSecret"];
                    HttpClient httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAccessToken().Result));
                    string str1 = string.Concat("https://login.microsoftonline.com/", TenantId, "/oauth2/token?api-version=1.0");
                    List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                    list.Add(new KeyValuePair<string, string>("resource", clientid));
                    list.Add(new KeyValuePair<string, string>("client_id", clientid));
                    list.Add(new KeyValuePair<string, string>("client_secret", clientsecret));
                    list.Add(new KeyValuePair<string, string>("grant_type", "password"));
                    list.Add(new KeyValuePair<string, string>("username", userprincipal.useremail));
                    list.Add(new KeyValuePair<string, string>("password", authparam.password));
                    list.Add(new KeyValuePair<string, string>("scope", "openid User.Read"));
                    List<KeyValuePair<string, string>> list1 = list;
                    FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(list1);
                    HttpResponseMessage result = httpClient.PostAsync(str1, formUrlEncodedContent).Result;

                    if (result.IsSuccessStatusCode)
                    {
                        responce = result.Content.ReadAsStringAsync().Result;
                        var res = TokenResponse(responce);
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
                else
                {
                    responce = @"{""StatusCode"":""" + 400 + @""",""Message"":""" + "User InActive" + @""",""data"":" + responce + "}";
                    return BadRequest(responce);
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
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
                string rClientid = this._baseconfiguration["AzureAd:clientId"];
                string rTenantId = this._baseconfiguration["AzureAd:TenantId"];
                string rClientsecret = this._baseconfiguration["AzureAd:clientSecret"];
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAccessToken().Result));
                string str1 = string.Concat("https://login.microsoftonline.com/", rTenantId, "/oauth2/token?api-version=1.0");
                List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
                list.Add(new KeyValuePair<string, string>("resource", rClientid));
                list.Add(new KeyValuePair<string, string>("client_id", rClientid));
                list.Add(new KeyValuePair<string, string>("client_secret", rClientsecret));
                list.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
                list.Add(new KeyValuePair<string, string>("refresh_token", refreshToken));
                list.Add(new KeyValuePair<string, string>("scope", "openid"));
                List<KeyValuePair<string, string>> list1 = list;
                FormUrlEncodedContent formUrlEncodedContent = new FormUrlEncodedContent(list1);
                HttpResponseMessage result = httpClient.PostAsync(str1, formUrlEncodedContent).Result;

                if (result.IsSuccessStatusCode)
                {
                    responce = result.Content.ReadAsStringAsync().Result;
                    var res = TokenResponse(responce);
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
                Log.Information("error occurred :" + ex.Message);
                responce = @"{""StatusCode"":""400"",""Message"":""" + "Bad Request" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);
            }

        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<string> GetAccessToken()
        {
            string accessToken = string.Empty;
            try
            {
                string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
                Microsoft.Identity.Client.AuthenticationResult tokenresult = await _capp.AcquireTokenForClient(scopes).ExecuteAsync();
                accessToken = tokenresult.AccessToken;
            }
            catch (Exception exception)
            {
                Log.Information("error occurred in token access :" + exception.Message);
            }
            return accessToken;
        }
        [ApiExplorerSettings(IgnoreApi = true)]

        [NonAction]
        private string GenerateotpToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_baseconfiguration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,  "Reset Token"),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(_baseconfiguration["Jwt:Issuer"],
                _baseconfiguration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<string> GetAdminAccessToken()
        {
            var Tenant_Id = _baseconfiguration["AzureAd:TenantId"];
            var Client_Id = _baseconfiguration["AzureAd:clientId"];
            var Client_Secret = _baseconfiguration["AzureAd:clientSecret"];
            using (var client = new HttpClient())
            {
                var token_Endpoint = $"https://login.microsoftonline.com/{Tenant_Id}/oauth2/v2.0/token";
                var form_Data = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", Client_Id },
                    { "client_secret", Client_Secret },
                    { "scope", "https://graph.microsoft.com/.default" }
                };
                var form_content = new FormUrlEncodedContent(form_Data);
                var tokenResponse = await client.PostAsync(token_Endpoint, form_content);
                var tokenResponseString = await tokenResponse.Content.ReadAsStringAsync();
                if (!tokenResponse.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
                var json = JObject.Parse(tokenResponseString);
                return json["access_token"].ToString();
            }
        }


        [NonAction]
        public async Task<EmailResponse> Username(String usermail)
        {
            EmailResponse EmailResponse = new EmailResponse();
            try
            {
                var res = await _mediator.Send(new GetUserEmailQuery(usermail));
                return res;
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                JSONString = "{\n  \"data\" : " + null + ",  \"StatusMessage\" : " + ex.Message.ToString() + ",\n  \"StatusCode\" : " + (int)HttpStatusCode.NotFound + " \n}";
            }
            return EmailResponse;
        }

        [NonAction]
        public AuthResponce TokenResponse(String responce)
        {
            try
            {
                JObject jObjr = JObject.Parse(responce);
                AuthResponce res = new AuthResponce()
                {
                    StatusCode = 200,
                    Message = "Login Success",
                    data = new List<AuthData>() {
                            new AuthData() {
                                access_token=jObjr["access_token"].ToString(),
                                id_token= jObjr["id_token"].ToString(),
                                refresh_token=jObjr["refresh_token"].ToString(),
                                resource="tokenString",
                                token_type="Bearer"
                            }
                       }
                };
                return res;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
