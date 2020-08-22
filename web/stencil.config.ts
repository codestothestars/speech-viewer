import replace from '@rollup/plugin-replace';
import { Config } from '@stencil/core';
import { sass } from '@stencil/sass';

export const config: Config = {
  globalScript: 'src/app.ts',
  globalStyle: 'src/app.css',
  outputTargets: [
    {
      serviceWorker: null,
      type: 'www'
    }
  ],
  plugins: [
    sass()
  ],
  rollupPlugins: {
    before: [
      replace({
      })
    ]
  },
  taskQueue: 'async'
};
