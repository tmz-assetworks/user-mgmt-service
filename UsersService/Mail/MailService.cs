
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Net.Mail;
using UsersService.Api.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace UsersService.Api.Mail
{
    public class MailService
    {
        private IConfiguration Configuration;
        public MailService(IConfiguration iConfig)
        {
            Configuration = iConfig;
        }
        public async Task SendEmailAsync(MailRequest request)
        {
           
            MailMessage msg = new MailMessage();
            //msg.To.Add(new MailAddress("anil.shukla@assetworks.com", "anil"));
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
                //Console.WriteLine("Message Sent Succesfully");
            }
            catch (Exception ex)
            {
               // Console.WriteLine(ex.ToString());
            }

        }

        public  async Task SendEmail(string nikname, string userprincipal, string emailId, long rOTP)
        {
            MailRequest request = new MailRequest();
            if (Configuration["flag:Emailflag"] == "0")
            {
                request.ToEmail = "ashu.setiya@assetworks.com";
                 //request.ToEmail = "tripathi7800@gmail.com";
                request.frommail = "mamta.mishra@assetworks.com";
            }
            else
            {
                request.ToEmail = emailId;
                request.frommail = "mamta.mishra@assetworks.com";
            }

            request.Subject = "Registration OTP";
            request.Body = "Dear " + nikname + ", <br><br> Your OTP Is:" + " " + rOTP + ". For verify otp please <a href=\""+Configuration["BaseUrl:fronendurl"] +"/verifyOTP?emailid=" + emailId + "\">click hear</a> <br><br> Regards <br> Assetwork Teams";

            UsersService.Api.Mail.MailService mailService = new UsersService.Api.Mail.MailService(Configuration);
            mailService.SendEmailAsync(request);
            Console.WriteLine("Mail Response :" + request);
        }
    }
}




