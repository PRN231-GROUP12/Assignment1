using Microsoft.AspNetCore.Mvc;
using PRN231_Group12.Assignment1.API.DTO;
using PRN231_Group12.Assignment1.API.Request;
using PRN231_Group12.Assignment1.Repo;
using PRN231_Group12.Assignment1.Repo.UnitOfWork;

namespace PRN231_Group12.Assignment1.API.Controllers;
[ApiController]
[Route("[controller]")]
public class OrdersController :  ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public OrdersController(ILogger<OrdersController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id}", Name = "GetOrderById")]
    public IActionResult GetOrderById([FromRoute] int id)
    {
        try
        {
            var order = _unitOfWork.GetRepository<Order>().GetById(id, x => x.OrderDetails!);
            if (order == null)
            {
                return NotFound();
            }

            var orderDto = new OrderDto()
            {
                Id = order.Id,
                MemberId = order.MemberId,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                Freight = order.Freight,
                OrderDetails = order.OrderDetails.Select(orderDetail => new OrderDetailDto()
                {
                    Id = orderDetail.Id,
                    ProductId = orderDetail.ProductId,
                    UnitPrice = orderDetail.UnitPrice,
                    Quantity = orderDetail.Quantity,
                    Discount = orderDetail.Discount
                }).ToList()
            };
            return Ok(orderDto);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching order by ID.");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("member/{id}", Name = "GetOrdersByMember")]
    public IActionResult GetOrderByMember([FromRoute] int id, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        try
        {
            var orders = _unitOfWork.GetRepository<Order>()
                .FindByCondition(x => x.MemberId == id, 
                    pageNumber, pageSize).ToList();
            var orderDtos = orders.Select(order => new OrdersDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                Freight = order.Freight
            }).ToList();
            return Ok(orderDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching orders by member ID.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpDelete("{id}", Name = "DeleteOrderById")]
    public IActionResult DeleteOrderById([FromRoute] int id)
    {
        try
        {
            var order = _unitOfWork.GetRepository<Order>().GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            _unitOfWork.GetRepository<Order>().Delete(order);
            _unitOfWork.Save();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting order by ID.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPut("{id}", Name = "UpdateOrderById")]
    public IActionResult UpdateOrderById([FromRoute] int id, [FromBody] UpdateOrderRequest request)
    {
        try
        {
            var order = _unitOfWork.GetRepository<Order>().GetById(id);
            if (order == null)
            {
                return NotFound();
            }
            order.MemberId = request.MemberId;
            order.OrderDate = request.OrderDate;
            order.RequiredDate = request.RequiredDate;
            order.ShippedDate = request.ShippedDate;
            order.Freight = request.Freight;
            _unitOfWork.GetRepository<Order>().Update(order);
            _unitOfWork.Save();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating order by ID.");
            return StatusCode(500, "Internal server error");
        }
    }
}