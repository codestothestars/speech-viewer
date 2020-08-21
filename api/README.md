# API Server

The API server for the speech viewer. Enables clients such as the [web front-end](../web) to interact with speech data.

## Usage

Examples using PowerShell, assuming the API is being hosted at `http://localhost:7071/api`. Response bodies, where applicable, are formatted as JSON.

```PowerShell
# Initialize data.
Invoke-WebRequest http://localhost:7071/api/initialize -Method POST

# List all speeches grouped by author.
Invoke-WebRequest http://localhost:7071/api/speeches/by-author
# [
#     {
#         "author": {
#             "firstName": "Abraham",
#             "id": 1,
#             "lastName": "Lincoln"
#         },
#         "speeches": [
#             {"id": 1, "name": "Gettysburg Address (Bancroft)"},
#             {"id": 2, "name": "Gettysburg Address (Bliss)"}
#         ]
#     }
# ]

# Get content of a specified speech.
Invoke-WebRequest http://localhost:7071/api/speeches/1/content
# [
#     "Four score and seven years ago...",
#     "Now we are engaged...",
#     "But, in a larger sense..."
# ]

# Destroy data.
Invoke-WebRequest http://localhost:7071/api/destroy -Method POST
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

### Initialize Database

Note that the database is not populated by default, and must be initialized. To do this, run Initialize function as indicated in the first usage example above.

### Database Migrations

The API relies on database migrations to initialize and, when requested, destroy the database at runtime. Whenever you make a change to the API that requires a change to the database schema, add one SQL file that applies the migration ("up") and one that reverses it ("down") to the [migrations](./SpeechViewer/migrations) folder, following the existing `<timestamp>-<name>-<direction>.sql` convention.

Once your migration is ready, [rebuild](#build) the app and run the [Initialize](#initialize-database) function to apply the migration.

### Contributing

Before committing changes, make sure that you...

1. Write/edit [XML documentation](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/xmldoc) for all new/modified members.
1. Write/edit [MSTest](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-mstest) unit tests for all new/modified functionality.
1. Run all unit tests with `dotnet test SpeechViewerTest` and correct all failures.
