using AssetsService.UnitTests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using UsersService.Application.Commands.Customer;
using UsersService.Application.Commands.Users;
using UsersService.Application.Queries;
using UsersService.Application.Responses.Customer;
using UsersService.Core.PagingHelper;
using UsersService.Core.Response;
using UsersService.Infrastructure.Helpers;
using UsersService.Responses.Users;
using VerifyAssetWorksAzureAD.Model;

namespace UsersService.Api.Controllers.Tests
{
    [TestClass()]
    public class CustomerControllerTests
    {

        private readonly CustomerController _customerController;
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<ILogger<CustomerController>> _logger;
        private readonly IConfiguration _baseconfiguration;
        private readonly TokenBase _tokenBase;
        private readonly Mock<IMediator> _mediator;

        public CustomerControllerTests()
        {
            _mediator = new Mock<IMediator>();

            _configuration = new Mock<IConfiguration>();
            _logger = new Mock<ILogger<CustomerController>>();
            _customerController = new CustomerController(_mediator.Object, _logger.Object, _tokenBase,_baseconfiguration);
        }
        #region Create Customer Test cases
        [TestMethod()]
        public void CreateCustomerTest()
        {
            CreateCustomersCommand createCustomersCommand = new CreateCustomersCommand()
            {

                userName = "alex",
                email = "alex11@gmail.com",
                notes = "notes123",
                pointofcontact = "contact1",
                PhoneNumber = "98765456",
                AddressLine1 = "california",
                AddressLine2 = "united-state",
                CountryID = 1,
                StateID = 1,
               // CityID = 1,
                ZipCode = "234123",
                CreatedBy = "user123"
            };
            CustomerResponse customerResponse = new CustomerResponse()
            {
                StatusCode = 200,
                StatusMessage = "Ok",
                Id = 0

            };
            _mediator.Setup(md => md.Send(It.IsAny<CreateCustomersCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(customerResponse);
            var actionResult = _customerController.CreateCustomer(createCustomersCommand).Result;
            Assert.AreEqual(200, (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as CustomerResponse;
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod()]
        public void BadRequestCreateCustomer()
        {
            CreateCustomersCommand createCustomersCommand = new CreateCustomersCommand();

            //createCustomersCommand.userName = "user";
            createCustomersCommand.email = "user@gmail.com";
            createCustomersCommand.notes = "notes123";
            createCustomersCommand.pointofcontact = "contact1";
            createCustomersCommand.PhoneNumber = "98";
            createCustomersCommand.AddressLine1 = "california";
            createCustomersCommand.AddressLine2 = "united-state";
            createCustomersCommand.CountryID = 1;
            createCustomersCommand.StateID = 1;
            //createCustomersCommand.CityID = 1;
            createCustomersCommand.ZipCode = "234123";
            createCustomersCommand.CreatedBy = "user123";

            //Act
            var result = _customerController.CreateCustomer(createCustomersCommand).Result;
            var alertResponse = (result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value;

            //Assert
            Assert.AreEqual("400", Convert.ToString((alertResponse.GetType().GetProperty("StatusCode").GetValue(alertResponse,null))));
            Assert.IsNotNull(result.Result);
        }

        [TestMethod()]
        public void BlankRequestCreateCustomer()
        {

            CreateCustomersCommand createCustomersCommand = new CreateCustomersCommand();

            createCustomersCommand.userName = "";
            createCustomersCommand.email = "";
            createCustomersCommand.notes = "";
            createCustomersCommand.pointofcontact = "";
            createCustomersCommand.PhoneNumber = "";
            createCustomersCommand.AddressLine1 = "";
            createCustomersCommand.AddressLine2 = "";
            createCustomersCommand.CountryID = 1;
            createCustomersCommand.StateID = 1;
            //createCustomersCommand.CityID = 1;
            createCustomersCommand.ZipCode = "";
            createCustomersCommand.CreatedBy = "user123";

            //Act
            var result = _customerController.CreateCustomer(createCustomersCommand).Result;
            var alertResponse = (result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value;

            //Assert
            Assert.AreEqual("400", Convert.ToString((alertResponse.GetType().GetProperty("StatusCode").GetValue(alertResponse, null))));
            Assert.IsNotNull(result.Result);


        }
        [TestMethod()]
        public void ExceptionCreateCustomer()
        {
            CustomerResponse CustomerResponse = new CustomerResponse();

            CreateCustomersCommand createCustomersCommand = new CreateCustomersCommand();
            createCustomersCommand.userName = "";
            createCustomersCommand.email = "       ";
            createCustomersCommand.notes = "";
            createCustomersCommand.pointofcontact = "";
            createCustomersCommand.PhoneNumber = "";
            createCustomersCommand.AddressLine1 = "        ";
            createCustomersCommand.AddressLine2 = "";
            createCustomersCommand.CountryID = 1;
            createCustomersCommand.StateID = 1;
            //createCustomersCommand.CityID = 1;
            //createCustomersCommand.ZipCode = "";
            createCustomersCommand.CreatedBy = "user123";

            //Act
            var result = _customerController.CreateCustomer(createCustomersCommand).Result;
            var alertResponse = (result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value;

            //Assert
            Assert.AreEqual("404", Convert.ToString((alertResponse.GetType().GetProperty("StatusCode").GetValue(alertResponse, null))));
            Assert.IsNotNull(result.Result);


        }
        #endregion
        [TestMethod()]
        public void GetCustomerById()
        {
            long GetCustomerById = 12;

            AllCustomerResp customerID = new AllCustomerResp()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                data = new List<customerbyID>()
            {
                new customerbyID()
                {
                Id = 12,
                UserName = "Nik",
                email = "nik@gmail.com",
                PhoneNumber = "343434343",
                pointofcontact = "213",
                AddressLine1 = "delhi",
                AddressLine2 = "Delhi-4",
                CountryID = 1,
                StateID = 3,
                //CityID = 10,
                countryName = "UnitedState",
                stateName = "Washington DC",
                cityName = "George",
                description = "Notes2",
                zipCode = "787878",
                isActive = true,
                }
            }

            };
            _mediator.Setup(md => md.Send(It.IsAny<GetByIdCustomersQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(customerID);
            var actionResult = _customerController.GetById(Convert.ToInt32(GetCustomerById)).Result;
            Assert.IsNotNull(actionResult.StatusCode);
            Assert.AreEqual(200, ((actionResult) as AllCustomerResp).StatusCode);
        }
        [TestMethod()]
        public void UpdateCustomerTest()
        {
            int updateCustomerId = 12;
            UpdateCustomerCommand updateCustomerCommand = new UpdateCustomerCommand()
            {
                id = 12,
                userName = "Nik-john",
                pointofcontact = "213",
                email = "nik@gmail.com",
                notes = "user2",
                PhoneNumber = "87654334",
                AddressLine1 = "goa",
                AddressLine2 = "dubai",
                CountryID = 1,
                StateID = 3,
                //CityID = 10,
                ZipCode = "4567",
                ModifiedBy = "user123"

            };
            CustomerResponse customerResponse = new CustomerResponse()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                Id = 12
            };
            _mediator.Setup(md => md.Send(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(customerResponse);
            var actionResult = _customerController.UpdateCustomer(updateCustomerCommand).Result;
            Assert.AreEqual(200, (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as CustomerResponse;
            Assert.IsNotNull(actionResult.Result);
        }

        [TestMethod()]
        public void GetAllCustomer()
        {
            GetAllCustomerRequest getAllCustomerRequest = new GetAllCustomerRequest()
            {

                opratorid = "1",
                SearchParam = "",
                PageSize = 10,
                PageNumber = 1,
            };

            AllCustomersResponse AllCustomersResponse = new AllCustomersResponse()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                Active = 3,
                InActive = 2,
                statusData = new List<StatusData>()
                {
                    new StatusData()
                    {
                        Key="1",
                        Value="1",
                        Color="red"
                    }
                },
                data = new PagedList<allcustomerbyID>()
                {
                    new allcustomerbyID()
                    {
                       Id = 12,
                       UserName = "Nik",
                       email = "nik@gmail.com",
                       PhoneNumber = "343434343",
                       pointofcontact = "213",
                       AddressLine1 = "delhi",
                       AddressLine2 = "Delhi-4",
                       CountryID = 1,
                       StateID = 3,
                       //CityID = 10,
                       countryName = "UnitedState",
                       stateName = "Washington DC",
                       cityName = "George",
                       description = "Notes2",
                       zipCode = "787878",
                       isActive = true,
                       modifiedOn=DateTime.Now,
                       noofevcharger=1,
                       assets=3,
                       users=2
                    }
                },
                paginationResponse = new PaginationResponse()
                {
                    TotalCount = 1,
                    PageSize=10,
                    CurrentPage=1,
                    HasNext=true,
                    HasPrevious=true,
                    TotalPages=1,
                }
            };
            _mediator.Setup(md => md.Send(It.IsAny<GetAllCustomerQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(AllCustomersResponse);
            var actionResult = _customerController.GetAllCustomer(getAllCustomerRequest).Result;
            Assert.AreEqual(200, (actionResult.Value.StatusCode));
            Assert.IsNotNull(actionResult.Value);

        }

        [TestMethod()]
        public void IsActiveCustomer()
        {
            DeleteCustomersCommand deleteCustomeCommand = new DeleteCustomersCommand()
            {
                Id = 76,
                IsActive = false,

            };
            CustomerResponse customerResponse = new CustomerResponse()
            {
                StatusCode = 200,
                StatusMessage = "Success",
                Id = 76,

            };
            _mediator.Setup(x => x.Send(It.IsAny<DeleteCustomersCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(customerResponse);
            var actionResult = _customerController.DeleteCustomer(deleteCustomeCommand);
            Assert.AreEqual(200, (actionResult.Result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).StatusCode);
            var alertResponse = (actionResult.Result.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as CustomerResponse;
            Assert.IsNotNull(actionResult.Result);


        }
    }

}