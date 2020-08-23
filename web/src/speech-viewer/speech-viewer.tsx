import {
  Component,
  ComponentDidLoad,
  Event,
  EventEmitter,
  h,
  Host,
  State
} from '@stencil/core';
import { AuthorInfo } from '../author-info';
import { SpeechesByAuthor } from '../speeches-by-author';

/**
 * Enables the user to select and view speeches.
 */
@Component({
  scoped: true,
  styleUrl: 'speech-viewer.css',
  tag: 'app-speech-viewer'
})
export class SpeechViewer implements ComponentDidLoad {
  /**
   * Event emitted when the viewer encounters an error while loading speeches
   * from the API.
   */
  @Event()
  public loadError?: EventEmitter<void>;

  /**
   * The content of the speech currently being displayed.
   */
  @State()
  public speech?: string[];

  /**
   * The speeches currently available for selection.
   */
  @State()
  public speeches?: SpeechesByAuthor[];

  /**
   * The author of the speech currently being displayed.
   */
  private author?: AuthorInfo;

  /**
   * Runs before the component renders, loading the available speeches from the
   * API.
   *
   * @returns A promise that resolves when the speeches have been loaded.
   */
  public async componentDidLoad(): Promise<void> {
    return this.loadSpeeches();
  }

  /**
   * Renders the home page.
   *
   * @returns The rendered home page.
   */
  public render(): unknown {
    const { speech } = this;

    const options = this.speeches?.map(({ author, speeches }) => {
      const { firstName, lastName } = author;

      const authorOptions = speeches.map(({ id, name }) => (
        <option value={id}>{name}</option> as unknown
      ));

      return (
        <optgroup label={`${firstName} ${lastName}`}>{authorOptions}</optgroup>
      ) as unknown;
    });

    return (
      /* eslint-disable jsx-a11y/no-onchange */
      <Host>
        <select onChange={this.change}>
          <option>Select a speech to view it.</option>

          {options}
        </select>

        <app-speech-content author={this.author} content={speech} />
      </Host>
      /* eslint-enable jsx-a11y/no-onchange */
    ) as unknown;
  }

  /**
   * Updates the page when the user selects a new speech.
   *
   * Does nothing if the user selects the default "please select" option.
   *
   * @param param0 The triggering change event.
   *
   * @returns A promise that resolves when the page's state has been updated.
   */
  private readonly change = async ({ target }: Event): Promise<void> => {
    const { value } = target as HTMLSelectElement;

    const id = Number(value);

    if (!Number.isNaN(id)) {
      try {
        await this.setSpeech(id);
      } catch (error) {
        this.loadError?.emit();
      }
    }
  };

  /**
   * Loads the available speeches from the API into the component.
   *
   * If an error is encountered while retrieving the speeches, places the
   * component into its error state.
   *
   * @returns A promise that resolves when the speeches have been loaded.
   */
  private async loadSpeeches() {
    try {
      const response = await fetch('__apiUrl__/speeches/by-author');

      const speeches = await response.json() as SpeechesByAuthor[];

      this.speeches = speeches;
    } catch (error) {
      this.loadError?.emit();
    }
  }

  /**
   * Loads the content of the speech with the specified ID from the API into the
   * component.
   *
   * @param id The identifier of the speech to load from the API.
   *
   * @returns A promise that resolves when the speech content has been loaded.
   */
  private async setSpeech(id: number) {
    const response = await fetch(`__apiUrl__/speeches/${id}/content`);

    const content = await response.json() as string[];

    this.speech = content;

    for (const { author, speeches } of this.speeches ?? []) {
      for (const speech of speeches) {
        if (speech.id === id) {
          this.author = author;

          return;
        }
      }
    }
  }
}
