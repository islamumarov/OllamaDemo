namespace OllamaDemo.Web.Model;

public class Message
{
    public string? Text { get; set; }
    public MessageOwner Owner { get; set; }
    public string Date { get; set; }
}

public enum MessageOwner
{
    User = 0,
    Ai = 1
}
