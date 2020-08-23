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

### End-to-End Tests

A set of end-to-end tests can be run from the front-end to test the full application.

```Shell
npm test
```

Note that the [API server](../api) must be running in order for the tests to pass. The values in the [`.npmrc`](./.npmrc) file configure the URLs used by the app and tests, which you can adjust to your environment if necessary.

### Contributing

Before committing changes, make sure that you...

1. Write/edit [JSDoc](https://jsdoc.app) documentation for all new/modified members.
1. Run [ESLint](https://eslint.org) with `npm run lint` and correct all errors.
