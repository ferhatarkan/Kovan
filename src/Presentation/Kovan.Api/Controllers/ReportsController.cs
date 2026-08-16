using Kovan.Application.Features.Reports.Queries.GetSalesSummaryReport;
using Kovan.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Kovan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = Roles.Admin)] // Raporlara sadece Admin'ler erişebilir
public class ReportsController : ControllerBase
{
    private readonly ISender _sender;

    public ReportsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("sales-summary")]
    public async Task<IActionResult> GetSalesSummary([FromQuery] GetSalesSummaryReportQuery query)
    {
        var report = await _sender.Send(query);
        return Ok(report);
    }
}