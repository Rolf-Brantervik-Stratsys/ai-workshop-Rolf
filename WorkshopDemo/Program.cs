using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

Console.WriteLine("===========================================");
Console.WriteLine("  Welcome to the AI Chat Application!");
Console.WriteLine("===========================================");
Console.WriteLine("Powered by Semantic Kernel");
Console.WriteLine("Type your messages and I'll respond.");
Console.WriteLine("Type 'exit' or 'quit' to leave.\n");

// Read API configuration from environment variables
string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") 
    ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is required. Set it with: $env:OPENAI_API_KEY='your-api-key'");
const string endpoint = "https://ai-workshop-251201-resource.openai.azure.com/";
string model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

// Build Semantic Kernel
var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(model, endpoint, apiKey);

var kernel = builder.Build();
var chatService = kernel.GetRequiredService<IChatCompletionService>();
var chatHistory = new ChatHistory("You are a helpful, friendly AI assistant.");

while (true)
{
    Console.Write("You: ");
    var userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
    {
        continue;
    }

    if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase) || 
        userInput.Equals("quit", StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine("\nBot: Goodbye! Thanks for chatting!\n");
        break;
    }

    chatHistory.AddUserMessage(userInput);

    try
    {
        var result = await chatService.GetChatMessageContentAsync(chatHistory);
        var response = result.Content ?? "I'm not sure how to respond to that.";
        chatHistory.AddAssistantMessage(response);
        Console.WriteLine($"Bot: {response}\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}\n");
    }
}
