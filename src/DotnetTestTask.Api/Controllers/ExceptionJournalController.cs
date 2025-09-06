using DotnetTestTask.Api.Models;
using DotnetTestTask.Application.GetExceptionJournal;
using DotnetTestTask.Application.GetExceptionJournalRecordById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotnetTestTask.Api.Controllers;

[ApiController]
public class ExceptionJournalController(ISender sender) : Controller
{
    [HttpPost("api.user.journal.getRange")]
    public async Task<ActionResult<List<JournalRecordDto>>> GetExceptionJournal(
        [FromQuery] int skip,
        [FromQuery] int take,
        [FromBody] JournalFilterDto? filter)
    {
        var result = await sender.Send(
             new GetExceptionJournalQuery(
                 skip,
                 take,
                 filter?.From,
                 filter?.To,
                 filter?.Search));

        return Ok(result);
    }

    [HttpPost("api.user.journal.getSingle")]
    public async Task<ActionResult<List<JournalRecordDto>>> GetExceptionJournal(
        [FromQuery] long id)
    {
        var result = await sender.Send(
             new GetExceptionJournalRecordByIdQuery(id));

        return Ok(result);
    }
}
