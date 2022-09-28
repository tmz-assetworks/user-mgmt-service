using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using UsersService.Application.Commands.Customer;
using UsersService.Application.Queries;
using UsersService.Application.Responses.Customer;
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
        TokenBase _tokenBase;
        string JSONString = String.Empty;
        public CustomerController(IMediator mediator, ILogger<CustomerController> logger, TokenBase token)
        {
            _mediator = mediator;
            _logger = logger;
            _tokenBase = token;

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
                    allCustomerResp.StatusMessage = "Record Found";
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
                    allCustomerResp.StatusMessage = "Record not Found";
                    allCustomerResp.data = null;
                }
            }
            catch (Exception ex)
            {
                allCustomerResp.StatusCode = (int)HttpStatusCode.BadRequest;
                allCustomerResp.StatusMessage = "Bad Request";
                allCustomerResp.data = null;
                _logger.LogError(ex.ToString());

            }
            return allCustomerResp;

        }
        [HttpGet("GetCustomerbyID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<AllCustomerResp> GetById(int id)
        {
            AllCustomerResp allCustomerResp = new AllCustomerResp();
            try
            {
                if (id == 0)
                {
                    _tokenBase.acces_token = await HttpContext.GetTokenAsync("access_token");

                    AllCustomerResp res = await _mediator.Send(new GetByIdCustomersQuery(long.Parse(_tokenBase.getcustomerId())));
                    //return getjson(res);
                    return res;
                }
                else
                {
                    AllCustomerResp res = await _mediator.Send(new GetByIdCustomersQuery(id));
                    //return getjson(res);
                    return res;
                }
            }
            catch (Exception ex)
            {
                JSONString = "{\n  \"data\" : " + null + ",  \"StatusMessage\" : " + ex.Message.ToString() + ",\n  \"StatusCode\" : " + (int)HttpStatusCode.NotFound + " \n}";
                _logger.LogError(ex.ToString());

            }
            return allCustomerResp;


        }
        [HttpPost("CreateCustomer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CreateCustomersCommand command)
        {
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
                        if (resp != null)
                        {
                            resp.StatusCode = (int)HttpStatusCode.OK;
                            resp.StatusMessage = "Record saved successfully";
                            resp.Id = result.Id;
                        }
                        else
                        {
                            resp.StatusCode = (int)HttpStatusCode.OK;
                            resp.StatusMessage = "Record not saved";
                            resp.Id = result.Id;
                        }
                    }
                    else
                    {
                        resp.StatusCode = 400;
                        resp.StatusMessage = "Customer not Created ";
                        resp.Id = 0;
                    }

                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = "Exception",
                    StatusCode = 404,
                    Content = "Customer not Created "
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
                    resp.StatusMessage = "Record updated successfully";
                    resp.Id = result.Id;
                }
                else
                {
                    resp.StatusCode = (int)HttpStatusCode.OK;
                    resp.StatusMessage = "Record not updated";
                    resp.Id = result.Id;
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = "Exception",
                    StatusCode = 404,
                    Content = "Customer not Updated "
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
                    resp.StatusMessage = "Record updated successfully";
                    resp.Id = result.Id;
                }
                else
                {
                    resp.StatusCode = (int)HttpStatusCode.OK;
                    resp.StatusMessage = "Record not updated";
                    resp.Id = result.Id;
                }
                return Ok(resp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return new ContentResult()
                {
                    ContentType = "Exception",
                    StatusCode = 404,
                    Content = "Customer not Deleted "
                };
            }
        }
    }
}
