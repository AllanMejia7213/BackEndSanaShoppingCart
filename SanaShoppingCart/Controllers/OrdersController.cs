using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanaShoppingCart.Models;
using System.Collections.Immutable;

namespace SanaShoppingCart.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public readonly SanaShoppingCartContext _dbContext;

        public OrdersController(SanaShoppingCartContext _context)
        {
            _dbContext = _context;
        }

        [HttpGet]
        [Route("OrderList")]
        public IActionResult GetOrderList()
        {
            try
            {
                var orderList = _dbContext.Orders.Select(o => new {
                    OrderId = o.OrderId,
                    Customer = o.Customer,
                    OrderDetail = o.OrderDetails.Select(d => new
                    {
                        DetailId = d.OrderDetailId,
                        Product = d.Product!.ProductName,
                        Price = d.Price,
                        Quantity = d.Quantity,
                        TotalPrice = d.TotalPrice
                    }),
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = orderList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }

            
        }

        [HttpGet]
        [Route("GetOrderById/(OrderId: int)")]
        public IActionResult GetOrderById(int OrderId)
        {
            List<Order> OrdertList = new List<Order>();
            try
            {
                OrdertList = _dbContext.Orders.Where(p => p.OrderId == OrderId).ToList();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = OrdertList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("SaveOrder")]
        public IActionResult SaveOrder([FromBody] Order _order)
        {
            try
            {
                _dbContext.Orders.Add(_order);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditOrder")]
        public IActionResult EditOrder([FromBody] Order _order)
        {
            Order? oOrder = _dbContext.Orders.Find(_order.OrderId);
            if (oOrder == null)
            {
                return BadRequest("Order not found");
            }
            try
            {
                oOrder.OrderDetails = _order.OrderDetails is null ? oOrder.OrderDetails : _order.OrderDetails;
                oOrder.OrderDate = _order.OrderDate is null ? oOrder.OrderDate : _order.OrderDate;
                oOrder.TotalAmount = _order.TotalAmount is null ? oOrder.TotalAmount : _order.TotalAmount;
                oOrder.Customer = oOrder.Customer is null ? oOrder.Customer : _order.Customer;
                _dbContext.Orders.Update(oOrder);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = oOrder });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteOrder/{idOrder:int}")]
        public IActionResult DeleteOrder(int idOrder)
        {
            Order? oOrder = _dbContext.Orders.Find(idOrder);
            if (oOrder == null)
            {
                return BadRequest("Order not found");
            }
            try
            {
                _dbContext.Orders.Remove(oOrder);
                _dbContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

    }
}
