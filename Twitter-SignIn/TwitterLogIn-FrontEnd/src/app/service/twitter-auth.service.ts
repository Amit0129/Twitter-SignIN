import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RequestToken } from '../model/RequestToken.model';


@Injectable({
  providedIn: 'root'
})
export class TwitterAuthService {

  baseUrl ="https://localhost:44367/api/";
  constructor(private http: HttpClient) { }

  getRequestToken(): Observable<RequestToken> {
    return this.http.get<RequestToken>(this.baseUrl +'TwitterClient/GetRequestToken');
  }
}
