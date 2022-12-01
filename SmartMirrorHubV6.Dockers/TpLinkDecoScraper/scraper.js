'use strict';

import puppeteer from 'puppeteer-core';
import fs from 'fs'

(async () => {
  const browser = await puppeteer.launch({headless: true, args: ["--no-sandbox", "--unlimited-storage"], executablePath: "/usr/bin/google-chrome-stable"});
  const page = await browser.newPage();

  await page.goto('http://tplinkdeco.net/');
  await page.waitForSelector("input[type=password]");
  await page.type('input[type=password]', process.env.PASSWORD);
  await page.waitForTimeout(2000);
  await page.click("a[title='LOG IN']");
  await page.waitForSelector("#map-clients");
  await page.click("#map-clients");
  
  var hasResults = false;
  while (hasResults === false) {
    const data = await page.evaluate(() => {
        const tds = Array.from(document.querySelectorAll(".grid-content-container table tr td[name='deviceName'] .td-content .device-info-container .name"));
        return tds.map(td => td.innerText);
      });

    hasResults = data.length > 0; 
    if (hasResults) {   
        var json = JSON.stringify(data);
        console.log(json);
        fs.writeFileSync('/app/output/tp-link-scraper-devices.json', json);
    }
    await new Promise(r => setTimeout(r, 1000));
  }

  await browser.close();
})();