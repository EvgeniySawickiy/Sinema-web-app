import { Component } from '@angular/core';
import {DatePipe, NgForOf, NgIf} from '@angular/common';
import {AuthService} from '../../auth/auth.service';
import {User} from '../../data/Interfaces/user.interface';
import {BookingService} from '../../data/services/booking.service';
import {DetailedBooking} from '../../data/Interfaces/detailed-booking.interface';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'app-profile-page',
  imports: [
    NgIf,
    NgForOf,
    DatePipe,
    FormsModule
  ],
  templateUrl: './profile-page.component.html',
  styleUrl: './profile-page.component.scss'
})
export class ProfilePageComponent {
  user: User | null = null;
  bookings: DetailedBooking[] = [];
  loading = true;
  error: string | null = null;

  isEditing = false;
  editedUser: Partial<User> = {};

  constructor(private authService: AuthService, private bookingService: BookingService) {}

  ngOnInit(): void {
    this.loadUserProfile();
    this.loadUserBookings();
  }

  loadUserProfile(): void {
    this.authService.getUserProfile().subscribe(
      (data) => {
        this.user = data;
        this.loading = false;
      },
      (error) => {
        console.error('Ошибка загрузки профиля:', error);
        this.error = 'Ошибка загрузки данных.';
        this.loading = false;
      }
    );
  }

  loadUserBookings(): void {
    this.bookingService.getUserBookings().subscribe(
      (data) => {
        this.bookings = data.filter(booking => booking.status === 0).reverse();
      },
      (error) => {
        console.error('Ошибка загрузки бронирований:', error);
      }
    );
  }
  toggleEdit(): void {
    this.isEditing = !this.isEditing;

    if (this.isEditing && this.user) {
      this.editedUser = { ...this.user };
    }
  }

  saveChanges(): void {
    if (!this.user) return;
    this.authService.updateUserProfile(this.editedUser).subscribe(
      (updatedUser) => {
        this.user = updatedUser;
        this.isEditing = false;
      },
      (error) => {
        console.error('Ошибка обновления профиля:', error);
      }
    );
  }

  logout(): void {
    this.authService.logout();
  }

  canCancelBooking(startTime: string): boolean {
    const now = new Date();
    const bookingDate = new Date(startTime);
    const diffInMs = bookingDate.getTime() - now.getTime();
    const diffInHours = diffInMs / (1000 * 60 * 60);
    return diffInHours > 1;
  }

  cancelBooking(bookingId: string): void {
    if (!confirm("Вы уверены, что хотите отменить бронирование?")) return;

    const reason = prompt("Укажите причину отмены (необязательно):") || "Не указана";

    this.bookingService.cancelBooking(bookingId, reason).subscribe(
      () => {
        alert("Бронирование успешно отменено.");
        this.bookings = this.bookings.filter(b => b.id !== bookingId);
      },
      (error) => {
        console.error("Ошибка отмены бронирования:", error);
        alert("Ошибка при отмене бронирования. Попробуйте позже.");
      }
    );
  }
}
