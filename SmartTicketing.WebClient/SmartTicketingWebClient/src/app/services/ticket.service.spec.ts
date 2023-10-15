
import { HttpClient } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { AuthService } from './auth.service';

import { TicketService } from './ticket.service';

describe('TicketService', () => {
    let authServiceSpy: jasmine.SpyObj<AuthService>;
    let httpClientSpy: jasmine.SpyObj<HttpClient>;

    let service: TicketService;

    beforeEach(() => {
        httpClientSpy = jasmine.createSpyObj('HttpClient', ['get']);

        TestBed.configureTestingModule({
            imports: [],
            providers: [{ provide: AuthService, useValue: authServiceSpy }]
        });

        service = new TicketService(authServiceSpy, httpClientSpy);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
