import { Component } from '@angular/core';
import {QueryRequest} from "./query-request.model";
import {SearchEngine} from "./search-engine.enum";
import {HttpClient, HttpHeaders} from "@angular/common/http";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  constructor(public httpClient: HttpClient) {
  }

  title = 'app';
  queryRequest = new QueryRequest();

  supportedEngines = [SearchEngine.Google, SearchEngine.Bing];

  getName(engine: SearchEngine): string {
    return SearchEngine.getName(engine);
  }

  ranking = 0;

  httpOtions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  }

  getResults(): void {
    var body = JSON.stringify(this.queryRequest);
    this.ranking = 0;
    this.httpClient.post<number>("Scraper/GetRanking", body, this.httpOtions).subscribe(r => this.ranking = r);
  }
}
