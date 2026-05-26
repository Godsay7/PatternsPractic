using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IFinanceService _service;

    public AccountsController(IFinanceService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var accounts = _service.GetAllAccounts();
        return Ok(accounts);
    }

    [HttpPost]
    public IActionResult Create([FromBody] AccountDTO dto)
    {
        try
        {
            _service.CreateAccount(dto);
            return Ok(new { message = "Account created successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] string newName)
    {
        try
        {
            _service.UpdateAccount(id, newName);
            return Ok("Account updated.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _service.DeleteAccount(id);
            return Ok(new { message = "Account deleted successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}