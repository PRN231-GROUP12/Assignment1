using Microsoft.AspNetCore.Mvc;
using PRN231_Group12.Assignment1.API.DTO;
using PRN231_Group12.Assignment1.API.Request;
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
    
    [HttpGet("search", Name = "SearchProducts")]
    public IActionResult SearchProducts([FromQuery] string keyword, [FromQuery] decimal minPrice, [FromQuery] decimal maxPrice, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        try
        {
            var products = new List<Product>();
            if(minPrice != 0 || maxPrice != 0 || !string.IsNullOrEmpty(keyword))
            {
                if (minPrice != 0 && maxPrice != 0 && minPrice < maxPrice 
                    && !string.IsNullOrEmpty(keyword))
                {
                    products = _unitOfWork.GetRepository<Product>()
                        .FindByCondition(product => product.UnitPrice >= minPrice 
                                                    && product.UnitPrice <= maxPrice,
                            pageNumber, pageSize).ToList();
                }
                else if (minPrice == 0 && maxPrice == 0)
                {
                    products = _unitOfWork.GetRepository<Product>()
                        .FindByCondition(product => product.ProductName.Contains(keyword),
                            pageNumber, pageSize).ToList();
                }
                else
                {
                    return BadRequest("Invalid price range or keyword.");
                }
            }
            
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
            _logger.LogError(ex, "Error occurred while fetching products by price range and keyword.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpGet("category/{categoryName}", Name = "GetProductsByCategory")]
    public IActionResult GetProductsByCategory([FromRoute] string categoryName, [FromQuery] int pageNumber, [FromQuery] int pageSize)
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
    
    [HttpPut("{id}", Name = "UpdateProduct")]
    
    public IActionResult UpdateProduct([FromRoute] int id, [FromBody] UpdateProductRequest request)
    {
        try
        {
            var existingProduct = _unitOfWork.GetRepository<Product>().GetById(id, x => x.Category!);
            if (existingProduct == null)
            {
                return NotFound("Product does not exist.");
            }

            var category = _unitOfWork.GetRepository<Category>().GetById(request.CategoryId);
            if (category is null)
            {
                return NotFound("Category does not exist.");
            }
            existingProduct.ProductName = request.ProductName;
            existingProduct.CategoryId = request.CategoryId;
            existingProduct.UnitPrice = request.UnitPrice;
            existingProduct.UnitsInStock = request.UnitsInStock;
            existingProduct.Weight = request.Weight;
            existingProduct.Category = category;
            _unitOfWork.GetRepository<Product>().Update(existingProduct);
            _unitOfWork.Save();
            return Ok(existingProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating product.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpDelete("{id}", Name = "DeleteProduct")]
    public IActionResult DeleteProduct([FromRoute] int id)
    {
        try
        {
            var existingProduct = _unitOfWork.GetRepository<Product>().GetById(id , x => x.Category!);
            if (existingProduct == null)
            {
                return NotFound();
            }

            _unitOfWork.GetRepository<Product>().Delete(existingProduct);
            _unitOfWork.Save();
            return Ok(existingProduct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting product.");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost(Name = "CreateProduct")]
    public IActionResult CreateProduct([FromBody] CreateProductRequest request)
    {
        try
        {
            var category = _unitOfWork.GetRepository<Category>().GetById(request.CategoryId);
            if (category is null)
            {
                return NotFound("Category does not exist.");
            }
            var product = new Product
            {
                ProductName = request.ProductName,
                CategoryId = request.CategoryId,
                UnitPrice = request.UnitPrice,
                UnitsInStock = request.UnitsInStock,
                Weight = request.Weight,
                Category = category
            };
            _unitOfWork.GetRepository<Product>().Insert(product);
            _unitOfWork.Save();
            return Ok(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating product.");
            return StatusCode(500, "Internal server error");
        }
    }
}
