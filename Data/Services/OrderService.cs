using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


using TestTask.Models;

namespace TestTask.Data.Services;

public class OrdersService : IOrdersService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public OrdersService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task SaveOrderToDb(OrderModel orderInfo)
    {
        await _context.Orders.AddAsync(orderInfo);
        await _context.SaveChangesAsync();
    }

    public async Task SendInfoToEmail(OrderModel orderInfo)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Sender Name", _configuration.GetSection("Email")["SendFrom"]));
        message.To.Add(new MailboxAddress("Recipient Name", _configuration.GetSection("Email")["SendTo"]));
        message.Subject = _configuration.GetSection("Email")["SubjectName"];

        message.Body = new TextPart("plain")
        {
            Text = $"Hello! Client {orderInfo.PhoneNum} ordered pizza with ingredients {orderInfo.IngredientsText}"
        };

        using (var client = new SmtpClient())
        {
            // Not configured
            return;
            await client.ConnectAsync("smtp.example.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                _configuration.GetSection("Email")["User"], 
                _configuration.GetSection("Email")["Password"]
                );
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
