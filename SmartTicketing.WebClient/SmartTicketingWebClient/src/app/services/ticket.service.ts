import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, retry } from 'rxjs/operators'
import { AuthService } from './auth.service';
import { environment } from 'src/environments/environment';
import * as signalR from "@microsoft/signalr";

@Injectable({
    providedIn: 'root'
}) // TODO: maybe it is best for vertical architecture to move it in the component, but this can be observed with the grow
export class TicketService {
    private readonly ApiUrl = environment.apiUrl;
    private readonly odataApiUrl = environment.odataApiUrl;

    constructor(
        private authService: AuthService,
        private httpClient: HttpClient) { }

    get(page: number): Observable<any[]> {
        var token = this.authService.getJwtToken();

        var headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'X-API-Key': 'SmartTicketingSecret',
            'Authorization': `Bearer ${token}`
        });

        var skip = (page - 1) * 100;

        return this.httpClient.get<any[]>(this.odataApiUrl + `/tickets?$select=TicketId,Title,Status,Description&$skip=${skip}`, { headers: headers })
            .pipe(
                retry(3),
                catchError(error => this.handleError(error))
            );
    }

    getSummaryByDescription(description: string): void {
        let connection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:13600/summaryHub")
            .build();

        connection.on("Summary", data => {
            alert(data);

            connection.stop();
        });

        connection.start()
            .then(() => connection.invoke("SendTicketDescription", description))
            .catch(err => console.log('Error while starting connection: ' + err));
    }

    getCount(): Observable<any> {
        var token = this.authService.getJwtToken();

        var headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'X-API-Key': 'SmartTicketingSecret',
            'Authorization': `Bearer ${token}`
        });

        return this.httpClient.get(this.ApiUrl + '/ticketsCount', { headers: headers })
            .pipe(
                retry(3),
                catchError(error => this.handleError(error))
            );
    }

    private handleError(err: HttpErrorResponse) {
        let errorMessage: string;
        if (err.error instanceof ErrorEvent) {
            errorMessage = `An error occurred: ${err.error.message}`;
        } else {
            errorMessage = `Backend returned code ${err.status}: ${err.statusText}`;
        }

        console.error(err); // TODO: log
        return throwError(errorMessage);
    }
}
