FROM node:slim

WORKDIR /app
COPY ["SmartMirrorHubV6.Dockers/TpLinkDecoScraper/package.json", "SmartMirrorHubV6.Dockers/TpLinkDecoScraper/package-lock.json*", "SmartMirrorHubV6.Dockers/TpLinkDecoScraper/scraper.js", "./"]
RUN npm install

# We don't need the standalone Chromium
ENV PUPPETEER_SKIP_CHROMIUM_DOWNLOAD true

# Install Google Chrome Stable and fonts
# Note: this installs the necessary libs to make the browser work with Puppeteer.
RUN apt-get update && apt-get install gnupg wget -y && \
  wget --quiet --output-document=- https://dl-ssl.google.com/linux/linux_signing_key.pub | gpg --dearmor > /etc/apt/trusted.gpg.d/google-archive.gpg && \
  sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list' && \
  apt-get update && \
  apt-get install google-chrome-stable -y --no-install-recommends && \
  rm -rf /var/lib/apt/lists/*
RUN apt-get update && apt-get install cron -y && \
    chmod 0644 ./scraper.js && \
    touch /var/log/cron.log && \
    (crontab -l ; echo "* * * * * /usr/local/bin/node /app/scraper.js >> /var/log/cron.log") | crontab

# Run the command on container startup
CMD printenv > /etc/environment && cron && tail -f /var/log/cron.log