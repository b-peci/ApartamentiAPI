using System.Net;
using System.Net.Mail;
using MediatR;

namespace Application.Features.Emails;

public class Email
{
    public static string SentEmail(string emailTo, string emailFrom, string password, Guid userId)
    {
        string fromEmail = emailFrom;
        string fromPassword = password;

        using MailMessage message = new MailMessage();
        message.From = new MailAddress(fromEmail);
        message.Subject = "Verify your account";
        message.To.Add(emailTo);
        message.Body = "<!DOCTYPE html><html lang='en'><head> <meta charset='UTF-8'> <meta http-equiv='X-UA-Compatible' " +
                       "content='IE=edge'> <meta name='viewport' content='width=device-width, initial-scale=1.0'> " +
                       "<title>Apartamenti</title></head><body>" +
                       " <div > <h1>Apartamenti</h1> <p>Hello home-seeker, thank you for choosing our platform to find your home</p> " +
                       "<p>In order to use our platform please take a second to confirm your email</p> <button style='border-color:#7367f0 " +
                       "!important; background-color: #7367f0 !important; box-shadow: none; color: white; width: 10rem; font-size: 1em; " +
                       "padding: 5px; border-radius: 5px; cursor: pointer;'>" +
                       $"<a href='http://localhost:3000/email-verified/{userId}' style='color:white; text-decoration: none;'>Verify account</a></button> " +
                       "<p style='font-style: italic;'>Welcome to Apartamenti</p> <p style='font-color: lightgray;'>  In search of Home</p> </div></body></html>";
        message.IsBodyHtml = true;
        var smtpClint = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(fromEmail, fromPassword),
            EnableSsl = true,
            Timeout = 20000
        };

        try
        {
            smtpClint.Send(message);
            return "Email sent successfully";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Could not sent email, please try again later";
        }


    }
}