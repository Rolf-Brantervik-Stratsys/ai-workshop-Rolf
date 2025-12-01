using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

Console.WriteLine("===========================================");
Console.WriteLine("  Welcome to the AI Chat Application!");
Console.WriteLine("===========================================");
Console.WriteLine("Powered by Semantic Kernel");
Console.WriteLine("Type your messages and I'll respond.");
Console.WriteLine("Type 'exit' or 'quit' to leave.\n");

// Read API configuration from environment variables
var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
const string endpoint = "https://ai-workshop-251201-resource.openai.azure.com/";
var model = Environment.GetEnvironmentVariable("OPENAI_MODEL") ?? "gpt-4o-mini";

if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("Warning: OPENAI_API_KEY environment variable not set.");
    Console.WriteLine("Please set it with: $env:OPENAI_API_KEY='your-api-key'\n");
    Console.WriteLine("Falling back to simple response mode...\n");
}

// Build Semantic Kernel
var builder = Kernel.CreateBuilder();

if (!string.IsNullOrEmpty(apiKey))
{
    builder.AddAzureOpenAIChatCompletion(model, endpoint, apiKey);
}

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

    string response;
    if (!string.IsNullOrEmpty(apiKey))
    {
        try
        {
            var result = await chatService.GetChatMessageContentAsync(chatHistory);
            response = result.Content ?? "I'm not sure how to respond to that.";
            chatHistory.AddAssistantMessage(response);
        }
        catch (Exception ex)
        {
            response = $"Error: {ex.Message}";
        }
    }
    else
    {
        response = GenerateSimpleResponse(userInput);
    }

    Console.WriteLine($"Bot: {response}\n");
}

static string GenerateSimpleResponse(string input)
{
    var lowerInput = input.ToLower();

    if (lowerInput.Contains("hello") || lowerInput.Contains("hi"))
    {
        return "Hello! How can I help you today?";
    }
    else if (lowerInput.Contains("how are you"))
    {
        return "I'm doing great, thank you for asking!";
    }
    else if (lowerInput.Contains("help"))
    {
        return "I'm here to chat with you. Just type anything and I'll respond!";
    }
    else if (lowerInput.Contains("name"))
    {
        return "I'm ChatBot, your friendly console assistant!";
    }
    else
    {
        return $"You said: '{input}'. That's interesting! Tell me more.";
    }
}
