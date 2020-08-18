# Web Front-End

The web front-end for the speech viewer.

## Development

### Dependencies

- [Node.js](https://nodejs.org) 14.8

### Install

Note that all commands below assume that the current working directory is the `web` directory containing this README.

Once you've installed the above dependencies and cloned this repository, install NPM dependencies.

```Shell
npm install
```

### Start

Use the following command to run the website in the development server provided by [Stencil](https://stenciljs.com).

```Shell
npm start
```

### Build

Use the provided build script to build a distributable version of the website.

```Shell
npm run build
```

### Contributing

Before committing changes, make sure that you...

1. Write/edit [JSDoc](https://jsdoc.app) documentation for all new/modified members.
1. Write/edit [Jest](https://jestjs.io) unit tests for all new/modified functionality.
1. Run [ESLint](https://eslint.org) with `npm run lint` and correct all errors.
1. Run all unit tests with `npm test` and correct all failures.

### Known Issues

- As of Stencil 1.17.3, code coverage does not work correctly. See ionic-team/stencil#2557 and ionic-team/stencil#2613. Presumably the next release will contain a fix.
