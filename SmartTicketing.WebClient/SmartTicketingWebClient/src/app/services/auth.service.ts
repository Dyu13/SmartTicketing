import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of, throwError } from 'rxjs';
import { catchError, mapTo, tap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AuthModel } from '../models/auth.model';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly JWT_TOKEN = 'JWT_TOKEN';
    private baseUrl = environment.apiUrl;

    constructor(private router: Router, private httpClient: HttpClient) { }

    login(userName: string, userPw: string): Observable<boolean> {
        let url = `${this.baseUrl}/auth`;
        var headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'X-API-Key': 'SmartTicketingSecret'
        });

        var user: AuthModel = { User: userName, Password: userPw };

        return this.httpClient.post(url, user, { headers: headers, responseType: 'text' })
            .pipe(
                tap(data => this.storeJwtToken(data)),
                mapTo(true),
                catchError(error => {
                    this.handleError(error);
                    return of(false);
                })
            );
    }

    logout() {
        this.removeToken();
        this.router.navigate(['./auth']);
    }

    isLoggedIn() {
        return !!this.getJwtToken();
    }

    getJwtToken() {
        return localStorage.getItem(this.JWT_TOKEN);
    }

    public decodeToken(token: string = ''): string {
        if (token === null || token === '') { return ""; }

        const parts = token.split('.');
        if (parts.length !== 3) {
            throw new Error('JWT must have 3 parts');
        }

        const decoded = this.urlBase64Decode(parts[1]);
        if (!decoded) {
            throw new Error('Cannot decode the token');
        }

        let tokenData: string = JSON.parse(decoded);
        return tokenData;
    }

    //#region Private Methods

    private storeJwtToken(jwt: string) {
        localStorage.setItem(this.JWT_TOKEN, jwt);
    }

    private removeToken() {
        localStorage.removeItem(this.JWT_TOKEN);
    }

    private urlBase64Decode(str: string) {
        let output = str.replace(/-/g, '+').replace(/_/g, '/');
        switch (output.length % 4) {
            case 0:
                break;
            case 2:
                output += '==';
                break;
            case 3:
                output += '=';
                break;
            default:
                // tslint:disable-next-line:no-string-throw
                throw 'Illegal base64url string!';
        }
        return decodeURIComponent((<any>window).escape(window.atob(output)));
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

    //#endregion Private Methods
}
