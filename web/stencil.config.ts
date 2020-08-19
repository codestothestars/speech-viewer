import { Config } from '@stencil/core';

export const config: Config = {
  globalScript: 'src/app.ts',
  globalStyle: 'src/app.css',
  outputTargets: [
    {
      serviceWorker: null,
      type: 'www'
    }
  ],
  taskQueue: 'async'
};
