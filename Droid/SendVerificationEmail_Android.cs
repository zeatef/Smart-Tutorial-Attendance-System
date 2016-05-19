using System;
using Xamarin.Forms;
using GUC_Attendance.Droid;
using GUC_Attendance;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

[assembly: Dependency (typeof (SendVerificationEmail_Android))]

namespace GUC_Attendance.Droid
{
	public class SendVerificationEmail_Android : ISendVerificationEmail
	{
		public SendVerificationEmail_Android ()
		{
		}

		public void sendEmail (string fname, string lname, string recipient, int verificationcode)
		{
			var message = new MimeMessage ();
			message.From.Add (new MailboxAddress ("GUC Attendance", "gucattendance@gmail.com"));
			message.To.Add (new MailboxAddress (fname + " " + lname, recipient));
			message.Subject = "Verify Your Account";
			message.Body = new TextPart ("html") {
				Text = "<html>\n  <head>\n    <style type=\"text/css\">\n      .ExternalClass,.ExternalClass div,.ExternalClass font,.ExternalClass p,.ExternalClass span,.ExternalClass td,img{line-height:100%}#outlook a{padding:0}.ExternalClass,.ReadMsgBody{width:100%}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%}table,td{mso-table-lspace:0;mso-table-rspace:0}img{-ms-interpolation-mode:bicubic;border:0;height:auto;outline:0;text-decoration:none}table{border-collapse:collapse!important}#bodyCell,#bodyTable,body{height:100%!important;margin:0;padding:0;font-family:ProximaNova,sans-serif}#bodyCell{padding:20px}#bodyTable{width:600px}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-regular-webfont-webfont.woff) format('woff');font-weight:400;font-style:normal}@font-face{font-family:ProximaNova;src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot);src:url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.eot?#iefix) format('embedded-opentype'),url(https://cdn.auth0.com/fonts/proxima-nova/proximanova-semibold-webfont-webfont.woff) format('woff');font-weight:600;font-style:normal}@media only screen and (max-width:480px){#bodyTable,body{width:100%!important}a,blockquote,body,li,p,table,td{-webkit-text-size-adjust:none!important}body{min-width:100%!important}#bodyTable{max-width:600px!important}#signIn{max-width:280px!important}}\n    </style>\n  </head>\n  <body>\n    <center>\n      <table style=\"width: 600px;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 0;font-family: &quot;ProximaNova&quot;, sans-serif;border-collapse: collapse !important;height: 100% !important;\" align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"100%\" width=\"100%\" id=\"bodyTable\">\n        <tr>\n          <td align=\"center\" valign=\"top\" id=\"bodyCell\" style=\"-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;mso-table-lspace: 0pt;mso-table-rspace: 0pt;margin: 0;padding: 20px;font-family: &quot;ProximaNova&quot;, sans-serif;height: 100% !important;\">\n          <div class=\"main\">\n            <p style=\"text-align: center;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%; margin-bottom: 30px;\">\n              <img src=\"http://i.imgur.com/V5CwfNf.png\" width=\"200\" alt=\"Your logo goes here\" style=\"-ms-interpolation-mode: bicubic;border: 0;height: auto;line-height: 100%;outline: none;text-decoration: none;\">\n            </p>\n\n            <h1 style = color:black>Welcome to GUC Attendance!</h1>\n\n            <p style = color:black>Thank you for signing up. Your verification code is: <strong style = color:red>"+verificationcode+ "</strong></p>\n\n  \n            \n\n            \n           <p style = color:black> Thanks!</p>\n            \n\n            <strong style = color:black>GUC Attendance</strong>\n\n            <br><br>\n            <hr style=\"border: 2px solid #EAEEF3; border-bottom: 0; margin: 20px 0;\">\n            <p style=\"text-align: center;color: #A9B3BC;-webkit-text-size-adjust: 100%;-ms-text-size-adjust: 100%;\">\n              If you did not make this request, please ignore this message.\n            </p>\n          </div>\n          </td>\n        </tr>\n      </table>\n    </center>\n  </body>\n</html>\n"
			};

			using (var client = new SmtpClient ()) {
				client.Connect ("smtp.gmail.com", 587, false);

				// Note: since we don't have an OAuth2 token, disable
				// the XOAUTH2 authentication mechanism.
				client.AuthenticationMechanisms.Remove ("XOAUTH2");

				// Note: only needed if the SMTP server requires authentication
				client.Authenticate ("gucattendance", "AttendanceSystem2016");

				client.Send (message);
				client.Disconnect (true);
			}

		}
	}
}