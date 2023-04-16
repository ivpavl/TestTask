using TestTask.Models;

namespace TestTask.Data.Services;
public interface IOrdersService
{
        Task SaveOrderToDb(OrderModel orderInfo);
        Task SendInfoToEmail(OrderModel orderInfo);

}
