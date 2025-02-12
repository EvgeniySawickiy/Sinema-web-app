import {Component, inject} from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {NgIf} from '@angular/common';
import {AuthService} from '../../../auth/auth.service';
import {Router} from '@angular/router';
import {debounceTime, switchMap} from 'rxjs';

@Component({
  selector: 'app-register-page',
  imports: [
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss'
})
export class RegisterPageComponent {
  registerForm: FormGroup;
  loginAvailable: boolean | null = null;
  authService = inject(AuthService);
  router = inject(Router);
  errorMessage: string | null = null;

  constructor(private fb: FormBuilder) {
    this.registerForm = this.fb.group({
      login: ['', [Validators.required, Validators.minLength(6)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', [Validators.required]],
      name: ['', [Validators.required]],
      surname: ['', [Validators.required]],
    },
    {
      validators: [this.passwordsMatchValidator]
    }
    );

    this.registerForm.get('login')?.valueChanges
      .pipe(
        debounceTime(300),
        switchMap(login => this.authService.checkLoginAvailability(login))
      )
      .subscribe(response => {
        this.loginAvailable = response.available;
      });
  }

  private passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;

    if (password && confirmPassword && password !== confirmPassword) {
      return { passwordMismatch: true };
    }
    return null;
  }


  onSubmit() {
    if (this.registerForm.valid) {
      const payload = this.registerForm.value;

      this.authService.register(payload).subscribe(
        (response) => {
          const userId = response.userId;
          this.authService.sendConfirmationEmail(userId).subscribe(
            () => {
              alert('Registration is successful! A confirmation email has been sent to your email address.');
              this.router.navigate(['/login']);
            },
            (error) => {
              console.error('Error when sending an email:', error);
              alert('The registration was successful, but the confirmation email could not be sent.');
            }
          );
        },
        (error) => {
          console.error('Registration failed:', error);

          if (error.error && error.error.error === 'Login is already in use') {
            this.errorMessage = 'The username is already taken. Please choose another one.';
          } else {
            this.errorMessage = 'Registration failed. Please try again later.';
          }
        }
      );
    }
  }
}
