import { Component, h } from '@stencil/core';

/**
 * The application root.
 */
@Component({
  styleUrl: 'root.css',
  tag: 'app-root'
})
export class Root {
  /**
   * Renders the application.
   *
   * @returns The rendered application.
   */
  public render = (): unknown => (
    <ion-app>
      <ion-router useHash={false}>
        <ion-route component='app-home' url='/' />
      </ion-router>

      <ion-nav />
    </ion-app>
  );
}
