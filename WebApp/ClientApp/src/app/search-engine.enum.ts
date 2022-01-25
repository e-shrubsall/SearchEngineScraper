
export enum SearchEngine {
  Google,
  Bing
}

export namespace SearchEngine {
  export function getName(searchEngine: SearchEngine): string {
    switch (searchEngine) {
      case SearchEngine.Google:
        return "Google";
      case SearchEngine.Bing:
        return "Bing"
      default:
        return "?";
    }
  }
}
