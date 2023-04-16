namespace TestTask.Data.Services;

public interface IPriceService
{
        int GetPrice(int PizzaSizeId, List<int> IngredientsId);

}
