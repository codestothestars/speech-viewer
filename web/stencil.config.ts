import replace from '@rollup/plugin-replace';
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
  rollupPlugins: {
    before: [
      replace({
      })
    ]
  },
  taskQueue: 'async'
};
