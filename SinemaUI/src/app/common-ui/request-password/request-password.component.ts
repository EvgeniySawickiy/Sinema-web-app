import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';
import {AuthService} from '../../auth/auth.service';

@Component({
  selector: 'app-request-password',
  imports: [
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './request-password.component.html',
  styleUrl: './request-password.component.scss'
})
export class RequestPasswordComponent {
  passwordResetForm: FormGroup;
  message: string | null = null;
  error: string | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.passwordResetForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    if (this.passwordResetForm.valid) {
      const email = this.passwordResetForm.value.email;

      this.authService.requestPasswordReset(email).subscribe(
        () => {
          this.message = 'Password reset link has been sent to your email.';
          this.error = null;
        },
        (err) => {
          this.message = null;
          this.error = 'Failed to send reset link. Please try again.';
        }
      );
    }
  }
}
