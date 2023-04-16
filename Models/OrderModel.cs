using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TestTask.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }
        public string ClientName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PhoneNum { get; set; } = null!;


        public int Price { get; set; }
        [NotMapped]
        public List<int> IngredientsId { get; set; } = null!;
        [NotMapped]
        public int PizzaFormId { get; set; }
        public string IngredientsText {get; set;} = null!;
        public string PizzaFormText {get; set;} = null!;
    }
}