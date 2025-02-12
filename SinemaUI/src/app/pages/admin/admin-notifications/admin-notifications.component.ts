import { Component } from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgIf} from '@angular/common';
import {NotificationService} from '../../../data/services/notification.service';

@Component({
  selector: 'app-admin-notifications',
  imports: [
    ReactiveFormsModule,
    NgIf
  ],
  templateUrl: './admin-notifications.component.html',
  styleUrl: './admin-notifications.component.scss'
})
export class AdminNotificationsComponent {
  notificationForm: FormGroup;
  statusMessage: string = '';

  constructor(private fb: FormBuilder, private notificationService: NotificationService) {
    this.notificationForm = this.fb.group({
      notificationType: ['push', Validators.required],
      deliveryType: ['broadcast', Validators.required],
      userId: [''],
      email: ['', [Validators.email]],
      bulkEmails: [''],
      subject: [''],
      message: ['', [Validators.required, Validators.minLength(3)]]
    });

    this.notificationForm.get('notificationType')?.valueChanges.subscribe(type => {
      if (type === 'push') {
        this.notificationForm.get('email')?.reset();
        this.notificationForm.get('bulkEmails')?.reset();
        this.notificationForm.get('subject')?.reset();
      }
    });

    this.notificationForm.get('deliveryType')?.valueChanges.subscribe(type => {
      if (type === 'bulk') {
        this.notificationForm.get('email')?.reset();
        this.notificationForm.get('userId')?.reset();
      } else if (type === 'user') {
        this.notificationForm.get('bulkEmails')?.reset();
      }
    });
  }

  sendNotification() {
    const { notificationType, deliveryType, userId, email, bulkEmails, subject, message } = this.notificationForm.value;

    if (notificationType === 'push') {
      if (deliveryType === 'broadcast') {
        this.notificationService.broadcastNotification(message);
        this.statusMessage = '📢 Push-уведомление отправлено всем пользователям!';
      } else if (deliveryType === 'user' && userId) {
        this.notificationService.sendNotification(userId, message);
        this.statusMessage = `👤 Push-уведомление отправлено пользователю с ID: ${userId}`;
      }
    } else if (notificationType === 'email') {
      if (deliveryType === 'user' && email) {
        this.notificationService.sendEmail(email, subject, message).subscribe({
          next: () => this.statusMessage = '✅ Email отправлен!',
          error: err => console.error('❌ Ошибка отправки Email:', err)
        });
      } else if (deliveryType === 'bulk' && bulkEmails) {
        const emailArray: string[] = bulkEmails.split(',').map((email: string) => email.trim()); // Разбиваем по запятым

        this.notificationService.sendBulkEmails(emailArray, subject, message).subscribe({
          next: () => this.statusMessage = '✅ Массовая email-рассылка завершена!',
          error: err => console.error('❌ Ошибка массовой рассылки:', err)
        });
      }
    }
  }
}
