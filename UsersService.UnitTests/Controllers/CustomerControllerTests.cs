using AssetsService.UnitTests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        protected readonly UsersService.Infrastructure.DBContext.DBContextCore _dbContext;
        //private readonly TokenBase _tokenBase;
        private readonly Mock<IMediator> _mediator;

        public CustomerControllerTests()
        {
            _mediator = new Mock<IMediator>();

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
                    {"flag:Emailflag", Environment.GetEnvironmentVariable("FLAG_EMAILFLAG")},
                    {"MailSettings:UserName", Environment.GetEnvironmentVariable("MAIL_USERNAME")},
                    {"MailSettings:Password", Environment.GetEnvironmentVariable("MAIL_PASSWORD")},
                    {"MailSettings:Host", Environment.GetEnvironmentVariable("MAIL_HOST")},
                    {"MailSettings:Port", Environment.GetEnvironmentVariable("MAIL_PORT")},
                    {"BaseUrl:fronendurl", Environment.GetEnvironmentVariable("BASEURL_FRONTED")},
                };
            if (Environment.GetEnvironmentVariable("AZUREAD_CID") == null)
            {
                myConfiguration = new Dictionary<string, string>
                {
                    {"AzureAd:clientId", "7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed"},
                    {"AzureAd:TenantId", "744aa8b0-bb99-4982-903f-52328216b4be"},
                    {"AzureAd:clientSecret","xg.8Q~mpJZWo5su4_WIKRaVaydfHP99xFa15uak_"},
                    {"AzureAd:Instance","https://login.microsoftonline.com/"},
                    {"EncryptDecryptkey","E534C8DF286CD5931069B522E695D4F1" },
                    {"Jwt:Key","ThisismySecretKeyForAuthentication"},
                    {"Jwt:Issuer", "Asset_AuthService"},
                    {"Jwt:Audience", "Asset_DashboardService"},
                    {"flag:Emailflag", "0"},
                    {"MailSettings:UserName", "mamta.mishra@assetworks.com"},
                    {"MailSettings:Password", "Universe@123"},
                    {"MailSettings:Host", "smtp.office365.com"},
                    {"MailSettings:Port", "587"},
                    {"AzureAd:resourceId", "f0d224c5-2ea1-4823-9b93-45da968a05b5"},
                    {"AzureAd:operatorRoleId", "92c0466d-8be6-4466-ba15-1cc36d001aee"},
                    {"AzureAd:adminRoleId", "25ca4c0c-0d2e-4ebf-aa81-9397e027d15f"},
                    {"BaseUrl:fronendurl", "https://asw-portal-ui.azurewebsites.net"},
                    {"LOG:CONNECTIONSTRING","DefaultEndpointsProtocol=https;AccountName=assetswork;AccountKey=Rk7iyAEtGHdMWfojFlyE23dXYsMDUkH1zvLghSjWW9kZX7Ecv6wuJuvRifNQfOChKmY5d1Hvx7mE+AStxFztQw==;EndpointSuffix=core.windows.net"},
                    {"AzureAd:ImageContainerName", "userimages"},
                };
            }
            _configuration = new Mock<IConfiguration>();
            _logger = new Mock<ILogger<CustomerController>>();
            TokenBase token = new TokenBase();
            token.acces_token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSIsImtpZCI6IjJaUXBKM1VwYmpBWVhZR2FYRUpsOGxWMFRPSSJ9.eyJhdWQiOiJzcG46NzY5OGNiZWQtN2Q5Zi00M2IzLWI5Y2QtYTRmMDliOWI1NWVkIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvNzQ0YWE4YjAtYmI5OS00OTgyLTkwM2YtNTIzMjgyMTZiNGJlLyIsImlhdCI6MTY2OTg4Njk0NCwibmJmIjoxNjY5ODg2OTQ0LCJleHAiOjE2Njk4OTIyMzIsImFjciI6IjEiLCJhaW8iOiJBVFFBeS84VEFBQUExQTlEbHBoOVBJM3BEcUpWZjFOUktNRHpPV2RoU3piRkdUWDZINm0zR3hXVWVpNmZWSGdaMWRzZ3hlOUtPR3pCIiwiYW1yIjpbInB3ZCJdLCJhcHBpZCI6Ijc2OThjYmVkLTdkOWYtNDNiMy1iOWNkLWE0ZjA5YjliNTVlZCIsImFwcGlkYWNyIjoiMSIsImZhbWlseV9uYW1lIjoib3BlcmF0b3IiLCJnaXZlbl9uYW1lIjoib3BlcmF0b3IiLCJpcGFkZHIiOiI1Mi4xNDIuMTcyLjIyIiwibmFtZSI6Im9wZXJhdG9yIiwib2lkIjoiZjRlOWI0MTktYzdkYy00MmI2LTkyYmMtZjIwNzcwNzE2N2YyIiwicmgiOiIwLkFWVUFzS2hLZEptN2drbVFQMUl5Z2hhMHZ1M0xtSGFmZmJORHVjMms4SnViVmUySUFPdy4iLCJyb2xlcyI6WyJPcGVyYXRvciJdLCJzY3AiOiJBcHBSb2xlQXNzaWdubWVudC5SZWFkV3JpdGUuQWxsIERpcmVjdG9yeS5BY2Nlc3NBc1VzZXIuQWxsIERpcmVjdG9yeS5SZWFkLkFsbCBEaXJlY3RvcnkuUmVhZFdyaXRlLkFsbCBEaXJlY3RvcnkuV3JpdGUuUmVzdHJpY3RlZCBlbWFpbCBHcm91cC5SZWFkLkFsbCBHcm91cC5SZWFkV3JpdGUuQWxsIElkZW50aXR5VXNlckZsb3cuUmVhZFdyaXRlLkFsbCBvZmZsaW5lX2FjY2VzcyBvcGVuaWQgcHJvZmlsZSBUZWFtU2V0dGluZ3MuUmVhZFdyaXRlLkFsbCBVc2VyLkV4cG9ydC5BbGwgVXNlci5JbnZpdGUuQWxsIFVzZXIuTWFuYWdlSWRlbnRpdGllcy5BbGwgVXNlci5SZWFkIFVzZXIuUmVhZC5BbGwgVXNlci5SZWFkQmFzaWMuQWxsIFVzZXIuUmVhZFdyaXRlIFVzZXIuUmVhZFdyaXRlLkFsbCBVc2VyQXV0aGVudGljYXRpb25NZXRob2QuUmVhZCBVc2VyQXV0aGVudGljYXRpb25NZXRob2QuUmVhZC5BbGwgVXNlckF1dGhlbnRpY2F0aW9uTWV0aG9kLlJlYWRXcml0ZSBVc2VyQXV0aGVudGljYXRpb25NZXRob2QuUmVhZFdyaXRlLkFsbCIsInN1YiI6IksyYW43OF9kSEx1T2tjdWtMdEFjanAxZlg5Tk9ZZTZKei02SGRacWI4MVkiLCJ0aWQiOiI3NDRhYThiMC1iYjk5LTQ5ODItOTAzZi01MjMyODIxNmI0YmUiLCJ1bmlxdWVfbmFtZSI6Im9wZXJhdG9yQGRldm9wc3Rla21pbmR6Lm9ubWljcm9zb2Z0LmNvbSIsInVwbiI6Im9wZXJhdG9yQGRldm9wc3Rla21pbmR6Lm9ubWljcm9zb2Z0LmNvbSIsInV0aSI6IldnMzVaeTBSWVU2dnJMRmN2ajhOQWciLCJ2ZXIiOiIxLjAifQ.g3DPuouxmb2VODht1ylRGr7l7PuHDoyGejBEMceTmcJyM-jo_ZAiEnFLRrWEsCfSqTuDE8HZvG7auxd447uVEbhaQV_qsWp2MQbS2KTDMQMZw2PAWclcWHp-A51FKWbwcVUqlvevFRW9u-isA95C9zRuL6hbhxlonarn1v8BKa5CtNfXvkIqfJvrV5NwHT1z62fMWrL6CfsykB8lQUnwu1UvgKkw--qOxSfAgtRpPZ2CiozWpdoIaVkeTAT75eOY0D_jCaR3mfMDv0VUUpNBxRXCFYa7WIRua6IxhbP7cBjZ30Fl5FjMcMwAgAfNY-3tNWMfcUDrXHAmPcvED5D_HA";
            _customerController = new CustomerController(_mediator.Object, _logger.Object, token, _baseconfiguration, _dbContext);
        }
        #region Create Customer Test cases
        [TestMethod()]
        public void SuccessCreateCustomerTest()
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
        public void BadRequestCreateCustomerTest()
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
            var alertResponse = (result.Result as OkObjectResult).Value;

            //Assert
            Assert.AreEqual("400", Convert.ToString((alertResponse.GetType().GetProperty("StatusCode").GetValue(alertResponse,null))));
            Assert.IsNotNull(result.Result);
        }

        [TestMethod()]
        public void BlankRequestCreateCustomerTest()
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
            var alertResponse = (result.Result as OkObjectResult).Value;

            //Assert
            Assert.AreEqual(400, (alertResponse.GetType().GetProperty("StatusCode").GetValue(alertResponse, null)));
            Assert.IsNotNull(result.Result);


        }
        [TestMethod()]
        public void ExceptionCreateCustomerTest()
        {
            CreateCustomersCommand createCustomersCommand = new CreateCustomersCommand();

            //Act
            var result = _customerController.CreateCustomer(createCustomersCommand).Result;
            var alertResponse = (result.Result as OkObjectResult).Value;

            //Assert
            Assert.AreEqual(400, (alertResponse.GetType().GetProperty("StatusCode").GetValue(alertResponse, null)));
            Assert.IsNotNull(result.Result);
        }
        #endregion

        #region GetCustomerbyID 
        [TestMethod()]
        public void SuccessGetCustomerByIdTest()
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
        public void BadRequestGetCustomerByIdTest()
        {
            int GetCustomerById =Convert.ToInt32("00000000000000");

            AllCustomerResp customerID = new AllCustomerResp()
            {
                StatusCode = 400,
                StatusMessage = "Bad Request",
                data = new List<customerbyID>()
            {
                new customerbyID()
                {
                //Id = 12,
                //UserName = "Nik",
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
            var actionResult = _customerController.GetById(GetCustomerById).Result;
            Assert.IsNotNull(actionResult.StatusCode);
            Assert.AreEqual(400, ((actionResult) as AllCustomerResp).StatusCode);
        }
        [TestMethod()]
        public void ExceptionGetCustomerByIdTest()
        {
            int GetCustomerById = 0;

            AllCustomerResp customerID = new AllCustomerResp()
            {
                StatusCode = 400,
                StatusMessage = "Bad Request",
                data = new List<customerbyID>()
            {
                new customerbyID()
                {
                //Id = 12,
                //UserName = "Nik",
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
            var actionResult = _customerController.GetById(GetCustomerById).Result;
            Assert.IsNotNull(actionResult.StatusCode);
            Assert.AreEqual(400, ((actionResult) as AllCustomerResp).StatusCode);
        }
        #endregion

        #region Update Customer Test
        [TestMethod()]
        public void SuccessUpdateCustomerTest()
        {
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
            var alertResponse = (actionResult.Result as Microsoft.AspNetCore.Mvc.OkObjectResult).Value as CustomerResponse;
            Assert.AreEqual(200, alertResponse.StatusCode);
            Assert.IsNotNull(actionResult.Result);
        }
        [TestMethod()]
        public void BadRequestUpdateCustomerTest()
        {
            UpdateCustomerCommand updateCustomerCommand = new UpdateCustomerCommand()
            {
                id = 84,
                userName = "Nik-john",
                pointofcontact = "213",
                email = "nik@gmail.com",
                notes = "user2",
                PhoneNumber = "87654334",
                AddressLine1 = "goa",
                AddressLine2 = "dubai",
                CountryID = 1,
                StateID = 3,
                ZipCode = "4567",
                ModifiedBy = "user123"

            };
            _mediator.Setup(md => md.Send(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>()));
            var actionResult = _customerController.UpdateCustomer(updateCustomerCommand).Result;
            var alertResponse = (actionResult.Value.StatusCode);
            Assert.AreEqual(404, alertResponse);
            Assert.IsNotNull(actionResult.Value);
        }
        #endregion
        #region Get All Customer
        [TestMethod()]
        public void SuccessGetAllCustomer()
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
        public void BadRequestGetAllCustomer()
        {
            GetAllCustomerRequest getAllCustomerRequest = new GetAllCustomerRequest()
            {

                opratorid = "01.00",
                //SearchParam = "",
                //PageSize = 0,
                PageNumber = 0,
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
                    PageSize = 10,
                    CurrentPage = 1,
                    HasNext = true,
                    HasPrevious = true,
                    TotalPages = 1,
                }
            };
            _mediator.Setup(md => md.Send(It.IsAny<GetAllCustomerQuery>(), It.IsAny<CancellationToken>()));
            var actionResult = _customerController.GetAllCustomer(getAllCustomerRequest).Result;
            Assert.AreEqual(400, (actionResult.Value.StatusCode));
            Assert.IsNotNull(actionResult.Value);

        }
        #endregion
        #region IsActiveCustomer
        [TestMethod()]
        public void SuccessIsActiveCustomer()
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
        [TestMethod()]
        public void BadRequestIsActiveCustomer()
        {
            DeleteCustomersCommand deleteCustomeCommand = new DeleteCustomersCommand()
            {
                Id = 0,
                IsActive = false,

            };
            _mediator.Setup(x => x.Send(It.IsAny<DeleteCustomersCommand>(), It.IsAny<CancellationToken>()));
            var actionResult = _customerController.DeleteCustomer(deleteCustomeCommand);
            var alertResponse = (actionResult.Result.Value).StatusCode;
            Assert.AreEqual(404, alertResponse);
            Assert.IsNotNull(actionResult.Result);


        }
        #endregion
    }

}