using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UsersService.Api.DataModel;
using UsersService.Api.Model;
using UsersService.Application.Commands.AuthPassword;
using UsersService.Application.Commands.Users;
using UsersService.Application.Queries;
using UsersService.Core.Queries;
using UsersService.Core.Response;
using UsersService.Infrastructure.EnumData;
using UsersService.Infrastructure.Helpers;
using UsersService.Responses.Users;
using System.IdentityModel.Tokens.Jwt;
using Serilog;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using System.Dynamic;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore;

namespace UsersService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _baseconfiguration;
        private readonly ILogger<UserController> _logger;
        protected readonly UsersService.Infrastructure.DBContext.DBContextCore _dbContext;
        TokenBase _tokenBase;
        string JSONString = String.Empty;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IConfidentialClientApplication _app;

        public UserController(IConfiguration configuration, IMediator mediator, ILogger<UserController> logger, TokenBase tokenBase, IWebHostEnvironment hostEnvironment, UsersService.Infrastructure.DBContext.DBContextCore dbContext)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _logger = logger;
            _baseconfiguration = configuration;
            _tokenBase = tokenBase;
            webHostEnvironment = hostEnvironment;
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
                    {"AzureAd:resourceId", Environment.GetEnvironmentVariable("AZUREAD_RESOURCEID")},
                    {"AzureAd:operatorRoleId", Environment.GetEnvironmentVariable("AZUREAD_OPERATORRID")},
                    {"AzureAd:adminRoleId", Environment.GetEnvironmentVariable("AZUREAD_ADMINRID")},
                    {"LOG:CONNECTIONSTRING", Environment.GetEnvironmentVariable("LOG_CONNECTIONSTRING")},
                    {"AzureAd:ImageContainerName", Environment.GetEnvironmentVariable("AZUREAD_IMAGECONTAINERNAME")},
                    {"AzureAd:Domain", Environment.GetEnvironmentVariable("AZUREAD_DOMAIN")},
                    {"flag:Emailflag", Environment.GetEnvironmentVariable("FLAG_EMAILFLAG")},
                    {"MailSettings:UserName", Environment.GetEnvironmentVariable("MAIL_USERNAME")},
                    {"MailSettings:Password", Environment.GetEnvironmentVariable("MAIL_PASSWORD")},
                    {"MailSettings:Host", Environment.GetEnvironmentVariable("MAIL_HOST")},
                    {"MailSettings:Port", Environment.GetEnvironmentVariable("MAIL_PORT")},
                    {"BaseUrl:fronendurl", Environment.GetEnvironmentVariable("BASEURL_FRONTED")},
                    {"AzureAd:audience", Environment.GetEnvironmentVariable("AZUREAD_AUD")},

                };
            if (Environment.GetEnvironmentVariable("AZUREAD_CID") != null)
                _baseconfiguration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
            var authority = $"https://login.microsoftonline.com/{this._baseconfiguration["AzureAd:TenantId"]}/v2.0";
            _app = ConfidentialClientApplicationBuilder
            .Create(_baseconfiguration["AzureAd:clientId"])
            .WithClientSecret(_baseconfiguration["AzureAd:clientSecret"])
            .WithAuthority(authority)
            .Build();
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<string> GetAccessToken()
        {
            string accessToken = string.Empty;
            string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
            try
            {
                Microsoft.Identity.Client.AuthenticationResult result = await _app.AcquireTokenForClient(scopes).ExecuteAsync();
                accessToken = result.AccessToken;
            }
            catch (Exception exception)
            {
                Log.Information("error occurred :" + exception.Message);
            }
            return accessToken;
        }
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

        [HttpPost("GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AllUserResponse>> GetUsers(GetUserRequest UserRequest)
        {
            _tokenBase.acces_token = HttpContext == null ? _tokenBase.acces_token : await HttpContext.GetTokenAsync("access_token");
            string responseobjectid = Convert.ToString(_tokenBase.getobjectid());
            AllUserResponse alluserResp = new AllUserResponse();
            try
            {
                if (UserRequest.PageSize == 0) UserRequest.PageSize = 10;
                if (UserRequest.PageNumber == 0) UserRequest.PageNumber = 1;
                UserRequest.opratorid = responseobjectid;
                var res = await _mediator.Send(new GetAllUserQuery(UserRequest));
                alluserResp.data = res.data;
                alluserResp.statusData = new List<StatusData>();
                if (res != null && res.data.Count > 0)
                {
                    List<StatusData> statusData = new List<StatusData>()
                    {
                       new StatusData { Key = Status_Indication.CustomerActiveInActive.TotalUser.GetEnumDisplayName(), Value = res!=null? (res.Active + res.InActive).ToString():"" , Color = CustomerStatusColor.TotalUser.GetEnumDisplayName() },
                       new StatusData { Key = Status_Indication.CustomerActiveInActive.Active.GetEnumDisplayName(), Value = res!=null? res.Active.ToString():"" , Color = CustomerStatusColor.Active.GetEnumDisplayName() },
                        new StatusData { Key = Status_Indication.CustomerActiveInActive.InActive.GetEnumDisplayName(), Value = res!=null? res.InActive.ToString():"" , Color = CustomerStatusColor.InActive.GetEnumDisplayName() },
                        };
                    alluserResp.statusData = statusData;
                    alluserResp.StatusCode = (int)HttpStatusCode.OK;
                    alluserResp.StatusMessage = "Record Found";
                    alluserResp.Active = res.Active;
                    alluserResp.InActive = res.InActive;
                    alluserResp.paginationResponse = new Core.PagingHelper.PaginationResponse
                    {
                        TotalCount = alluserResp.data.TotalCount,
                        PageSize = alluserResp.data.PageSize,
                        CurrentPage = alluserResp.data.CurrentPage,
                        TotalPages = alluserResp.data.TotalPages,
                        HasNext = alluserResp.data.HasNext,
                        HasPrevious = alluserResp.data.HasPrevious
                    };
                }
                else
                {
                    alluserResp.StatusCode = (int)HttpStatusCode.OK;
                    alluserResp.StatusMessage = "Record not Found";
                    alluserResp.data = null;
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                alluserResp.StatusCode = (int)HttpStatusCode.BadRequest;
                alluserResp.StatusMessage = "Bad Request";
                alluserResp.data = null;
                _logger.LogError(ex.ToString());
            }
            return alluserResp;
        }
        [HttpGet("GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<GetUserResponseDT> GetUserById(int id)
        {
            GetUserResponseDT getUserResponse = new GetUserResponseDT();
            try
            {
                _tokenBase.acces_token = HttpContext == null ? _tokenBase.acces_token : await HttpContext.GetTokenAsync("access_token");
                GetUserResponseDT res = await _mediator.Send(new GetByIdUserQuery(id));
                return res;
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                JSONString = "{\n  \"data\" : " + null + ",  \"StatusMessage\" : " + ex.Message.ToString() + ",\n  \"StatusCode\" : " + (int)HttpStatusCode.NotFound + " \n}";
            }
            return getUserResponse;
        }
        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserCommand command)
        {
            if (command == null)
            {
                return BadRequest(@"{""StatusCode"":""400"",""Message"":""Invalid request data""}");
            }
            UserResponse resp = new UserResponse();

            string userprincipal = "";
            _tokenBase.acces_token = HttpContext == null ? _tokenBase.acces_token : await HttpContext.GetTokenAsync("access_token");
            MailResponse mailresponse = new MailResponse();
            string callingMethod = APIConstant.InsTaskNotification;
            TaskNotificationRequest taskNotificationRequest = new TaskNotificationRequest();
            if (!string.IsNullOrEmpty(command.name))
            {
                if (command.name.Contains(' '))
                    userprincipal = command.name.ToString().Split(' ')[0].Trim() + "@" + this._baseconfiguration["AzureAd:Domain"];
                else userprincipal = command.name + "@" + this._baseconfiguration["AzureAd:Domain"];
            }
            string responce = string.Empty;
            if (string.IsNullOrEmpty(command.DisplayName) || string.IsNullOrEmpty(userprincipal))
            {
                responce = @"{""StatusCode"":""400"",""Message"":""" + "Invalid User Details" + @"""}";
                return BadRequest(responce);
            }
            try
            {
                string nikname = "";
                if (!string.IsNullOrEmpty(command.name))
                {
                    if (command.name.Contains(' '))
                        nikname = command.name.ToString().Split(' ')[0].Trim();
                    else nikname = command.name;
                }
                UserModelAD userN = new UserModelAD()
                {
                    displayName = command.name,
                    accountEnabled = command.isActive,
                    userPrincipalName = userprincipal,
                    mailNickname = nikname,
                    mail = command.EmailId,
                    passwordProfile = new PasswordProfile()
                    {
                        Password = "User@" + DateTime.Now.Ticks.ToString(),
                        ForceChangePasswordNextSignIn = true
                    }
                };
                HttpClient httpClient = new HttpClient();
                StringContent stringContent = new StringContent(System.Text.Json.JsonSerializer.Serialize<UserModelAD>(userN), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAccessToken().Result));
                HttpResponseMessage result = await httpClient.PostAsync("https://graph.microsoft.com/v1.0/users", stringContent);
                responce = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    responce = result.Content.ReadAsStringAsync().Result;
                    responce = @"{""StatusCode"":""200"",""Message"":""User Created Successfuly"",""data"":" + responce + "}";
                    dynamic data = JObject.Parse(responce);
                    string responseobjectid = Convert.ToString(data["data"]["id"]);
                    if (string.IsNullOrEmpty(command.PhoneNumber.ToString()) || string.IsNullOrEmpty(command.EmailId))
                    {
                        responce = @"{""StatusCode"":""400"",""Message"":""" + "Invalid User Details" + @"""}";
                        return BadRequest(responce);
                    }
                    AssignRoleModel assignRole1 = new AssignRoleModel();
                    assignRole1.resourceId = _baseconfiguration["AzureAd:resourceId"];
                    assignRole1.principalId = responseobjectid;
                    var userRole = command.UserRolesCommand;
                    string userRoleName = _tokenBase.getrole();

                    bool isAdmin = string.Equals(userRoleName, "admin", StringComparison.OrdinalIgnoreCase);
                    bool isSuperAdmin = string.Equals(userRoleName, "superadmin", StringComparison.OrdinalIgnoreCase);

                    if ((isAdmin || isSuperAdmin) && userRole[0].Roleid != 4)
                    {
                        assignRole1.appRoleId = _baseconfiguration["AzureAd:adminRoleId"];
                    }
                    else if (isAdmin && userRole[0].Roleid == 4)
                    {
                        assignRole1.appRoleId = _baseconfiguration["AzureAd:operatorRoleId"];
                    }
                    StringContent stringRoleContent = new StringContent(System.Text.Json.JsonSerializer.Serialize<AssignRoleModel>(assignRole1), Encoding.UTF8, "application/json");
                    HttpResponseMessage httpResponseMessage = await httpClient.PostAsync("https://graph.microsoft.com/v1.0/users/" + responseobjectid + "/appRoleAssignments", stringRoleContent);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Record Insertion :" + httpResponseMessage);
                        command.objectid = responseobjectid;
                        command.userPrincipalName = userprincipal;
                        var resultRes = await _mediator.Send(command);
                        Console.WriteLine("Record Saved :" + httpResponseMessage);

                        if (resultRes != null)
                        {
                            resp.StatusCode = (int)HttpStatusCode.OK;
                            resp.StatusMessage = "Record saved successfully";
                            resp.Id = resultRes.Id;
                            UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(_baseconfiguration, _dbContext);
                            Console.WriteLine("Send Mail :" + (nikname, userprincipal, command.EmailId, long.Parse(resultRes.OTP)));
                            mailresponse = mailService.SendEmail(nikname, userprincipal, command.EmailId, long.Parse(resultRes.OTP));
                            //------insert notification-------------
                            taskNotificationRequest.category = "Email";
                            taskNotificationRequest.messagetype = mailresponse.Subject;
                            taskNotificationRequest.content = mailresponse.Body;
                            taskNotificationRequest.ipaddress = "192.186.178.07";
                            taskNotificationRequest.userId = _tokenBase.getobjectid();
                            try
                            {
                                StringContent httpContent = new StringContent(JsonConvert.SerializeObject(taskNotificationRequest), Encoding.UTF8, "application/json");
                                HttpResponseMessage response = await Helper.GetCallAssetWithBodyAuthAPIAsync(callingMethod, httpContent, _tokenBase.acces_token);
                            }
                            catch (Exception ex)
                            {
                                Log.Information("error occurred :" + ex.Message);
                                return Ok(resp);
                            }
                        }
                        else
                        {
                            resp.StatusCode = (int)HttpStatusCode.OK;
                            resp.StatusMessage = "Record not updated";
                            resp.Id = resultRes.Id;
                        }
                        return Ok(resp);
                    }
                    else
                    {
                        var roleResult = await httpResponseMessage.Content.ReadAsStringAsync();
                        return BadRequest(roleResult);
                    }
                }
                else
                {
                    responce = result.Content.ReadAsStringAsync().Result;
                    string errorMessage = ParseAzureResponse(responce);
                    resp.StatusCode = (int)result.StatusCode;
                    resp.StatusMessage = errorMessage;
                    return Ok(resp);
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                responce = @"{""StatusCode"":""400"",""Message"":""" + "Bad Request" + @""",""data"": [{""token"":""" + null + @"""}]}";
                return BadRequest(responce);
            }
        }

        /// <summary>
        /// Parsing response for duplicate User
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string ParseAzureResponse(string json)
        {
            JObject responseData = JObject.Parse(json);
            
            var error = responseData["error"];
            if (error == null) return "";
            var details = error["details"] as JArray;
            if (details != null && details.HasValues)
            {
                foreach (var detail in details)
                {

                    string target = (string?)detail?["target"] ?? string.Empty;
                    if (target.Contains("proxyAddresses", StringComparison.OrdinalIgnoreCase)) return "Provide an email address not already in use.";
                    if (target.Contains("userPrincipalName", StringComparison.OrdinalIgnoreCase)) return "Select a username that is not already in use. Do not include spaces or special characters.";
                }
            }

            return (string?)error?["message"] ?? "User creation failed.";
        }



        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
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
            AuthController authController = new AuthController(_baseconfiguration, _mediator, _dbContext);
            var userprincipal = await authController.Username(resetPassword.emailid);
            string str = "";
            if (string.IsNullOrEmpty(resetPassword.emailid) || string.IsNullOrEmpty(resetPassword.password))
            {
                str = @"{""StatusCode"":""400"",""Message"":""Bad Request""}";
                return BadRequest(str);
            }
            try
            {
                UserNModel userN = new UserNModel();
                PasswordProfile passwordProfile = new PasswordProfile();
                passwordProfile.ForceChangePasswordNextSignIn = false;
                passwordProfile.Password = EncryptDecrypt.Decrypt(resetPassword.password, _baseconfiguration["EncryptDecryptkey"]);
                userN.passwordProfile = passwordProfile;
                HttpClient httpClient = new HttpClient();
                StringContent stringContent = new StringContent(System.Text.Json.JsonSerializer.Serialize<UserNModel>(userN), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Authorization = (new AuthenticationHeaderValue("Bearer", GetAdminAccessToken().Result));
                HttpResponseMessage httpResponseMessage = await httpClient.PatchAsync(string.Concat("https://graph.microsoft.com/v1.0/users/", userprincipal.useremail), stringContent);
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
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                str = @"{""StatusCode"":""400"",""Message"":""Bad Request"",""Data"":""" + ex.Message + @"""}";
                return BadRequest(str);
            }
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UpdateUserResponse>> UpdateUser([FromBody] UpdateUserCommand command)
        {
            UpdateUserResponse resp = new UpdateUserResponse();
            try
            {
                if (command.Id != 0)
                {
                    var result = await _mediator.Send(command);
                    if (result != null)
                    {
                        resp.StatusCode = result.StatusCode;
                        resp.StatusMessage = result.StatusMessage;
                        resp.Id = result.Id;
                    }
                    else
                    {
                        resp.StatusCode = (int)HttpStatusCode.OK;
                        resp.StatusMessage = "Record not updated";
                        resp.Id = result.Id;
                    }
                }
                else
                {
                    resp.StatusCode = (int)HttpStatusCode.OK;
                    resp.StatusMessage = "Please enter valid UserID";
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = "Exception",
                    StatusCode = 404,
                    Content = "User not Update "
                };
            }
        }
        [HttpDelete("DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetUserResponseDT>> DeleteUser([FromBody] DeleteUserCommand command)
        {
            GetUserResponseDT getUserResponseDT = new GetUserResponseDT();
            try
            {
                if (command.Id != 0)
                {
                    var result = await _mediator.Send(command);
                    if (result != null)
                    {
                        getUserResponseDT.StatusCode = (int)HttpStatusCode.OK;
                        getUserResponseDT.StatusMessage = "Record updated";
                    }
                    else
                    {
                        getUserResponseDT.StatusCode = (int)HttpStatusCode.OK;
                        getUserResponseDT.StatusMessage = "Record not updated";
                    }
                }
                else
                {
                    getUserResponseDT.StatusCode = (int)HttpStatusCode.OK;
                    getUserResponseDT.StatusMessage = "Please enter valid UserID";
                }
                return Ok(getUserResponseDT);
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = "Exception",
                    StatusCode = 404,
                    Content = "User not Delete "
                };
            }
        }

        [HttpGet("UserbyobjectID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<GetUserobjectbyidDT> GetUserByobjectid(string objectid)
        {
            GetUserobjectbyidDT getUserResponse = new GetUserobjectbyidDT();
            try
            {
                GetUserobjectbyidDT res = await _mediator.Send(new GetbyUserobjectID(objectid));
                return res;
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                JSONString = "{\n  \"data\" : " + null + ",  \"StatusMessage\" : " + ex.Message.ToString() + ",\n  \"StatusCode\" : " + (int)HttpStatusCode.NotFound + " \n}";
                _logger.LogError(ex.ToString());

            }
            return getUserResponse;
        }
        [HttpGet("CustomerDDL")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerDDL>> CustomerDDL()
        {
            CustomerDDL allCustomerDDL = new CustomerDDL();
            try
            {
                var res = await _mediator.Send(new CustomerDDLQuery());
                if (res != null)
                {
                    allCustomerDDL.StatusCode = (int)HttpStatusCode.OK;
                    allCustomerDDL.StatusMessage = "Record Found";
                    allCustomerDDL.data = res;
                }
                else
                {
                    allCustomerDDL.StatusCode = (int)HttpStatusCode.OK;
                    allCustomerDDL.StatusMessage = "Record not Found";
                    allCustomerDDL.data = null;
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                allCustomerDDL.StatusCode = (int)HttpStatusCode.BadRequest;
                allCustomerDDL.StatusMessage = "Bad Request";
                allCustomerDDL.data = null;
                _logger.LogError(ex.ToString());

            }
            return allCustomerDDL;

        }

        [HttpGet("UserRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<userrolesresponse>> UserroleDDL()
        {
            userrolesresponse userrolesDDL = new userrolesresponse();
            try
            {
                var res = await _mediator.Send(new userroleDDLQuery());
                if (res != null)
                {
                    userrolesDDL.StatusCode = (int)HttpStatusCode.OK;
                    userrolesDDL.StatusMessage = "Record Found";
                    userrolesDDL.data = res;
                }
                else
                {
                    userrolesDDL.StatusCode = (int)HttpStatusCode.OK;
                    userrolesDDL.StatusMessage = "Record not Found";
                    userrolesDDL.data = null;
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                userrolesDDL.StatusCode = (int)HttpStatusCode.BadRequest;
                userrolesDDL.StatusMessage = "Bad Request";
                userrolesDDL.data = null;
                _logger.LogError(ex.ToString());

            }
            return userrolesDDL;

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public async Task<string> GetAdminAccessToken()
        {
            var tenantId = this._baseconfiguration["AzureAd:TenantId"];
            var clientId = this._baseconfiguration["AzureAd:clientId"];
            var clientSecret = this._baseconfiguration["AzureAd:clientSecret"];
            using (var client = new HttpClient())
            {
                var tokenEndpoint = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
                var formData = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "scope", "https://graph.microsoft.com/.default" }
                };
                var content = new FormUrlEncodedContent(formData);
                var response = await client.PostAsync(tokenEndpoint, content);
                var responseString = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return string.Empty;
                }
                var json = JObject.Parse(responseString);
                return json["access_token"].ToString();
            }
        }

        [HttpPut("UpdateUserProfilePicture")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<UserProfileResponse> UpdateUserProfilePicture([FromForm] UpdatedUserProfileImage model)
        {
            UpdateUserProfileImage updateUserProfileImage = new UpdateUserProfileImage();
            UserProfileResponse resp = new UserProfileResponse();
            _tokenBase.acces_token = HttpContext == null ? _tokenBase.acces_token : await HttpContext.GetTokenAsync("access_token");
            try
            {
                string ImagePath = "";
                if (model.ProfilePicture != null)
                {
                    string connectionString = this._baseconfiguration["LOG:CONNECTIONSTRING"];
                    string containerName = this._baseconfiguration["AzureAd:ImageContainerName"];
                    BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

                    // Get a reference to a blob
                    ImagePath = _tokenBase.getobjectid() + System.IO.Path.GetExtension(model.ProfilePicture.FileName);
                    BlobClient blob = container.GetBlobClient(ImagePath);

                    // Open the file and upload its data
                    using (Stream file = model.ProfilePicture.OpenReadStream())
                    {
                        blob.Upload(file, true);
                    }
                    updateUserProfileImage.ImagePath = ImagePath;
                    var result = await _mediator.Send((updateUserProfileImage));
                    if (result != null)
                    {
                        resp.StatusCode = (int)HttpStatusCode.OK;
                        resp.StatusMessage = "Image updated successfully";
                    }
                    else
                    {
                        resp.StatusCode = (int)HttpStatusCode.OK;
                        resp.StatusMessage = "Record not updated";
                    }
                }
                else
                {
                    resp.StatusCode = 404;
                    resp.StatusMessage = "Image not Update ";
                }

            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                resp.StatusCode = 404;
                resp.StatusMessage = "Image not Update ";
            }
            return (resp);
        }

        [HttpGet("GetUserProfileImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ExpandoObject>> GetUserProfileImage()
        {
            dynamic expandoObject = new ExpandoObject();
            try
            {
                _tokenBase.acces_token = HttpContext == null ? _tokenBase.acces_token : await HttpContext.GetTokenAsync("access_token");
                GetUserProfileResponseDT getUserResponse = new GetUserProfileResponseDT();
                GetUserProfileResponseDT res = await _mediator.Send(new GetByUserProfileIdQuery());
                string connectionString = this._baseconfiguration["LOG:CONNECTIONSTRING"];
                string containerName = this._baseconfiguration["AzureAd:ImageContainerName"];
                BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
                BlobClient blobClient = container.GetBlobClient(res.data.ImagePath);
                BlobDownloadInfo blobdata = await blobClient.DownloadAsync();
                MemoryStream memoryStream = new MemoryStream();
                await blobdata.Content.CopyToAsync(memoryStream);
                expandoObject.statusMessage = "Record Found";
                expandoObject.statusCode = 200;
                expandoObject.data = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                expandoObject.statusMessage = "Image not found.";
                expandoObject.statusCode = 404;
            }
            return expandoObject;
        }

        [HttpPut("UpdateUserProfile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserProfileResponse>> UpdateUserProfile([FromBody] UpdateUserProfileCommand command)
        {
            UserProfileResponse resp = new UserProfileResponse();
            try
            {
                if (command.Id != 0)
                {
                    var result = await _mediator.Send(command);
                    if (result != null)
                    {
                        resp.StatusCode = (int)HttpStatusCode.OK;
                        resp.StatusMessage = "Record updated successfully";
                    }
                    else
                    {
                        resp.StatusCode = (int)HttpStatusCode.OK;
                        resp.StatusMessage = "Record not updated";
                    }
                }
                else
                {
                    resp.StatusCode = (int)HttpStatusCode.OK;
                    resp.StatusMessage = "Please enter valid UserID";
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = "Exception",
                    StatusCode = 404,
                    Content = "User not Update "
                };
            }
        }

        [HttpGet("GetUserProfileById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<GetUserProfileResponseDT> GetUserProfileById()
        {
            _tokenBase.acces_token = HttpContext == null ? _tokenBase.acces_token : await HttpContext.GetTokenAsync("access_token");
            GetUserProfileResponseDT getUserResponse = new GetUserProfileResponseDT();
            try
            {
                GetUserProfileResponseDT res = await _mediator.Send(new GetByUserProfileIdQuery());
                return res;
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                JSONString = "{\n  \"data\" : " + null + ",  \"StatusMessage\" : " + ex.Message.ToString() + ",\n  \"StatusCode\" : " + (int)HttpStatusCode.NotFound + " \n}";
                _logger.LogError(ex.ToString());
            }
            return getUserResponse;
        }

    }
}
