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

    public async Task SendEmailAsyncApproved(Employee e)
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
            Subject = "RRHH || Solicitud Aceptada",
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

    public async Task<Result> SendEmailAsyncCancel(Employee e)
    {
        Result result = new Result();

        result.IsSuccess = true;
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
            Subject = "RRHH || Solicitud Cancelada",
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
            result.IsSuccess = false;
            return result;
            throw new InvalidOperationException("Error al enviar el correo electrónico", ex);
        }

        return result;
    }

    public async Task SendEmailRequest(Employee e)
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
            Subject = "RRHH || Solicitud Recibida",
            Body = $"Estimado {e.FirstName} {e.LastName} su solicitud ha sido recibida, recibirá la respuesta de la misma, una vez que su manager la haya revisado",
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

    public async Task<Result> SendEmailToMultipleAsync(List<string> emails, TextDTO t)
    {
        Result result = new Result();

        result.IsSuccess = true;

        var smtpSettings = _configuration.GetSection("SmtpSettings");

        var client = new SmtpClient(smtpSettings["Host"])
        {
            Port = int.Parse(smtpSettings["Port"]),
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
            EnableSsl = true
        };

        foreach (var email in emails)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpSettings["FromEmail"]),
                Subject = "RRHH || Correo Masivo",
                Body = t.ToString(),
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                // Manejar errores de forma apropiada (registrar logs, etc.)
                throw new InvalidOperationException($"Error al enviar correo a {email}", ex);
            }
        }

        return result;
    }

    public async Task SendEmailReviewsApproved(Employee e, PerformanceReviewsDTO p)
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
            Subject = "RRHH || Solicitud Aceptada",
            Body = $@"
            <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            color: #333;
                            font-size: 14px;
                        }}
                        .header {{
                            color: #4CAF50;
                            font-size: 18px;
                            font-weight: bold;
                        }}
                        .content {{
                            margin-top: 20px;
                            font-size: 16px;
                        }}
                        .footer {{
                            margin-top: 30px;
                            font-size: 12px;
                            color: #777;
                        }}
                    </style>
                </head>
                <body>
                    <p class='header'>Estimado {e.FirstName} {e.LastName},</p>
                    <p>Le informamos que su rendimiento durante el año ha sido valorado. A continuación, podrá ver los comentarios de su manager:</p>
                    <div class='content'>
                        <p><strong>Valoración del Manager:</strong></p>
                        <p>{p.Comments}</p>
                    </div><
                    <div class='footer'>
                        <p>Atentamente,</p>
                        <p>El equipo de RRHH</p>
                    </div>
                </body>
            </html>
        ",
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
