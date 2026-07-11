using MailKit.Net.Smtp;
using MimeKit;

namespace CentroLuant.Services
{
    public class CorreoService
    {
        private readonly IConfiguration _config;

        public CorreoService(IConfiguration config)
        {
            _config = config;
        }

        public async Task EnviarRecordatorioCita(string correoDestino, string nombrePaciente,
            string fechaCita, string horaCita, string especialista)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Centro Odontológico Luant",
                _config["Correo:Email"]));
            mensaje.To.Add(new MailboxAddress(nombrePaciente, correoDestino));
            mensaje.Subject = "Recordatorio de Cita - Centro Odontológico Luant";

            var body = new BodyBuilder
            {
                HtmlBody = $@"
<!DOCTYPE html>
<html lang='es'>
<head>
<meta charset='utf-8' />
<meta name='viewport' content='width=device-width, initial-scale=1.0' />
</head>
<body style='margin:0; padding:0; background-color:#f0f4f8; font-family: Segoe UI, Arial, sans-serif;'>
<table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background-color:#f0f4f8; padding: 30px 15px;'>
<tr>
<td align='center'>
<table role='presentation' width='600' cellpadding='0' cellspacing='0' style='max-width:600px; width:100%; background:#ffffff; border-radius:16px; overflow:hidden; box-shadow:0 8px 25px rgba(21,101,192,0.15);'>

<!-- Header -->
<tr>
<td style='background:linear-gradient(135deg, #0a2d6e 0%, #1565c0 50%, #0097a7 100%); background-color:#1565c0; padding:32px 30px; text-align:center;'>
<div style='width:56px; height:56px; background:rgba(255,255,255,0.18); border-radius:14px; display:inline-block; line-height:56px; margin-bottom:14px;'>
<span style='font-size:26px;'>🦷</span>
</div>
<h1 style='color:#ffffff; font-size:20px; font-weight:800; margin:0;'>Centro Odontológico Luant</h1>
<p style='color:rgba(255,255,255,0.75); font-size:12.5px; margin:4px 0 0;'>Sistema de Gestión Clínica</p>
</td>
</tr>

<!-- Body -->
<tr>
<td style='padding:34px 36px;'>
<p style='font-size:11px; font-weight:700; color:#0097a7; letter-spacing:1px; text-transform:uppercase; margin:0 0 6px;'>Recordatorio de Cita</p>
<h2 style='font-size:20px; font-weight:800; color:#0a2d6e; margin:0 0 16px;'>Hola, {nombrePaciente} 👋</h2>
<p style='font-size:14.5px; color:#444; line-height:1.6; margin:0 0 22px;'>
Le recordamos que tiene una cita programada en nuestro centro odontológico. A continuación los detalles:
</p>

<!-- Info card -->
<table role='presentation' width='100%' cellpadding='0' cellspacing='0' style='background:#f8faff; border-left:4px solid #0097a7; border-radius:10px; margin-bottom:24px;'>
<tr>
<td style='padding:20px 22px;'>
<table role='presentation' width='100%' cellpadding='0' cellspacing='0'>
<tr>
<td style='padding-bottom:12px; vertical-align:top; width:34px;'><span style='font-size:17px;'>📅</span></td>
<td style='padding-bottom:12px;'>
<span style='display:block; font-size:11px; color:#888; text-transform:uppercase; letter-spacing:0.5px;'>Fecha</span>
<span style='display:block; font-size:15px; font-weight:700; color:#0a2d6e;'>{fechaCita}</span>
</td>
</tr>
<tr>
<td style='padding-bottom:12px; vertical-align:top;'><span style='font-size:17px;'>🕐</span></td>
<td style='padding-bottom:12px;'>
<span style='display:block; font-size:11px; color:#888; text-transform:uppercase; letter-spacing:0.5px;'>Hora</span>
<span style='display:block; font-size:15px; font-weight:700; color:#0a2d6e;'>{horaCita}</span>
</td>
</tr>
<tr>
<td style='vertical-align:top;'><span style='font-size:17px;'>👨‍⚕️</span></td>
<td>
<span style='display:block; font-size:11px; color:#888; text-transform:uppercase; letter-spacing:0.5px;'>Especialista</span>
<span style='display:block; font-size:15px; font-weight:700; color:#0a2d6e;'>{especialista}</span>
</td>
</tr>
</table>
</td>
</tr>
</table>

<p style='font-size:14px; color:#555; line-height:1.6; margin:0 0 6px;'>
Por favor, confirme su asistencia o contáctenos con anticipación si necesita reprogramar su cita.
</p>
<p style='font-size:14px; color:#555; line-height:1.6; margin:0;'>
Gracias por confiar en nosotros.
</p>
</td>
</tr>

<!-- Footer -->
<tr>
<td style='background:#f8faff; border-top:1px solid #eef2ff; padding:22px 36px; text-align:center;'>
<p style='margin:0 0 6px; font-size:12.5px; color:#0a2d6e; font-weight:700;'>Centro Odontológico Luant</p>
<p style='margin:0; font-size:11.5px; color:#999; line-height:1.6;'>
Av. Universitaria 1801, Lima, Perú<br />
+51 987 654 321 · centroluant2026@gmail.com
</p>
</td>
</tr>

</table>

<p style='font-size:11px; color:#aaa; margin-top:18px;'>© 2026 Centro Odontológico Luant · Diseñamos sonrisas, creamos confianza</p>
</td>
</tr>
</table>
</body>
</html>"
            };

            mensaje.Body = body.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["Correo:Host"],
                int.Parse(_config["Correo:Puerto"]!), false);
            await smtp.AuthenticateAsync(_config["Correo:Email"],
                _config["Correo:Password"]);
            await smtp.SendAsync(mensaje);
            await smtp.DisconnectAsync(true);
        }
    }
}