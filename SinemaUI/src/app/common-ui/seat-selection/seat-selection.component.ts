import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {DatePipe, NgClass, NgForOf, NgIf} from '@angular/common';
import {ShowTime} from '../../data/Interfaces/showtime.interface';
import {Seat} from '../../data/Interfaces/seat.interface';
import {Hall} from '../../data/Interfaces/hall.interface';
import {BookingService} from '../../data/services/booking.service';
import {AuthService} from '../../auth/auth.service';
import { v4 as uuidv4 } from 'uuid';
import {Router} from '@angular/router';

@Component({
  selector: 'app-seat-selection',
  imports: [
    NgForOf,
    DatePipe,
    NgIf,
    NgClass
  ],
  templateUrl: './seat-selection.component.html',
  styleUrl: './seat-selection.component.scss'
})
export class SeatSelectionComponent {
  @Input() session!: ShowTime;
  @Input() hall!: Hall;
  @Input() isOpen = false;
  @Output() close = new EventEmitter<void>();
  @Output() seatsSelected = new EventEmitter<Seat[]>();
  @Output() bookingConfirmed = new EventEmitter<string[]>();

  isBooking = false;
  seatRows: Seat[][] = [];
  selectedSeats: Seat[] = [];
  hoveredSeat: Seat | null = null;
  reservedSeats: Seat[] = [];
  tooltipX: number = 0;
  tooltipY: number = 0;
  showLoginModal = false;

  private bookingService = inject(BookingService);
  private authService = inject(AuthService);
  private router = inject(Router);

  ngOnInit() {
    this.loadSeatLayout();
    this.loadReservedSeats();
  }

  loadSeatLayout() {
    if (!this.hall?.seatLayoutJson) {
      console.error("Ошибка: `seatLayoutJson` пустой или не передан.");
      return;
    }

    try {
      const layout = JSON.parse(this.hall.seatLayoutJson);
      this.seatRows = layout.Rows.map((row: any) =>
        row.Seats.map((seatNumber: number) => ({
          id: uuidv4(),
          row: row.RowNumber,
          number: seatNumber,
          isReserved: false,
          hallId: this.hall.id,
          showTimeId: this.session.id
        }))
      );

    } catch (error) {
      console.error("Ошибка при разборе `seatLayoutJson`:", error);
    }
  }

  loadReservedSeats() {
    if (!this.session?.id) {
      console.error("Ошибка: ID сеанса отсутствует.");
      return;
    }

    this.bookingService.getReservedSeats(this.session.id).subscribe(
      (reservedSeats) => {
        this.reservedSeats = reservedSeats;

        this.seatRows.forEach(row => {
          row.forEach(seat => {
            if (this.isSeatReserved(seat.row, seat.number)) {
              seat.isReserved = true;
            }
          });
        });

        console.log("Занятые места загружены:", this.reservedSeats);
      },
      (error) => {
        console.error("Ошибка загрузки забронированных мест:", error);
      }
    );
  }

  isSeatReserved(row: number, number: number): boolean {
    return this.reservedSeats.some(seat => seat.row === row && seat.number === number);
  }

  toggleSeatSelection(seat: Seat) {
    if (seat.isReserved) return;

    const index = this.selectedSeats.findIndex(s => s.id === seat.id);
    if (index === -1) {
      if (this.selectedSeats.length < 5) {
        this.selectedSeats.push(seat);
      } else {
        alert("Вы можете выбрать не более 5 мест!");
      }
    } else {
      this.selectedSeats.splice(index, 1);
    }
  }

  isSelected(seat: Seat): boolean {
    return this.selectedSeats.some(s => s.id === seat.id);
  }

  confirmSelection() {
    const userId = this.authService.getUserId();
    console.log(userId);
    if (!userId) {
      this.showLoginModal = true;
      return;
    }
    this.isBooking = true;
    const bookingData = {
      userId,
      showtimeId: this.session.id,
      seats: this.selectedSeats,
      totalAmount: this.getTotalPrice()
    };

    this.bookingService.bookSeats(bookingData).subscribe(
      () => {
        alert("Бронирование успешно!");
        this.onClose();
      },
      (error) => {
        console.error("Ошибка бронирования:", error);
        alert("Ошибка бронирования. Попробуйте позже.");
      },
      ()=>{
        this.isBooking = false;
      }
    );
  }

  navigateToLogin() {
    this.showLoginModal = false;
    this.router.navigateByUrl('login');
  }

  getTotalPrice(): number {
    return this.selectedSeats.length * this.session.ticketPrice;
  }

  showSeatInfo(seat: Seat, event: MouseEvent) {
    if (!seat.isReserved) {
      this.hoveredSeat = seat;

      const modal = document.querySelector('.seat-selection-modal') as HTMLElement;

      if (modal) {
        const modalRect = modal.getBoundingClientRect();

        this.tooltipX = event.clientX - modalRect.left + 5;
        this.tooltipY = event.clientY - modalRect.top - 40;
      }
    }
  }

  getFormattedSelectedSeats(): string[] {
    const groupedSeats: { [row: number]: number[] } = {};

    this.selectedSeats.forEach((seat) => {
      if (!groupedSeats[seat.row]) {
        groupedSeats[seat.row] = [];
      }
      groupedSeats[seat.row].push(seat.number);
    });

    return Object.entries(groupedSeats).map(
      ([row, seats]) => `Ряд ${row}: Места ${seats.join(", ")}`
    );
  }

  hideSeatInfo() {
    this.hoveredSeat = null;
  }

  onClose() {
    this.selectedSeats = [];
    this.close.emit();
  }
}
