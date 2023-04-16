namespace TestTask.Models;

public class CreateOrderModel
{    

    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public List<IngredientModel> Ingredients { get; set; } = null!;

    public List<PizzaFormModel> PizzaForm { get; set; } = null!;
    public int SelectedPizzaFormId { get; set; }

}
