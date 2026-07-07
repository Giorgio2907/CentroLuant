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
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: #1a73e8; padding: 20px; text-align: center;'>
                        <h2 style='color: white; margin: 0;'>Centro Odontológico Luant</h2>
                    </div>
                    <div style='padding: 30px; background: #f9f9f9;'>
                        <h3>Recordatorio de Cita</h3>
                        <p>Estimado/a <strong>{nombrePaciente}</strong>,</p>
                        <p>Le recordamos que tiene una cita programada:</p>
                        <div style='background: white; padding: 15px; border-left: 4px solid #1a73e8; margin: 20px 0;'>
                            <p><strong>Fecha:</strong> {fechaCita}</p>
                            <p><strong>Hora:</strong> {horaCita}</p>
                            <p><strong>Especialista:</strong> {especialista}</p>
                        </div>
                        <p>Por favor confirme su asistencia o contáctenos si necesita reprogramar.</p>
                        <p>Gracias por confiar en nosotros.</p>
                    </div>
                    <div style='background: #1a73e8; padding: 10px; text-align: center;'>
                        <p style='color: white; margin: 0; font-size: 12px;'>Centro Odontológico Luant</p>
                    </div>
                </div>"
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