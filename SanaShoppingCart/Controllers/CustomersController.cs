using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SanaShoppingCart.Models;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Cors;

namespace SanaShoppingCart.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public readonly SanaShoppingCartContext _dbContext;

        public CustomersController(SanaShoppingCartContext _context)
        {
            _dbContext = _context;
        }

        [HttpGet]
        [Route("CustomerList")]
        public IActionResult GetCustomerList()
        {
            try
            {
                var CustomerList = _dbContext.Customers.Select(c => new {
                    CustomerId = c.CustomerId,
                    CustomerName = c.FirstName,
                    Description = c.LastName,
                    Price = c.Email,
                    Stock = c.Address,
                    Orders = c.Orders.Select(o => new {
                        OrderId = o.OrderId,
                        OrderDate = o.OrderDate,
                        TotalAmount = o.TotalAmount,
                        OrderDetails = o.OrderDetails
                    })
                });

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = CustomerList });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetCustomerByEmail/(email : string)")]
        public IActionResult GetCustomerByEmail(string email)
        {
            Customer customer = new Customer();
            try
            {
                customer = (Customer)_dbContext.Customers.Where(c => c.Email == email);
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = customer });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("SaveCustomer")]
        public IActionResult SaveCustomer([FromBody] Customer _customer)
        {
            try
            {
                _dbContext.Customers.Add(_customer);
                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpPut]
        [Route("EditCustomer")]
        public IActionResult EditCustomer([FromBody] Customer _customer)
        {
            Customer? oCustomer = _dbContext.Customers.Find(_customer.CustomerId);
            if (oCustomer == null)
            {
                return BadRequest("Customer not found");
            }
            try
            {
                oCustomer.FirstName = _customer.FirstName is null ? oCustomer.FirstName : _customer.FirstName;
                oCustomer.LastName = _customer.LastName is null ? oCustomer.LastName : _customer.LastName;
                oCustomer.Email = _customer.Email is null ? oCustomer.Email : _customer.Email;
                oCustomer.Address = _customer.Address is null ? oCustomer.Address : _customer.Address;
                _dbContext.Customers.Update(oCustomer);
                _dbContext.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = oCustomer });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status200OK, new { mensaje = ex.Message });
            }
        }

        [HttpDelete]
        [Route("DeleteCustomer/{customerId:int}")]
        public IActionResult DeleteCustomer(int customerId)
        {
            Customer? oCustomer = _dbContext.Customers.Find(customerId);
            if (oCustomer == null)
            {
                return BadRequest("Customer not found");
            }
            try
            {
                _dbContext.Customers.Remove(oCustomer);
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
