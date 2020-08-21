# API Server

The API server for the speech viewer. Enables clients such as the [web front-end](../web) to interact with speech data.

## Usage

Examples using PowerShell, assuming the API is being hosted at `http://localhost:7071/api`. Response bodies, where applicable, are formatted as JSON.

```PowerShell

```

## Development

### Dependencies

- [.NET Core](https://dotnet.microsoft.com/download/dotnet-core) 3.1
- [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools)

### Environment Setup

Note that all commands below assume that the current working directory is the `api` directory containing this README.

Generate a local settings file for Azure Functions.

```Shell
func settings decrypt --prefix SpeechViewer
```

Open the generated `SpeechViewer/local.settings.json` file and add the settings required by the API to the `Values` object as indicated below. The API requires an Azure SQL database for data storage.

```JSON
{
  "IsEncrypted": false,
  "Values": {
    "databaseName": "myDatabaseName",
    "databasePassword": "myDatabasePassword",
    "databaseServer": "myDatabaseServer",
    "databaseUser": "myDatabaseUser",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}
```

### Restore

Restore NuGet dependencies.

```Shell
dotnet restore SpeechViewer
```

### Build

Build the project with .NET Core.

```Shell
dotnet build SpeechViewer
```

### Run

Run the API on a local development server using the Azure Functions Core Tools.

```Shell
func start --prefix SpeechViewer
```

### Contributing

Before committing changes, make sure that you...

1. Write/edit [XML documentation](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc) for all new/modified members.
