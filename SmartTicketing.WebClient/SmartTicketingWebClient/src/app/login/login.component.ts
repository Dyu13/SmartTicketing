import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms'
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AuthModel } from '../models/auth.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  isLoading: boolean | undefined;

  constructor(
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm): void {
    this.isLoading = true;

    var formValue = JSON.stringify(form.value);
    const authModel: AuthModel = JSON.parse(formValue);

    this.authService.login(authModel.User, authModel.Password).subscribe((isAuthenticated: Boolean) => {
      this.isLoading = false; // TODO: Display a spinner based on ngIf

      if (isAuthenticated) {
        // window.location.reload(); // TODO: refresh shared component
        this.router.navigate(['./tickets'])
      }

      // TODO: notify user when failed
    });
  }
}
