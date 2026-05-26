using System;
using BLL.DTO;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IFinanceService _service;

    public TransactionsController(IFinanceService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var transactions = _service.GetTransactionHistory();
        return Ok(transactions);
    }

    [HttpPost]
    public IActionResult Create([FromBody] TransactionDTO dto)
    {
        try
        {
            _service.MakeTransaction(dto);
            return Ok(new { message = "Transaction recorded successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferDTO dto)
    {
        try
        {
            _service.TransferFunds(dto);
            return Ok(new { message = "Transfer completed successfully!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}