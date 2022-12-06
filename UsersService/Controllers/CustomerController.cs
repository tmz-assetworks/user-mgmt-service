using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Text;
using System.Text.Json;
using UsersService.Api.Model;
using UsersService.Application.Commands.Customer;
using UsersService.Application.Queries;
using UsersService.Application.Responses.Customer;
using UsersService.Core.ConstantResponse;
using UsersService.Core.Entities;
using UsersService.Core.Response;
using UsersService.Infrastructure.EnumData;
using UsersService.Infrastructure.Helpers;

namespace UsersService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CustomerController> _logger;
        private readonly IConfiguration _baseconfiguration;
        TokenBase _tokenBase;
        string JSONString = String.Empty;
        public CustomerController(IMediator mediator, ILogger<CustomerController> logger, TokenBase token, IConfiguration baseconfiguration)
        {
            _mediator = mediator;
            _logger = logger;
            _tokenBase = token;
            _baseconfiguration = baseconfiguration;
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
                    {"flag:Emailflag", Environment.GetEnvironmentVariable("flag_Emailflag")},
                    {"MailSettings:UserName", Environment.GetEnvironmentVariable("Mail_UserName")},
                    {"MailSettings:Password", Environment.GetEnvironmentVariable("Mail_Password")},
                    {"MailSettings:Host", Environment.GetEnvironmentVariable("Mail_Host")},
                    {"MailSettings:Port", Environment.GetEnvironmentVariable("Mail_Port")},
                    {"AzureAd:resourceId", Environment.GetEnvironmentVariable("AZUREAD_RESOURCEID")},
                    {"AzureAd:operatorRoleId", Environment.GetEnvironmentVariable("AZUREAD_OPERATORRID")},
                    {"AzureAd:adminRoleId", Environment.GetEnvironmentVariable("AZUREAD_ADMINRID")},
                    {"BaseUrl:fronendurl", Environment.GetEnvironmentVariable("BaseUrl_fronted")},
                    {"AzureAd:helpdeskUserName", Environment.GetEnvironmentVariable("helpdeskUserName")},
                    {"AzureAd:helpdeskPassword", Environment.GetEnvironmentVariable("helpdeskPassword")},
                };
             _baseconfiguration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
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
                JSONString = "{\n  \"StatusCode\" : " + (int)HttpStatusCode.OK + ",\n  \"StatusMessage\" : \"Record not found\",\n  \"data\" : " + null + " \n}";
            }
            return JSONString;
        }

        [HttpPost("GetAllCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AllCustomersResponse>> GetAllCustomer(GetAllCustomerRequest customerRequest)
        {
            AllCustomersResponse allCustomerResp = new AllCustomersResponse();
            try
            {
                if (customerRequest.PageSize == 0) customerRequest.PageSize = 10;
                if (customerRequest.PageNumber == 0) customerRequest.PageNumber = 1;
                var res = await _mediator.Send(new GetAllCustomerQuery(customerRequest));
                allCustomerResp.data = res.data;
                allCustomerResp.statusData = new List<StatusData>();
                if (res != null && res.data.Count > 0)
                {
                    List<StatusData> statusData = new List<StatusData>()
                    {
                       new StatusData { Key = Status_Indication.CustomerActiveInActive.TotalCustomer.GetEnumDisplayName(), Value = res!=null? (res.Active + res.InActive).ToString():"" , Color = CustomerStatusColor.TotalCustomer.GetEnumDisplayName() },
                       new StatusData { Key = Status_Indication.CustomerActiveInActive.Active.GetEnumDisplayName(), Value = res!=null? res.Active.ToString():"" , Color = CustomerStatusColor.Active.GetEnumDisplayName() },
                        new StatusData { Key = Status_Indication.CustomerActiveInActive.InActive.GetEnumDisplayName(), Value = res!=null? res.InActive.ToString():"" , Color = CustomerStatusColor.InActive.GetEnumDisplayName() },
                        };
                    allCustomerResp.statusData = statusData;
                    allCustomerResp.StatusCode = (int)HttpStatusCode.OK;
                    allCustomerResp.StatusMessage = RespnoseMessage.Record_found;
                    allCustomerResp.Active = res.Active;
                    allCustomerResp.InActive = res.InActive;
                    allCustomerResp.paginationResponse = new Core.PagingHelper.PaginationResponse
                    {
                        TotalCount = allCustomerResp.data.TotalCount,
                        PageSize = allCustomerResp.data.PageSize,
                        CurrentPage = allCustomerResp.data.CurrentPage,
                        TotalPages = allCustomerResp.data.TotalPages,
                        HasNext = allCustomerResp.data.HasNext,
                        HasPrevious = allCustomerResp.data.HasPrevious
                    };
                }
                else
                {
                    allCustomerResp.StatusCode = (int)HttpStatusCode.OK;
                    allCustomerResp.StatusMessage = RespnoseMessage.Record_not_found;
                    allCustomerResp.data = null;
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                allCustomerResp.StatusCode = (int)HttpStatusCode.BadRequest;
                allCustomerResp.StatusMessage = RespnoseMessage.Bad_Request;
                allCustomerResp.data = null;
                _logger.LogError(ex.ToString());

            }
            return allCustomerResp;

        }
        [HttpGet("GetCustomerbyID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<AllCustomerResp> GetById(int? id)
        {
            AllCustomerResp allCustomerResp = new AllCustomerResp();
            try
            {
                if (id == null || id==0)
                {
                    _tokenBase.acces_token = await HttpContext.GetTokenAsync("access_token");
                    AllCustomerResp res = await _mediator.Send(new GetByIdCustomersQuery(""));
                    return res;
                }
                else
                {
                    AllCustomerResp res = await _mediator.Send(new GetByIdCustomersQuery(Convert.ToString(id.Value)));
                    return res;
                }
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                JSONString = "{\n  \"data\" : " + null + ",  \"StatusMessage\" : " + ex.Message.ToString() + ",\n  \"StatusCode\" : " + (int)HttpStatusCode.NotFound + " \n}";
                _logger.LogError(ex.ToString());

            }
            return allCustomerResp;


        }
        [HttpPost("CreateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CreateCustomersCommand command)
        {
            _tokenBase.acces_token = await HttpContext.GetTokenAsync("access_token");
            MailResponse mailresponse = new MailResponse();
            string callingMethod = APIConstant.InsTaskNotification;
            TaskNotificationRequest taskNotificationRequest = new TaskNotificationRequest();
            CustomerResponse resp = new CustomerResponse();
            try
            {
                if (!string.IsNullOrEmpty(command.userName) && !string.IsNullOrEmpty(command.AddressLine1) && !string.IsNullOrEmpty(command.ZipCode))
                {
                    if (command.userName.Contains(' ') && command.AddressLine1.Contains(' '))
                    {
                        command.userName = command.userName.ToString().TrimStart();
                        command.AddressLine1 = command.AddressLine1.ToString().TrimStart();
                    }
                    else
                    {
                        command.userName = command.userName;
                        command.AddressLine1 = command.AddressLine1;
                    }

                    if (!string.IsNullOrEmpty(command.userName))
                    {
                        var result = await _mediator.Send(command);
                        if (result != null)
                        {
                            resp.StatusCode = (int)HttpStatusCode.OK;
                            resp.StatusMessage = RespnoseMessage.Record_Save_Successfully;
                            resp.Id = result.Id;
                            UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(_baseconfiguration);
                            //Console.WriteLine("Send Mail :" + (nikname, userprincipal, command.EmailId, long.Parse(resultRes.OTP)));
                            mailresponse=mailService.SendEmailCustomer(command.userName,command.email);
                            //------insert notification-------------
                            taskNotificationRequest.category = "Email";
                            taskNotificationRequest.messagetype = mailresponse.Subject;
                            taskNotificationRequest.content = mailresponse.Body;
                            taskNotificationRequest.ipaddress = "192.186.178.07";
                            taskNotificationRequest.userId = _tokenBase.getobjectid();
                            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(taskNotificationRequest), Encoding.UTF8, "application/json");
                            HttpResponseMessage response = await Helper.GetCallAssetWithBodyAuthAPIAsync(callingMethod, httpContent, _tokenBase.acces_token);
                        }
                        else
                        {
                            resp.StatusCode = (int)HttpStatusCode.OK;
                            resp.StatusMessage = RespnoseMessage.Record_Not_Saved;
                            resp.Id = 0;
                        }
                    }
                    else
                    {
                        resp.StatusCode = 400;
                        resp.StatusMessage = RespnoseMessage.Customer_not_Created;
                        resp.Id = 0;
                    }

                }
                else
                {
                    resp.StatusCode = 400;
                    resp.StatusMessage = RespnoseMessage.Customer_not_Created;
                    resp.Id = 0;
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = RespnoseMessage.Exception,
                    StatusCode = 404,
                    Content = RespnoseMessage.Customer_not_Created
                };
            }
        }

        [HttpPut("UpdateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerResponse>> UpdateCustomer([FromBody] UpdateCustomerCommand command)
        {
            CustomerResponse resp = new CustomerResponse();
            try
            {
                var result = await _mediator.Send(command);
                if (result != null)
                {
                    resp.StatusCode = (int)HttpStatusCode.OK;
                    resp.StatusMessage = RespnoseMessage.Record_Updated_Successfully;
                    resp.Id = result.Id;
                }
                else
                {
                    resp.StatusCode = (int)HttpStatusCode.OK;
                    resp.StatusMessage = RespnoseMessage.Record_Not_Updated;
                    resp.Id = result.Id;
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = RespnoseMessage.Exception,
                    StatusCode = 404,
                    Content = RespnoseMessage.Customer_not_Updated
                };
            }
        }
        [HttpDelete("DeleteCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerResponse>> DeleteCustomer([FromBody] DeleteCustomersCommand command)
        {
            CustomerResponse resp = new CustomerResponse();
            try
            {
                var result = await _mediator.Send(command);
                if (result != null)
                {
                    resp.StatusCode = (int)HttpStatusCode.OK;
                    resp.StatusMessage = RespnoseMessage.Record_Updated_Successfully;
                    resp.Id = result.Id;
                }
                else
                {
                    resp.StatusCode = (int)HttpStatusCode.OK;
                    resp.StatusMessage = RespnoseMessage.Record_Not_Updated;
                    resp.Id = result.Id;
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                Log.Information("error occurred :" + ex.Message);
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = RespnoseMessage.Exception,
                    StatusCode = 404,
                    Content = RespnoseMessage.Customer_not_Deleted
                };
            }
        }
    }
}
