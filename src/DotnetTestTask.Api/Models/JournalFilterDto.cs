namespace DotnetTestTask.Api.Models;

public class JournalFilterDto
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? Search { get; set; }
}
