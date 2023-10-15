import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';

import { AuthService } from './auth.service';

@Component({
    template: ''
})
class DummyComponent {
}

describe('AuthService', () => {
    let httpClientSpy: jasmine.SpyObj<HttpClient>;
    let router: Router;
    let service: AuthService;

    beforeEach(() => {
        httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);

        TestBed.configureTestingModule({
            imports: [RouterTestingModule.withRoutes([
                { path: 'auth', component: DummyComponent }
            ])]
        });

        router = TestBed.get(Router);

        service = new AuthService(router, httpClientSpy);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
