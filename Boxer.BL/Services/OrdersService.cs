using Boxer.BL.Interfaces;
using Boxer.DL;
using Boxer.DL.DTOProperties;
using Microsoft.EntityFrameworkCore;

namespace Boxer.BL.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly BoxerDBContext _context;

        public OrdersService(BoxerDBContext context)
        {
            _context = context;
        }

        public async Task<List<Orders>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task<Orders> GetOrderById(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(c => c.OrderID == id);
        }

        public async Task AddOrder(Orders orders)
        {
            // Fetch the next order number
            // MaxAsync() method to retrieve the maximum order number from the database.
            // (int?)a.OrderNumber extracts the OrderNumber property and converts it to a nullable integer (int?) to handle cases where there are no records yet.
            // The ?? 999 expression ensures that if there are no existing order numbers, the default value of 1000 will be used as the starting point for the next order number.
            int nextOrderNumber = await _context.Orders.MaxAsync(a => (int?)a.OrderNumber) ?? 9999;
            nextOrderNumber++;

            // Assign the generated number to the model
            orders.OrderNumber = nextOrderNumber;

            _context.Orders.Add(orders);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateOrder(Orders orders)
        {
            _context.Orders.Update(orders);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteOrder(int id)
        {
            var orders = await _context.Orders.FirstOrDefaultAsync(c => c.OrderID == id);
            if (orders == null)
            {
                return false;
            }

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
