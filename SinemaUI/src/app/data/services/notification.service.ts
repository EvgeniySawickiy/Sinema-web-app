import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private hubConnection!: signalR.HubConnection;
  private isConnected = false;

  private apiUrl = 'http://localhost:7000/notifications';

  constructor(private toastr: ToastrService, private http: HttpClient) {}

  startConnection(): void {
    if (this.isConnected) {
      return;
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:8060/notificationHub', {
        transport: signalR.HttpTransportType.WebSockets,
        withCredentials: false
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.isConnected = true;
      })
      .catch(err => {
        console.error('🔴 Ошибка подключения:', err);
        this.isConnected = false;
      });

    this.hubConnection.onclose(error => {
      this.isConnected = false;
    });

    this.hubConnection.on('ReceiveNotification', (message: string) => {
      this.toastr.info(message, '📢 Новое уведомление', { disableTimeOut: true });
    });
  }

  sendNotification(userId: string, message: string): void {
    if (!this.isConnected) {
      return;
    }
    this.hubConnection.invoke('SendNotification', userId, message)
      .catch(err => console.error('Ошибка отправки:', err));
  }

  broadcastNotification(message: string): void {
    if (!this.isConnected) {
      return;
    }
    this.hubConnection.invoke('BroadcastNotification', message)
      .catch(err => console.error('Ошибка рассылки:', err));
  }

  sendEmail(email: string, subject: string, message: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/email`, { email, subject, message });
  }

  sendBulkEmails(emails: string[], subject: string, message: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/emails`, { emails, subject, message });
  }
}
