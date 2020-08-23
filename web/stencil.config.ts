import replace from '@rollup/plugin-replace';
import { Config } from '@stencil/core';
import { sass } from '@stencil/sass';

const { npm_config_speech_viewer_api } = process.env;

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
        __apiUrl__: npm_config_speech_viewer_api
      })
    ]
  },
  taskQueue: 'async'
};
