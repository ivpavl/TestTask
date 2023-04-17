using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.Data.Static;
using TestTask.Data.Services;
using TestTask.Data.Extensions;

namespace TestTask.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IOrdersService _orderService;
    // private readonly ISession _session;

    public HomeController(
        ILogger<HomeController> logger, 
        IOrdersService orderService
        // IHttpContextAccessor httpContextAccessor
        )
    {
        _orderService = orderService;
        _logger = logger;
        // _session = httpContextAccessor.HttpContext.Session;

    }

    public IActionResult Index()
    {

        CreateOrderModel model = new CreateOrderModel();
        model.Ingredients = Ingredients.IngredientList;
        model.PizzaForm = PizzaForms.PizzaFormList;

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [HttpPost]
    public IActionResult Index(CreateOrderModel orderInfo)
    {
        if (!ModelState.IsValid)
        {
            orderInfo.Ingredients = Ingredients.IngredientList;
            orderInfo.PizzaForm = PizzaForms.PizzaFormList;
            return View("Index", orderInfo);
        }

        OrderModel order = _orderService.SetupOrder(orderInfo);

        HttpContext.Session.SetObject<OrderModel>(order.PhoneNum, order);

        return View("OrderConfirmation", order);
    }

    [HttpPost]
    public async Task<IActionResult> OrderConfirmation(OrderModel model)
    {
        model = HttpContext.Session.GetObject<OrderModel>(model.PhoneNum);
        if(model is null)
            return View("Error");
            
        await _orderService.SendInfoToEmail(model);
        await _orderService.SaveOrderToDb(model);

        return View("Thanks");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
