import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import * as Global from '../global.config';
import { Observable } from 'rxjs/internal/Observable';
import { IEvent } from '../index';
import { map } from 'rxjs/operators';


@Injectable()
export class DataService {

    private baseURL = Global.AzureConnect[Global.AzureConnect.env].endPoint;

    constructor(private http: HttpClient) { }

    getEventList(): Observable<IEvent[]> {
        return this.httpGet(`${this.baseURL}/events`)
            .pipe(
                map
                    (
                    (res) => res
                    )
            );

    }

    httpGet<T>(url: string): Observable<any> {
        const options = { headers: this.getHeader() };
        return this.http.get(url);
    }

    private getHeader(): HttpHeaders {

        return new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        });

    }

}
