import { AuthorInfo } from './author-info';
import { AuthorSpeechInfo } from './author-speech-info';

/**
 * Basic information about an author and speeches that they wrote.
 */
export interface SpeechesByAuthor {
  /**
   * The author of the speeches.
   */
  author: AuthorInfo;

  /**
   * Speeches written by the author.
   */
  speeches: AuthorSpeechInfo[];
}
