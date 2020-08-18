import { newSpecPage } from '@stencil/core/testing';
import { Home } from './home';

describe('home', () => {
  it('renders', async () => {
    const { root } = await newSpecPage({
      components: [Home],
      html: '<app-home></app-home>'
    });

    expect(root).toBeDefined();
  });
});
