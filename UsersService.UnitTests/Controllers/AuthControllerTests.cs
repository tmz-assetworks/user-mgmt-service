using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsersService.Api.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using VerifyAssetWorksAzureAD.Model;
using UsersService.Api.DataModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using AssetsService.UnitTests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace UsersService.Api.Controllers.Tests
{
    [TestClass()]
    public class AuthControllerTests
    {
        private readonly AuthController authController;
        //private readonly Mock<IConfiguration> _configuration;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;
        //IDictionary<string, string> myConfiguration = new Dictionary<string, string>();

        public AuthControllerTests()
        {

            authController = new AuthController(_configuration, _mediator);
        }


        [TestMethod()]
        public void BadRequest()
        {

            //passing password without Encrypt
            AuthModel auth = new AuthModel();
            auth.username = "test1";
            auth.password = "testa";

            //Act

            var result = authController.Post(auth).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));

            string StatusCode = jObj["StatusCode"].ToString();
            string Message = jObj["Message"].ToString();

            //Assert  
            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "400");


        }
        [TestMethod()]
        public void BlankRequest()
        {

            //passing password and username blank
            AuthModel auth = new AuthModel();
            auth.username = "";
            auth.password = "";

            //Act

            var result = authController.Post(auth).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));


            string StatusCode = jObj["StatusCode"].ToString();
            string Message = jObj["Message"].ToString();
            //Assert  
            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "400");


        }
        [TestMethod()]
        public void EncryptPasswordRequest()
        {

            //passing password and username blank
            AuthModel auth = new AuthModel();
            auth.username = "test1";
            auth.password = "DY+4dU3HQ2JjnH57BAUFbD7lzfLolaz8hjj/EacFc8rS3+s1cvknTN5Pi85X+P2g";

            //Act
            var result = authController.Post(auth).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));

            string error = jObj["error"].ToString();
            string error_description = jObj["error_description"].ToString();
            string error_codes = jObj["error_codes"].ToString();
            //Assert  
            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "400");


        }
        [TestMethod()]
        public void ValidGetToken()
        {

            AuthModel auth = new AuthModel();
            auth.username = "admin@devopstekmindz.onmicrosoft.com";
            //auth.password = EncryptDecrypt.EncryptString("vPm@2022", myConfiguration["EncryptDecryptkey"]);
            auth.password = "DY+4dU3HQ2JjnH57BAUFbD7lzfLolaz8hjj/EacFc8rS3+s1cvknTN5Pi85X+P2g";

            //Act

            var result = authController.Post(auth).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));

            string access_token = jObj["access_token"].ToString();
            string id_token = jObj["id_token"].ToString();
            //Assert  
            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "200");


        }
        [TestMethod()]
        public void RoleExsistOrNot()
        {

            //checking role exists or not 
            AuthModel auth = new AuthModel();
            auth.username = "admin@devopstekmindz.onmicrosoft.com";
            //auth.password = EncryptDecrypt.EncryptString("vPm@2022", myConfiguration["EncryptDecryptkey"]);
            auth.password = "DY+4dU3HQ2JjnH57BAUFbD7lzfLolaz8hjj/EacFc8rS3+s1cvknTN5Pi85X+P2g";

            //Act

            var result = authController.Post(auth).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));


            string access_token = jObj["access_token"].ToString();
            string id_token = jObj["id_token"].ToString();

            var stream = id_token;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "roles").Value;


            //Assert  
            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "200");


        }
        [TestMethod()]
        public void InvalidTokenVerify()
        {

            //checking role exists or not 
            AuthModel auth = new AuthModel();
            auth.username = "admin@devopstekmindz.onmicrosoft.com";
            //auth.password = EncryptDecrypt.EncryptString("vPm@2022", myConfiguration["EncryptDecryptkey"]);
            auth.password = "DY+4dU3HQ2JjnH57BAUFbD7lzfLolaz8hjj/EacFc8rS3+s1cvknTN5Pi85X+P2g";
            //Act

            Microsoft.AspNetCore.Mvc.IActionResult result = authController.Post(auth).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));

            string access_token = jObj["access_token"].ToString();
            string id_token = jObj["id_token"].ToString();

            var stream = id_token + "ss";
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "roles").Value;

            //Assert  
            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "200");


        }
        //===============Test case for Logout API

        [TestMethod()]
        public void BlankRequest_logout()
        {

            //passing emailid as blank
            string emailid = "";
            //Act
            var result = authController.Logout(emailid).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));


            string StatusCode = jObj["StatusCode"].ToString();
            string Message = jObj["Message"].ToString();
            //Assert  
            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "404");


        }
        [TestMethod()]
        public void VerifyInValidEmail_logout()
        {

            //check invalid email 
            string emailid = "admin@devopstekmindzz.onmicrosoft.com";
            //Act
            var result = authController.VerifyUser(emailid).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));
            string StatusCode = result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString();

            //Assert  
            Assert.AreEqual(StatusCode, "404");


        }
        [TestMethod()]
        public void CheckInvalidEmail_logout()
        {

            //passing emailid as blank
            string emailid = "abhishek@tekmindz.com";
            //Act
            var result = authController.VerifyUser(emailid).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));
            string StatusCode = result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString();

            //Assert  
            Assert.AreEqual(StatusCode, "404");


        }
        [TestMethod()]
        public void VerifyValidEmail_logout()
        {

            //check valid email 
            string emailid = "admin@devopstekmindz.onmicrosoft.com";
            //Act
            var result = authController.VerifyUser(emailid).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));
            string StatusCode = result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString();

            //Assert  
            Assert.AreEqual(StatusCode, "200");


        }

        [TestMethod()]
        public void logoutSuccess_logout()
        {

            //succes logout 
            string emailid = "admin@devopstekmindz.onmicrosoft.com";
            //Act
            var result = authController.Logout(emailid).Result;

            JObject jObj = JObject.Parse(TestModel.GetValue(result));
            string StatusCode = result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString();

            //Assert  
            Assert.AreEqual(StatusCode, "200");


        }

        [TestMethod()]
        public void EncryptPasswordRequestChangePassword()
        {

            //passing password withoutout  Encrypt for Change Password

            string emailid = "admin@devopstekmindz.onmicrosoft.com";
            string password = "DY+4dU3HQ2JjnH57BAUFbD7lzfLolaz8hjj/EacFc8rS3+s1cvknTN5Pi85X+P2g";


            //Act

            //var result = authController.ChangePassword(emailid, password).Result;
            // JObject jObj = JObject.Parse(TestModel.GetValue(result));

            //string StatusCode = result.GetType().GetProperty("StatusCode").GetValue(result,null).ToString();
            ///   string Message = jObj["Message"].ToString();

            //Assert  
            //Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "200");



        }
        [TestMethod()]
        public void BlankRequestChangePassword()
        {

            //passing emailid and password blank for ChangePassword

            string emailid = "";
            string password = "";

            //Act

            //var result = authController.ChangePassword(emailid, password).Result;



            //Assert  

           // Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "400");


        }
        [TestMethod()]
        public void BlankPassword()
        {

            //passing emailid and password blank for Change Password

            string emailid = "admin@devopstekmindz.onmicrosoft.com";
            string password = "";

            //Act

            //var result = authController.ChangePassword(emailid, password).Result;



            //Assert  

           // Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "400");


        }
        [TestMethod()]
        public void BlankRequest_OTP_Emailid()
        {

            //passing blank emailid and otp 

            string emailid = "";
            string OTP = "";

            //Act

            var result = authController.VerifyUserByOTP(emailid, OTP).Result;



            //Assert  

            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "400");


        }
        [TestMethod()]
        public void BlankRequest_EmailId()
        {

            //passing blank emailid 

            string emailid = "";
            string OTP = "123456";

            //Act

            var result = authController.VerifyUserByOTP(emailid, OTP).Result;



            //Assert  

            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "400");


        }
        [TestMethod()]
        public void BlankRequest_OTP()
        {

            //passing Blank otp 

            string emailid = "admin@devopstekmindz.onmicrosoft.com";
            string OTP = "";

            //Act

            var result = authController.VerifyUserByOTP(emailid, OTP).Result;



            //Assert  

            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "400");


        }

        [TestMethod()]
        public void Validate_Wrong_Otp()
        {

            //passing Blank otp 

            string emailid = "admin@devopstekmindz.onmicrosoft.com";
            string OTP = "1112345";

            //Act

            var result = authController.VerifyUserByOTP(emailid, OTP).Result;



            //Assert  

            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "404");


        }
        [TestMethod()]
        public void Validate_Correct_Otp()
        {

            //passing Blank otp 

            string emailid = "admin@devopstekmindz.onmicrosoft.com";
            string OTP = "12345";

            //Act

            var result = authController.VerifyUserByOTP(emailid, OTP).Result;



            //Assert  

            Assert.AreEqual(result.GetType().GetProperty("StatusCode").GetValue(result, null).ToString(), "200");


        }


    }
}