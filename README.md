# AI Workshop - Semantic Kernel & GitHub Copilot CLI Demo

This project is a demonstration application for a workshop on using **Microsoft Semantic Kernel** and **GitHub Copilot CLI**. It showcases how to build an intelligent chat application that integrates with Azure OpenAI services.

## Overview

This is a .NET console application that creates an interactive AI chatbot powered by Azure OpenAI through Microsoft's Semantic Kernel framework. The application demonstrates:

- **Semantic Kernel Integration**: Using Microsoft's Semantic Kernel to orchestrate AI capabilities
- **Azure OpenAI Integration**: Connecting to Azure OpenAI services for chat completion
- **Conversational AI**: Maintaining chat history for contextual conversations
- **Environment Configuration**: Managing API keys and settings through environment variables

## Prerequisites

Before running this application, ensure you have:

- **.NET 10.0 SDK** or later installed ([Download here](https://dotnet.microsoft.com/download))
- **Azure OpenAI Service** access with a deployed model
- **API Key** for Azure OpenAI
- **GitHub Copilot CLI** (optional, for workshop demonstrations)

## Project Structure

```
ai-workshop-251201/
├── WorkshopDemo/
│   ├── Program.cs           # Main application code
│   └── WorkshopDemo.csproj  # Project configuration
├── .gitignore
└── README.md
```

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/axel-bergman-stratsys/ai-workshop-251201.git
cd ai-workshop-251201
```

### 2. Configure Environment Variables

The application requires the following environment variables:

#### Windows (PowerShell)
```powershell
$env:OPENAI_API_KEY = "your-azure-openai-api-key"
$env:OPENAI_MODEL = "gpt-4o-mini"  # Optional, defaults to gpt-4o-mini
```

#### Linux/MacOS (Bash)
```bash
export OPENAI_API_KEY="your-azure-openai-api-key"
export OPENAI_MODEL="gpt-4o-mini"  # Optional, defaults to gpt-4o-mini
```

**Note**: The Azure OpenAI endpoint is currently configured in the code as:
- Endpoint: `https://ai-workshop-251201-resource.openai.azure.com/`

If you need to use a different endpoint, update line 14 in `Program.cs`.

### 3. Build the Project

```bash
cd WorkshopDemo
dotnet build
```

### 4. Run the Application

```bash
dotnet run
```

## Usage

Once the application starts, you'll be prompted to select a model:

```
===========================================
  Welcome to Strat-AI-chat
===========================================

Powered by Semantic Kernel

Please select a model:
1. gpt-4o-mini
2. gpt-5.1-chat

Enter your choice (1-2):
```

### Model Selection

- **gpt-4o-mini**: Fast and cost-effective model, suitable for most tasks
- **gpt-5.1-chat**: Advanced model with enhanced capabilities

After selecting a model, you can start chatting:

- Type your messages and press Enter to chat with the AI
- The AI maintains conversation context throughout the session
- Press **Ctrl+C** to exit the application

### Example Interaction

```
Enter your choice (1-2): 1

Using model: gpt-4o-mini

You: What is Semantic Kernel?
Bot: Semantic Kernel is an open-source SDK that lets you easily combine AI services...

You: How can I use it in my projects?
Bot: You can use Semantic Kernel by...
```

## Technologies Used

- **[.NET 10.0](https://dotnet.microsoft.com/)** - Application framework
- **[Microsoft Semantic Kernel](https://github.com/microsoft/semantic-kernel)** (v1.67.1) - AI orchestration framework
- **[Azure OpenAI Service](https://azure.microsoft.com/products/ai-services/openai-service)** - AI model hosting and inference
- **GitHub Copilot CLI** - Development assistance tool (for workshop demonstrations)

## Workshop Topics

This demo project is designed to illustrate:

1. **Setting up Semantic Kernel** in a .NET application
2. **Configuring Azure OpenAI** chat completion services
3. **Managing chat history** for contextual conversations
4. **Error handling** in AI applications
5. **Environment-based configuration** for secure API key management
6. **Using GitHub Copilot CLI** for development acceleration

## Troubleshooting

### Common Issues

**Issue**: `OPENAI_API_KEY environment variable is required`
- **Solution**: Set the `OPENAI_API_KEY` environment variable before running the application

**Issue**: Build fails with SDK version error
- **Solution**: Ensure .NET 10.0 SDK or later is installed

**Issue**: Connection errors to Azure OpenAI
- **Solution**: Verify your API key is valid and the endpoint URL is correct

## Contributing

This is a workshop demonstration project. Feel free to fork and experiment with it for learning purposes.

## License

This project is created for educational workshop purposes.

## Resources

- [Semantic Kernel Documentation](https://learn.microsoft.com/semantic-kernel/)
- [Azure OpenAI Service Documentation](https://learn.microsoft.com/azure/ai-services/openai/)
- [GitHub Copilot Documentation](https://docs.github.com/copilot)
- [.NET Documentation](https://learn.microsoft.com/dotnet/)