import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AuthService } from '../../shared/auth.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { NgForm } from '@angular/forms';

// This is the interface for the response of the login API (Success: boolean)
interface LoginResponse {
  success: boolean;
}

@Component({
  selector: 'app-login-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login-form.component.html',
  styles: ``
})
export class LoginFormComponent {
  loginForm: FormGroup = new FormGroup({});

  constructor(public authService: AuthService, private toastr:ToastrService) { }

  onSubmit(form:NgForm) {
    if (form.valid == false) {
      return;
    }

    this.authService.login()
      .subscribe({
        next: res => {
          if ( (res as LoginResponse).success) { // Logowanie powiodło się
            this.authService.isLoggedIn = true;
            this.toastr.success('Logged in successfully', 'User Register')
          } else { // Logowanie nie powiodło się
            this.authService.isLoggedIn = false;
            this.toastr.error('Login failed', 'User Register')
          }
        },
        error: err => { // Błąd po stronie serwera
          console.log(err);
        }
      })
  }
}