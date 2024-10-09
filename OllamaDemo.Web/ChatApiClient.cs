namespace OllamaDemo.Web;

public class ChatApiClient(HttpClient httpClient)
{
    public async Task<string> GetMessageAsync(string message)
    {
        var response =  await httpClient.PostAsJsonAsync<string>("/chat", message);
        
        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
    }
}
