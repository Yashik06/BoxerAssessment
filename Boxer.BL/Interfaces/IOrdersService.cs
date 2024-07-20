using Boxer.DL.DTOProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxer.BL.Interfaces
{
    public interface IOrdersService
    {
        Task<List<Orders>> GetAllOrders();
        Task<Orders> GetOrderById(int id);
        Task AddOrder(Orders orders);
        Task UpdateOrder(Orders orders);
        Task<bool> DeleteOrder(int id);
    }
}
