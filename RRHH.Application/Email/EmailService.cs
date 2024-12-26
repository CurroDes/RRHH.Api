using Microsoft.Extensions.Configuration;
using RRHH.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RRHH.Application.Interfaces;
using RRHH.Domain.Entities;

namespace RRHH.Application.Email;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsyncApprovedd(EmployeesDTO e)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");

        var client = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["FromEmail"]),
            Subject = "Solicitud Aceptada",
            Body = $"Estimado {e.FirstName} {e.LastName} su solicitud ha sido estudiada y en este caso aceptada. Gracias por sus servicios.",
            IsBodyHtml = true
        };

        mailMessage.To.Add(e.Email);
        try
        {
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Maneja el error de forma apropiada
            throw new InvalidOperationException("Error al enviar el correo electrónico", ex);
        }
    }

    public async Task SendEmailAsyncCancel(EmployeesDTO e)
    {
        var smtpSettings = _configuration.GetSection("SmtpSettings");

        var client = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["FromEmail"]),
            Subject = "Solicitud Cancelada",
            Body = $"Estimado {e.FirstName} {e.LastName} su solicitud ha sido estudiada y en este caso cancelada. Revise con su Manager las causas. Gracias por su tiempo",
            IsBodyHtml = true
        };

        mailMessage.To.Add(e.Email);
        try
        {
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Maneja el error de forma apropiada
            throw new InvalidOperationException("Error al enviar el correo electrónico", ex);
        }
    }
}
