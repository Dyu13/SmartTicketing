import { Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthService } from '../auth.service';

import { AuthGuard } from './auth.guard';

@Component({
    template: ''
})
class DummyComponent {
}

describe('AuthGuard', () => {
    let authService: AuthService;
    let authServiceStub: Partial<AuthService>;

    let guard: AuthGuard;

    beforeEach(() => {
        // stub AuthService for test purposes
        authServiceStub = {
            isLoggedIn(): boolean { return false; }
        }

        TestBed.configureTestingModule({
            imports: [RouterTestingModule.withRoutes([
                { path: 'auth', component: DummyComponent }
            ])],
            providers: [{ provide: AuthService, useValue: authServiceStub }]
        });

        // AuthService from the root injector
        authService = TestBed.inject(AuthService);

        guard = TestBed.inject(AuthGuard);
    });

    it('should be created', () => {
        expect(guard).toBeTruthy();
    });
});
