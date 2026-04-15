namespace DanceSchoolApp.Server.Services;

public class EmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    /// <summary>
    /// Envia as credenciais geradas para o email do novo utilizador.
    /// </summary>
    public async Task SendWelcomeEmailAsync(string toEmail, string role, string temporaryPassword)
    {
        var subject = "Bem-vindo à Dance School App — As suas credenciais";

        var body = $"""
            <html>
            <body style="font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: auto;">
              <h2 style="color: #7b2d8b;">Bem-vindo à App da Ent´artes!</h2>
              <p>A sua conta foi criada com o perfil  <strong>{role}</strong>.</p>
              <p>Utilize as seguintes credenciais para entrar na aplicação:</p>
              <table style="background:#f4f4f4; padding:16px; border-radius:8px; width:100%;">
                <tr><td><strong>Email:</strong></td><td>{toEmail}</td></tr>
                <tr><td><strong>Password:</strong></td><td style="font-size:1.2em; letter-spacing:2px;">{temporaryPassword}</td></tr>
              </table>
              <p style="margin-top:16px;">
                Após o primeiro login, poderá alterar a sua password e preencher os seus dados pessoais.
              </p>
              <p style="color:#888; font-size:0.85em;">
                Se não estava à espera deste email, pode ignorá-lo.
              </p>
            </body>
            </html>
            """;

        await SendEmailAsync(toEmail, subject, body);
    }

    /// <summary>
    /// Método base para envio de email via SMTP.
    /// Requer configuração na secção "Email" do appsettings.json.
    /// </summary>
    private async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var host = _config["Email:Host"] ?? throw new InvalidOperationException("Email:Host not configured.");
        var port = int.Parse(_config["Email:Port"] ?? "587");
        var user = _config["Email:Username"] ?? throw new InvalidOperationException("Email:Username not configured.");
        var pass = Environment.GetEnvironmentVariable("DanceSchoolApp_Email_Password")
                   ?? throw new InvalidOperationException("DanceSchoolApp_Email_Password env var not set.");
        var from = _config["Email:From"] ?? user;

        // Usamos System.Net.Mail (nativo) para não adicionar dependência extra.
        // Se preferires MailKit, substitui este bloco pelo equivalente.
        using var client = new System.Net.Mail.SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new System.Net.NetworkCredential(user, pass)
        };

        var message = new System.Net.Mail.MailMessage(from, to, subject, htmlBody)
        {
            IsBodyHtml = true
        };

        try
        {
            await client.SendMailAsync(message);
            _logger.LogInformation("Email enviado para {Email}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao enviar email para {Email}", to);
            throw;
        }
    }
}