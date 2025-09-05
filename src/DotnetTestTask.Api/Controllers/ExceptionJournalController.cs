using DotnetTestTask.Application.GetExceptionJournal;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotnetTestTask.Api.Controllers;

[ApiController]
[Route("api")]
public class ExceptionJournalController(ISender sender): Controller
{
    [HttpGet("exception-journal")]
    public async Task<ActionResult<List<JournalRecordDto>>> GetExceptionJournal(
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
       var result = await sender.Send(new GetExceptionJournalQuery(page, pageSize));
       return Ok(result);
    }
}
