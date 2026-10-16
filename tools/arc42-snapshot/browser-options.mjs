export function getPuppeteerLaunchOptions(environment = process.env) {
  return {
    args: environment.GITHUB_ACTIONS === 'true'
      ? ['--no-sandbox', '--disable-setuid-sandbox']
      : [],
  };
}
