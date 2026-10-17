#!/usr/bin/env node
import { mkdir } from 'node:fs/promises';
import { resolve, join } from 'node:path';
import { pathToFileURL } from 'node:url';
import puppeteer from 'puppeteer';
import { getPuppeteerLaunchOptions } from './browser-options.mjs';

const [snapshotArgument, outputArgument] = process.argv.slice(2);
if (!snapshotArgument || !outputArgument) {
  process.stderr.write('Usage: node capture-evidence.mjs <snapshot.html> <output-directory>\n');
  process.exit(2);
}

const snapshot = resolve(snapshotArgument);
const output = resolve(outputArgument);
await mkdir(output, { recursive: true });
const browser = await puppeteer.launch({
  headless: true,
  ...getPuppeteerLaunchOptions(),
});

const views = [
  { section: '03', diagram: 0, name: 'context-business' },
  { section: '03', diagram: 1, name: 'context-technical' },
  { section: '05', diagram: 0, name: 'building-blocks' },
  { section: '05', diagram: 1, name: 'backend-dependencies' },
  { section: '05', diagram: 2, name: 'frontend-dependencies' },
  { section: '06', diagram: 0, name: 'invoice-finalization' },
  { section: '07', diagram: 0, name: 'deployment-compose' },
  { section: '07', diagram: 1, name: 'deployment-azure' },
  { section: '10', diagram: 0, name: 'quality-tree' },
];

try {
  const page = await browser.newPage();
  await page.setViewport({ width: 1280, height: 1000, deviceScaleFactor: 1 });
  for (const view of views) {
    await page.goto(`${pathToFileURL(snapshot).href}#/section/${view.section}`, { waitUntil: 'load' });
    await page.waitForFunction((sectionNumber, diagramIndex) => {
      const content = document.querySelector('.doc');
      return location.hash === `#/section/${sectionNumber}` && content?.querySelectorAll('figure.diagram')[diagramIndex];
    }, {}, view.section, view.diagram);
    await page.evaluate((diagramIndex) => {
      const main = document.querySelector('main#main');
      const figure = document.querySelectorAll('.doc figure.diagram')[diagramIndex];
      if (main && figure) {
        main.style.scrollBehavior = 'auto';
        const mainBounds = main.getBoundingClientRect();
        const figureBounds = figure.getBoundingClientRect();
        main.scrollTop = Math.max(0, main.scrollTop + figureBounds.top - mainBounds.top - (main.clientHeight - figureBounds.height) / 2);
      }
    }, view.diagram);
    await new Promise((resolve) => setTimeout(resolve, 500));
    await page.screenshot({ path: join(output, `arc42-${view.name}.png`) });
  }
  await page.setViewport({ width: 375, height: 812, deviceScaleFactor: 1 });
  await page.goto(`${pathToFileURL(snapshot).href}#/section/03`, { waitUntil: 'load' });
  await page.waitForFunction(() => document.querySelector('.doc figure.diagram'));
  await page.click('#nav-close');
  await page.waitForFunction(() => !document.body.classList.contains('mobile-nav-open'));
  await page.evaluate(() => document.activeElement?.blur());
  await page.screenshot({
    path: join(output, 'arc42-context-business-mobile.png'),
    fullPage: true,
  });
} finally {
  await browser.close();
}
