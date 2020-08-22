import { Component, h, Prop } from '@stencil/core';
import { AuthorInfo } from '../author-info';

/**
 * Displays the content of a speech.
 */
@Component({
  scoped: true,
  styleUrl: 'speech-content.scss',
  tag: 'app-speech-content'
})
export class SpeechContent {
  /**
   * The author who wrote the speech.
   */
  @Prop()
  public author?: AuthorInfo;

  /**
   * The speech content.
   */
  @Prop()
  public content?: string[];

  /**
   * Renders the speech content.
   *
   * @returns The rendered speech content.
   */
  public render(): unknown {
    const { author, content } = this;

    const { firstName, lastName } = author ?? {};

    const paragraphs = content?.map(paragraph => <p>{paragraph}</p> as unknown);

    if (paragraphs === undefined) {
      return undefined;
    }

    return (
      <blockquote>
        {paragraphs}

        <footer>&mdash;&nbsp;{firstName} {lastName}</footer>
      </blockquote>
    ) as unknown;
  }
}
