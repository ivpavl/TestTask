using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TestTask.Models;
using TestTask.Data.Static;


namespace TestTask.Data.Services;

public class OrdersService : IOrdersService
{
    
    private readonly IPriceService _priceService;
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public OrdersService(
        AppDbContext context, 
        IPriceService priceService, 
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
        _priceService = priceService;
    }
    public async Task SaveOrderToDb(OrderModel orderInfo)
    {
        await _context.Orders.AddAsync(orderInfo);
        await _context.SaveChangesAsync();
    }
    public OrderModel SetupOrder(CreateOrderModel createOrderInfo)
    {
        OrderModel order = new OrderModel();

                
        List<int> IngredientsId = createOrderInfo.Ingredients
                    .Where(ingredient => ingredient.IsSelected)
                    .Select(ingredient => ingredient.Id)
                    .ToList();
        
        int price = _priceService.GetPrice(createOrderInfo.SelectedPizzaFormId, IngredientsId);

        order.Price = price;
        order.IngredientsId = IngredientsId;
        order.PizzaFormId = createOrderInfo.SelectedPizzaFormId;
        order.PhoneNum = createOrderInfo.Phone;
        order.ClientName = createOrderInfo.Name;
        order.Address = createOrderInfo.Address;
        order.IngredientsText = "";

        foreach (int ingredientId in IngredientsId)
        {
            IngredientModel ingredient = Ingredients.IngredientList.FirstOrDefault(i => i.Id == ingredientId)!;
            if (ingredient is not null)
                order.IngredientsText += ingredient.Name + ", ";
        }

        PizzaFormModel pizzaForm = PizzaForms.PizzaFormList.FirstOrDefault(x => x.Id == order.PizzaFormId)!;
        if (pizzaForm is null)
        {
            order.PizzaFormText = PizzaForms.DefaultPizzaForm.Name;
            order.PizzaFormId = PizzaForms.DefaultPizzaForm.Id;
        }
        return order;
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
