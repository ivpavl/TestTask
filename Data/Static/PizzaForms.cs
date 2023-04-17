using TestTask.Models;

namespace TestTask.Data.Static
{
    public static class PizzaForms
    {

        public static List<PizzaFormModel> PizzaFormList = new List<PizzaFormModel>()
        {
            new PizzaFormModel(){Id = 1, Name = "Small"},
            new PizzaFormModel(){Id = 2, Name = "Medium"},
            new PizzaFormModel(){Id = 3, Name = "Large"},
        };
        public static PizzaFormModel DefaultPizzaForm = new PizzaFormModel()
        {
            Id = 1, Name = "Small"
        };
        public static Dictionary<int, int> PizzaFormIdToPrice = new Dictionary<int, int>()
        {
            {1, 3},
            {2, 4},
            {3, 5},
        };
        
    }
}