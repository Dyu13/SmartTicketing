import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  //#region Public Properties

  Title = "Smart Ticketing";
  IsAuthenticated: boolean | undefined;

  //#endregion Public Properties

  constructor(
    private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {
    this.IsAuthenticated = this.authService.isLoggedIn();
  }

  //#region Public Methods

  Login() {
    this.router.navigate(['login']);
  }

  Logout() {
    this.IsAuthenticated = false;
    this.authService.logout();
  }

  //#endregion Public Methods

}
