import { launch, Page, Browser, ElementHandle, JSHandle } from 'puppeteer';

describe('Speech Viewer', () => {
  let browser: Browser;
  let page: Page;
  let options: JSHandle | undefined;
  let select: ElementHandle | null;

  beforeAll(async () => {
    browser = await launch();
  });

  beforeEach(async () => {
    page = await browser.newPage();

    // eslint-disable-next-line no-process-env, no-undef
    await page.goto(process.env.npm_config_speech_viewer_web as string);
  });

  afterEach(async () => {
    await page.close();
  });

  afterAll(async () => {
    await browser.close();
  });

  describe('on load', () => {
    test('displays either the speech viewer or an error message', async () => {
      const content = await page.waitFor('select, .error');

      expect(content).not.toBeNull();
    });
  });

  describe('when the user destroys the data', () => {
    beforeEach(destroy);

    testDisplaysError();

    describe('and then initializes the data', () => {
      beforeEach(initialize);

      testDisplaysSpeechViewer();
    });
  });

  describe('when the user initializes the data', () => {
    beforeEach(initialize);

    testDisplaysSpeechViewer();

    describe('and then destroys the data', () => {
      beforeEach(async () => {
        await waitForSelect();

        await destroy();
      });

      testDisplaysError();
    });

    describe('after the speeches have loaded', () => {
      beforeEach(async () => {
        select = await waitForSelect();
      });

      test('selects a non-choice by default', async () => {
        const selectedOptions = await select?.getProperty('selectedOptions');

        const option = await selectedOptions?.getProperty('0');

        const textHandle = await option?.getProperty('text');

        const text = await textHandle?.jsonValue();

        expect(text).toMatch(/^Select/u);
      });

      test('provides speeches to select', async () => {
        await getOptions();

        const lengthHandle = await options?.getProperty('length');

        const length = await lengthHandle?.jsonValue();

        expect(length).toBeGreaterThan(1);
      });

      describe('and the user selects a speech', () => {
        let speech: number;

        beforeEach(async () => {
          speech = await getRandomSpeech();

          await selectOption(speech);

          await page.waitFor('blockquote p');
        });

        test('displays the content of the speech', async () => {
          await expectSpeechContent();
        });

        test('displays the author after the speech', async () => {
          const footer = await page.$('blockquote p ~ footer');

          const textHandle = await footer?.getProperty('textContent');

          const text = await textHandle?.jsonValue();

          expect(text).toMatch(/\w/u);
        });

        describe('and then selects the default option', () => {
          beforeEach(async () => {
            await selectOption(0);
          });

          test('continues displaying the speech', async () => {
            await expectSpeechContent();
          });
        });
      });
    });
  });

  async function clickButton(selector: string) {
    const button = await page.waitFor(selector);

    await page.waitFor(1000);

    return button.click();
  }

  async function destroy() {
    return clickButton('.destroy');
  }

  async function expectSpeechContent() {
    const paragraphs = await page.$$('blockquote p');

    const textHandles = await Promise.all(
      paragraphs
        .map(async paragraph => paragraph.getProperty('textContent'))
    );

    const text = await Promise.all(
      textHandles.map(async handle => handle.jsonValue())
    );

    expect(text.length)
      .toBeGreaterThan(0);

    expect(text)
      .toEqual(text.map(() => expect.stringMatching(/\w/u) as string));
  }

  async function initialize() {
    return clickButton('.initialize');
  }

  async function getOptions() {
    options = await select?.getProperty('options');
  }

  function getRandomInt(minimum: number, maximum: number) {
    const min = Math.ceil(minimum);

    const max = Math.floor(maximum);

    const base = Math.random() * (max - min);

    return Math.floor(base + min);
  }

  async function getRandomSpeech() {
    await getOptions();

    const lengthHandle = await options?.getProperty('length');

    const length = await lengthHandle?.jsonValue() as number;

    return getRandomInt(1, length);
  }

  async function selectOption(index: number) {
    const option = await options?.getProperty(String(index));

    const valueHandle = await option?.getProperty('value');

    const value = await valueHandle?.jsonValue() as string;

    return select?.select(value);
  }

  function testDisplaysError() {
    test('displays an error message and hides the speech viewer', async () => {
      const error = await page.waitFor('.error');

      select = await page.$('select');

      expect(error).not.toBeNull();

      expect(select).toBeNull();
    });
  }

  function testDisplaysSpeechViewer() {
    test('displays the speech select and no error message', async () => {
      select = await waitForSelect();

      const error = await page.$('.error');

      expect(select).not.toBeNull();

      expect(error).toBeNull();
    });
  }

  async function waitForSelect() {
    return page.waitFor('select');
  }
});
