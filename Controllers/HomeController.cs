using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.Data.Static;
using TestTask.Data.Services;


namespace TestTask.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPriceService _priceService;
    private readonly IOrdersService _orderService;

    public HomeController(ILogger<HomeController> logger, IPriceService priceService, IOrdersService orderService)
    {
        _logger = logger;
        _priceService = priceService;
        _orderService = orderService;
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
    public IActionResult Index(CreateOrderModel model)
    {
        
        var IngredientsId = new List<int>();
        foreach(IngredientModel ingredient in model.Ingredients)
            if(ingredient.IsSelected)
                IngredientsId.Add(ingredient.Id);

        int price = _priceService.GetPrice(model.SelectedPizzaFormId, IngredientsId);

        OrderModel confirmModel = new OrderModel();
        confirmModel.Price = price;
        confirmModel.IngredientsId = IngredientsId;
        confirmModel.PizzaFormId = model.SelectedPizzaFormId;
        confirmModel.PhoneNum = model.Phone;
        confirmModel.ClientName = model.Name;

        return View("OrderConfirmation", confirmModel);
    }

    [HttpPost]
    public async Task<IActionResult> OrderConfirmation(OrderModel model)
    {
        // Adding IngredientsText to model
        for (int i = 0; i < model.IngredientsId.Count(); i++)
        {
            foreach (IngredientModel ing in Ingredients.IngredientList)
            {
                if (ing.Id == model.IngredientsId[i])
                    model.IngredientsText += ing.Name + ", ";
            }
        }

        // Adding PizzaFormText to model
        foreach (PizzaFormModel form in PizzaForms.PizzaFormList)
        {
            if(form.Id == model.PizzaFormId)
            {
                model.PizzaFormText = form.Name;
                break;
            }
        }


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
