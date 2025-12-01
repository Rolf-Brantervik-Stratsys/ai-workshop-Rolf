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

var kernel = BuildSemanticKernel(selectedModel);

if (selectedModel == "csv-analysis")
{
    await AnalyzeCsvFile(kernel);
}
else
{
    await StartChat(kernel);
}

async Task AnalyzeCsvFile(Kernel kernel)
{
    Console.WriteLine("\nCSV File Analysis Mode - Swedish Language Detection");
    Console.WriteLine("Default file: data.csv");
    Console.Write("Enter CSV file path (or press Enter to use default): ");
    var filePath = Console.ReadLine();
    
    if (string.IsNullOrWhiteSpace(filePath))
    {
        filePath = "data.csv";
    }

    if (!File.Exists(filePath))
    {
        Console.WriteLine($"\nError: File '{filePath}' not found.");
        return;
    }

    var allLines = File.ReadAllLines(filePath);
    var chatService = kernel.GetRequiredService<IChatCompletionService>();
    
    const int batchSize = 20;
    int totalProcessed = 0;
    var swedishIds = new List<string>();

    Console.WriteLine("\nAnalyzing file for Swedish language content...\n");

    for (int i = 0; i < allLines.Length; i += batchSize)
    {
        var batch = allLines.Skip(i).Take(batchSize).ToArray();
        var batchText = string.Join("\n", batch);
        
        var chatHistory = new ChatHistory(
            "You are a language detection assistant. Analyze the provided CSV lines (format: Id;Text) and identify which lines contain Swedish language text. " +
            "Respond ONLY with the IDs of lines that are in Swedish, one ID per line. " +
            "If no Swedish text is found, respond with 'No Swedish text found.'");
        
        chatHistory.AddUserMessage($"Analyze these CSV lines and identify Swedish language content:\n\n{batchText}");

        try
        {
            var result = await chatService.GetChatMessageContentAsync(chatHistory);
            var response = result.Content ?? "No response received.";
            
            if (!response.Contains("No Swedish text found"))
            {
                // Extract IDs from the response
                var lines = response.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    var trimmedLine = line.Trim();
                    if (!string.IsNullOrWhiteSpace(trimmedLine) && !trimmedLine.StartsWith("No Swedish"))
                    {
                        swedishIds.Add(trimmedLine);
                    }
                }
            }
            
            totalProcessed += batch.Length;
            Console.WriteLine($"[Processed {totalProcessed} lines]");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing batch: {ex.Message}\n");
        }
    }

    Console.WriteLine($"\nAnalysis complete. Total lines processed: {totalProcessed}");
    Console.WriteLine("\n===========================================");
    Console.WriteLine("Swedish Text IDs:");
    Console.WriteLine("===========================================");
    
    if (swedishIds.Count > 0)
    {
        foreach (var id in swedishIds)
        {
            Console.WriteLine(id);
        }
        Console.WriteLine($"\nTotal Swedish entries found: {swedishIds.Count}");
    }
    else
    {
        Console.WriteLine("No Swedish text found in the file.");
    }
}

async Task StartChat(Kernel kernel)
{
    Console.WriteLine("\nType your messages and I'll respond.");
    Console.WriteLine("Press Ctrl+C to exit.\n");

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
}

Kernel BuildSemanticKernel(string model)
{
    // Read API configuration from environment variables
    var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("OPENAI_API_KEY environment variable is required. Set it with: $env:OPENAI_API_KEY='your-api-key'");
    var endpoint = Environment.GetEnvironmentVariable("OPENAI_ENDPOINT") ?? "https://ai-workshop-251201-resource.openai.azure.com/";

    // Build Semantic Kernel
    var kernelBuilder = Kernel.CreateBuilder();

    // Use gpt-4o-mini for CSV analysis
    var actualModel = model == "csv-analysis" ? "gpt-4o-mini" : model;
    
    kernelBuilder.AddAzureOpenAIChatCompletion(actualModel, endpoint, apiKey);

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
        Console.WriteLine("3. Analyze CSV file (uses gpt-4o-mini)");
        Console.Write("\nEnter your choice (1-3): ");

        var choice = Console.ReadLine();
        model = choice switch
        {
            "1" => "gpt-4o-mini",
            "2" => "gpt-5.1-chat",
            "3" => "csv-analysis",
            _ => null
        };

        if (model == null)
        {
            Console.WriteLine("");
        }
    }

    return model;
}
