using Microsoft.Extensions.AI;
namespace OllamaDemo.ApiService.Interfaces;

public interface IChatClient : IDisposable
{
    Task<ChatCompletion> CompleteAsync(string message); 
    IAsyncEnumerable<StreamingChatCompletionUpdate> CompleteStreamingAsync(); 
    ChatClientMetadata Metadata { get; } 
    TService? GetService<TService>(object? key = null) where TService : class;
}
