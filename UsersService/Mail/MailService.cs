
using System.Net.Mail;
using UsersService.Api.Model;
using UsersService.Core.Entities;

namespace UsersService.Api.Mail
{
    public class MailService
    {
        private IConfiguration Configuration;
        protected readonly UsersService.Infrastructure.DBContext.DBContextCore _dbContext;
        public MailService(IConfiguration iConfig, UsersService.Infrastructure.DBContext.DBContextCore dbContext)
        {
            Configuration = iConfig;
            _dbContext = dbContext;
        }
        public async Task SendEmailAsync(MailRequest request)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(request.ToEmail, (request.ToEmail.Split("@")[0])));
            msg.From = new MailAddress(request.frommail, (request.frommail.Split("@")[0]));
            msg.Subject = request.Subject;
            msg.Body = request.Body;
            msg.IsBodyHtml = true;
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(this.Configuration.GetSection("MailSettings")["UserName"], this.Configuration.GetSection("MailSettings")["Password"]);
            client.Port = Convert.ToInt32(this.Configuration.GetSection("MailSettings")["Port"]); // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = this.Configuration.GetSection("MailSettings")["Host"];
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
            }
            catch (Exception ex)
            {

            }
        }

        public MailResponse SendEmail(string nikname, string userprincipal, string emailId, long rOTP)
        {
            MailRequest request = new MailRequest();
            MailResponse response = new MailResponse();
            if (Configuration["flag:Emailflag"] == "0")
            {
                request.ToEmail = Configuration["MailSettings:UserName"];
                request.frommail = Configuration["MailSettings:UserName"];
            }
            else
            {
                request.ToEmail = emailId;
                request.frommail = Configuration["MailSettings:UserName"];
            }
            EmailTemplate objuser = _dbContext.EmailTemplate.Where(e => e.Type == "USER_REGISTRATION").FirstOrDefault();
            string srrlink = "<a href ='" + Configuration["BaseUrl:fronendurl"] + "/verifyOTP?emailid=" + emailId + "'>click here</a>";
            string actual = objuser.Body.Replace("$link", srrlink).Replace("$nikname", nikname).Replace("$rOTP", rOTP.ToString());
            request.Subject = objuser.Subjects;
            request.Body = actual;
            UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(Configuration,_dbContext);
            SendEmailAsync(request);
            Console.WriteLine("Mail Response :" + request);
            response.Subject = request.Subject;
            response.Body = request.Body;
            return response;
        }
        public MailResponse SendEmailCustomer(string customer, string emailId)
        {
            MailRequest request = new MailRequest();
            MailResponse response = new MailResponse();
            if (Configuration["flag:Emailflag"] == "0")
            {
                request.ToEmail = Configuration["MailSettings:UserName"];
                request.frommail = Configuration["MailSettings:UserName"];
            }
            else
            {
                request.ToEmail = emailId;
                request.frommail = Configuration["MailSettings:UserName"];
            }
            EmailTemplate objcustomer = _dbContext.EmailTemplate.Where(e =>e.Type=="CUSTOMER_REGISTRATION").FirstOrDefault();
            string actual = objcustomer.Body.Replace("$User", customer);
            request.Subject = objcustomer.Subjects;
            request.Body= actual;
            UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(Configuration, _dbContext);
            mailService.SendEmailAsync(request);
            Console.WriteLine("Mail Response :" + request);
            response.Subject = request.Subject;
            response.Body = request.Body;
            return response;
        }

        public MailResponse SendEmailUserVerify(string emailId, string rOTP,string name)
        {
            MailRequest request = new MailRequest();
            MailResponse response = new MailResponse();
            if (Configuration["flag:Emailflag"] == "0")
            {
                request.ToEmail = Configuration["MailSettings:UserName"];
                request.frommail = Configuration["MailSettings:UserName"];
            }
            else
            {
                request.ToEmail = emailId;
                request.frommail = Configuration["MailSettings:UserName"];
            }
            //EmailTemplate objcustomer = _dbContext.EmailTemplate.Where(e => e.Type == "FORGATE_PASSWORD").FirstOrDefault();
            EmailTemplate objcustomer = _dbContext.EmailTemplate.Where(e => e.Type == "FORGOT_PASSWORD").FirstOrDefault();
            string actual = objcustomer.Body.Replace("$User", name).Replace("$rotp",rOTP);
            request.Subject = objcustomer.Subjects;
            request.Body = actual;
            UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(Configuration, _dbContext);
            mailService.SendEmailAsync(request);
            Console.WriteLine("Mail Response :" + request);
            response.Subject = request.Subject;
            response.Body = request.Body;
            return response;
        }
    }
}




