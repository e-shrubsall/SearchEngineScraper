import { SearchEngine } from "./search-engine.enum";

export class QueryRequest {
  queryText: string = "";
  targetUrl: string = "";
  engine: SearchEngine = SearchEngine.Google;
}
