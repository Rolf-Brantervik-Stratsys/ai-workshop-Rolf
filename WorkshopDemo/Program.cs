using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

Console.WriteLine("");
Console.WriteLine("===========================================");
Console.WriteLine("  Welcome to Strat-AI-chat");
Console.WriteLine("===========================================");
Console.WriteLine("");
Console.WriteLine("Powered by Semantic Kernel\n");

var selectedModel = SelectModel();

Console.WriteLine($"\nUsing model: {selectedModel}");
Console.WriteLine("\nType your messages and I'll respond.");
Console.WriteLine("Press Ctrl+C to exit.\n");

var kernel = BuildSemanticKernel(selectedModel);

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

Kernel BuildSemanticKernel(string model)
{
    // Read API configuration from environment variables
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is required. Set it with: $env:OPENAI_API_KEY='your-api-key'");
    var endpoint = Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? "https://ai-workshop-251201-resource.openai.azure.com/";

    // Build Semantic Kernel
    var kernelBuilder = Kernel.CreateBuilder();

    kernelBuilder.AddAzureOpenAIChatCompletion(model, endpoint, apiKey);

    return kernelBuilder.Build();
}

string SelectModel()
{
    string? model = null;
    while (model == null)
    {
        Console.WriteLine("Please select a model:");
        Console.WriteLine("1. gpt-4o-mini");
        Console.WriteLine("2. gpt-5.1-chat");
        Console.Write("\nEnter your choice (1-2): ");

        var choice = Console.ReadLine();
        model = choice switch
        {
            "1" => "gpt-4o-mini",
            "2" => "gpt-5.1-chat",
            _ => null
        };

        if (model == null)
        {
            Console.WriteLine("");
        }
    }

    return model;
}
