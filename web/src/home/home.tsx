import { Component, h, Host, State } from '@stencil/core';

/**
 * The home page.
 */
@Component({
  scoped: true,
  styleUrl: 'home.scss',
  tag: 'app-home'
})
export class Home {
  /**
   * Indicates whether the speech viewer cannot be displayed and an error is
   * displayed instead.
   */
  @State()
  public error = false;

  /**
   * The content of the speech currently being displayed.
   */
  @State()
  public speech?: string[];

  /**
   * Renders the home page.
   *
   * @returns The rendered home page.
   */
  public render(): unknown {
    // eslint-disable-next-line @typescript-eslint/init-declarations
    let content: unknown;

    if (this.error) {
      content = (
        <div class='error'>
          The speech viewer was unable to retrieve speeches from the API. This
          might mean that the API is currently unavailable, or it might mean
          that the API data needs to be initialized. Try initializing the data
          with the button below.
        </div>
      ) as unknown;
    } else {
      content = <app-speech-viewer onLoadError={this.showError} /> as unknown;
    }

    return (
      <Host>
        <h1>Speech Viewer</h1>

        {content}

        <div class='buttons'>
          <button class='initialize' onClick={this.initialize}>
            Initialize Data
          </button>

          <button class='destroy' onClick={this.destroy}>
            Delete Data
          </button>
        </div>
      </Host>
    ) as unknown;
  }

  /**
   * Destroys the data in the API and changes the component to its error state.
   *
   * @returns A promise that resolves when the data has been destroyed.
   */
  private readonly destroy = async (): Promise<void> => {
    this.error = true;

    try {
      await fetch('__apiUrl__/destroy', { method: 'POST' });
    } catch { } // eslint-disable-line no-empty
  };

  /**
   * Initializes the data in the API and afterward loads the available speeches
   * into the component.
   *
   * @returns A promise that resolves when the speeches have been loaded.
   */
  private readonly initialize = async (): Promise<void> => {
    try {
      await fetch('__apiUrl__/initialize', { method: 'POST' });

      this.error = false;
    } catch { } // eslint-disable-line no-empty
  };

  /**
   * Places the component into its error state, displaying an error instead of
   * the speech viewer.
   */
  private readonly showError = (): void => {
    this.error = true;
  };
}
