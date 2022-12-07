namespace CwkSocial.MinimalAPi.Contracts.Common;

public class ErrorResponse
{
    public ErrorResponse()
    {
        Errors = new();
    }
    public int StatusCode {get; set;}
    public string? StatusPhrase { get; set;}
    public List<string> Errors { get; } = new();
    public DateTime Timestamp { get; set; }
}