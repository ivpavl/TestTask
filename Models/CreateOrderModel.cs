using System.ComponentModel.DataAnnotations;

namespace TestTask.Models;

public class CreateOrderModel
{    
    [Required(ErrorMessage = "Your name is required!")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Your address is required!")]
    public string Address { get; set; } = null!;
    
    [Required(ErrorMessage = "Your phone is required!")]
    public string Phone { get; set; } = null!;

    public List<IngredientModel> Ingredients { get; set; } = null!;
    public List<PizzaFormModel> PizzaForm { get; set; } = null!;
    public int SelectedPizzaFormId { get; set; }

}
