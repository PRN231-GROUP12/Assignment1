using Microsoft.AspNetCore.Mvc;
using PRN231_Group12.Assignment1.API.DTO;
using PRN231_Group12.Assignment1.Repo;
using PRN231_Group12.Assignment1.Repo.Entity;
using PRN231_Group12.Assignment1.Repo.UnitOfWork;

namespace PRN231_Group12.Assignment1.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController :  ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(ILogger<ProductController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet("{id}", Name = "GetProductById")]
    public IActionResult GetProductById([FromRoute] int id)
    {
        try
        {
            var product = _unitOfWork.GetRepository<Product>().GetById(id, x => x.Category!);
            if (product == null)
            {
                return NotFound();
            }
            var productDetailDto = new ProductDetailDto()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Weight = product.Weight,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                CategoryName = product.Category!.CategoryName
            };
            return Ok(productDetailDto);
        }
        catch (Exception ex)
        { 
            _logger.LogError(ex, "Error occurred while fetching product by ID.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("{minPrice}/{maxPrice}/{pageNumber}/{pageSize}", Name = "GetProductsByPrice")]
    public IActionResult GetProductsByPrice([FromRoute] decimal minPrice, [FromRoute] decimal maxPrice, [FromRoute] int pageNumber, [FromRoute] int pageSize)
    {
        try
        {
            var products = _unitOfWork.GetRepository<Product>()
                .FindByCondition(x => x.UnitPrice >= minPrice && 
                x.UnitPrice <= maxPrice, pageNumber, pageSize).ToList();
            var productDtos = products.Select(product => new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Weight = product.Weight,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
            }).ToList();
        
            return Ok(productDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching products by price range.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("category/{categoryName}/{pageNumber}/{pageSize}", Name = "GetProductsByCategory")]
    public IActionResult GetProductsByCategory([FromRoute] string categoryName, [FromRoute] int pageNumber, [FromRoute] int pageSize)
    {
        try
        {
            var products = _unitOfWork.GetRepository<Product>()
                .FindByCondition(x => x.Category!.CategoryName == categoryName,
                    pageNumber, pageSize).ToList();
            var productDtos = products.Select(product => new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Weight = product.Weight,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
            }).ToList();
        
            return Ok(productDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching products by category.");
            return StatusCode(500, "Internal server error");
        }
    }
}
