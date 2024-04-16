using Microsoft.AspNetCore.Mvc;
using PRN231_Group12.Assignment1.API.DTO;
using PRN231_Group12.Assignment1.Repo.Entity;
using PRN231_Group12.Assignment1.Repo.UnitOfWork;

namespace PRN231_Group12.Assignment1.API.Controllers;
[ApiController]
[Route("[controller]")]
public class MembersController :  ControllerBase
{
    private readonly ILogger<MembersController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public MembersController(ILogger<MembersController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet("{id}", Name = "GetMemberById")]
    public IActionResult GetMemberById([FromRoute] int id)
    {
        try
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);
            if (member == null)
            {
                return NotFound();
            }
            var memberDto = new MemberDto()
            {
                Id = member.Id,
                Email = member.Email,
                Country = member.Country,
                City = member.City,
                CompanyName = member.CompanyName,
            };
            return Ok(memberDto);
        }
        catch (Exception ex)
        { 
            _logger.LogError(ex, "Error occurred while fetching member by ID.");
            return StatusCode(500, "Internal server error");
        }
    }
}