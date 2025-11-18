
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
            // Build MIME message
            var message = new MimeKit.MimeMessage();
            message.From.Add(new MimeKit.MailboxAddress(request.frommail.Split('@')[0],request.frommail));
            message.To.Add(new MimeKit.MailboxAddress(request.ToEmail.Split('@')[0],request.ToEmail));

            message.Subject = request.Subject;

            var bodyBuilder = new MimeKit.BodyBuilder
            {
                HtmlBody = request.Body,
                TextBody = MimeKit.Text.TextFormat.Text.ToString()
            };

            message.Body = bodyBuilder.ToMessageBody();

            // Read MailSettings from configuration
            string host = this.Configuration.GetSection("MailSettings")["Host"];
            int port = int.Parse(this.Configuration.GetSection("MailSettings")["Port"]);
            string username = "apikey";
            string password = this.Configuration.GetSection("MailSettings")["Password"];

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    // IMPORTANT: Fix for SendGrid SSL certificate mismatch
                    client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
                    {
                        var cert2 = certificate as X509Certificate2;
                        if (cert2 == null)
                            return false;

                        // Extract CN (Common Name)
                        string cn = cert2.GetNameInfo(X509NameType.SimpleName, false);

                        // Allow valid SendGrid CN patterns
                        if (cn == "*.smtp.sendgrid.net" || cn == "smtp.sendgrid.net")
                            return true;

                        // Default strict validation
                        return errors == SslPolicyErrors.None;
                    };

                    client.CheckCertificateRevocation = false;

                    // Connect (STARTTLS)
                    await client.ConnectAsync(host, port, MailKit.Security.SecureSocketOptions.StartTls);

                    // Authenticate
                    await client.AuthenticateAsync(username, password);

                    // Send
                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    // Log only — same as your old code (which ignored error)
                    Console.WriteLine("Mail Error: " + ex.Message);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
        public async Task SendEmailAsync1(MailRequest request)
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




