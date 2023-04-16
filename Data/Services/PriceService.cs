using TestTask.Data.Static;

namespace TestTask.Data.Services;
public class PriceService : IPriceService
{

    public int GetPrice(int PizzaSizeId, List<int> IngredientsId)
    {
        int sum = 0;
        if(PizzaForms.PizzaFormIdToPrice.ContainsKey(PizzaSizeId))
            sum = PizzaForms.PizzaFormIdToPrice[PizzaSizeId];
        else
            sum = PizzaForms.PizzaFormIdToPrice.First().Value;

        foreach (int id in IngredientsId)
        {
            sum += Ingredients.PizzaIngredientsIdToPrice[id];
        }
        return sum;
    }
}
