# Speech Viewer [![api](https://github.com/codestothestars/speech-viewer/workflows/API%20Build-Test/badge.svg?branch=develop)](https://github.com/codestothestars/speech-viewer/actions?query=workflow%3A%22API+Build-Test%22) [![web](https://github.com/codestothestars/speech-viewer/workflows/Web%20Build/badge.svg?branch=develop)](https://github.com/codestothestars/speech-viewer/actions?query=workflow%3A%22Web+Build%22)

A website for viewing famous historical speeches.

## Development

The speech viewer consists of multiple distinct components. For simplicity in coordinating these components, each is contained within its own subdirectory of this repository.

- [`api`](./api) - The API server, an [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions) application built using [.NET Core](https://docs.microsoft.com/en-us/dotnet/core).
- [`web`](./web) - The web front-end, a [Stencil](https://stenciljs.com) application built using [Node.js](https://nodejs.org).

Each directory contains its own README with instructions for building and running.

### Testing

A set of end-to-end tests for the full application is provided in the [web front-end](./web). See the web README for instructions.

### Contributing

Before committing changes, make sure that you...

1. Write/edit documentation for all new/modified members.
1. Write/edit tests for all new/modified functionality.
1. Run linting where applicable and correct all errors.
1. Run tests and correct all failures.

See each component directory for specific commands and tools.

### Branching Model

This project uses the following branching rules.

- `master` contains the current production state. Development does not occur here.
- `develop` contains the current development state planned for the next release. Feature branches are created from here and merged back in when the feature is complete.
- Use a named feature branch for each feature in development. This is where all main development should occur.
- `release-*` branches are created from `develop` to prepare the next release. Perform final testing and version checking here, then merge into `master` to perform a production release and back into `develop` to update development.
