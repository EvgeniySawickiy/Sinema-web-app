import {Component, inject} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {CommonModule, NgIf} from "@angular/common";
import {AuthService} from "../../../auth/auth.service";
import {Router, RouterLink} from '@angular/router';

@Component({
  selector: 'app-login-page',
  imports: [
    ReactiveFormsModule,
    NgIf,
    CommonModule,
    RouterLink,
  ],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss'
})
export class LoginPageComponent {
  loginForm: FormGroup;
  authService = inject(AuthService);
  Router  = inject(Router);
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      login: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const { login, password } = this.loginForm.value;

           this.authService.login({ login, password }).subscribe(
             (result) => {
               this.errorMessage = null;
               this.Router.navigateByUrl('');
               console.log(result);
             },
             (error) => {
               console.error('Login failed:', error);
               if (error.error && error.error.error) {
                 this.errorMessage = error.error.error;
               } else {
                 this.errorMessage = 'An unexpected error occurred. Please try again.';
               }
             }
           );
    }
  }
}
