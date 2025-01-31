import {Seat} from './seat.interface';

export interface Booking {
  id: string;
  userId: string;
  showtimeId: string;
  status : number;
  seats: Seat[];
  totalAmount: number;
}

export interface BookingRequest {
  userId: string;
  showtimeId: string;
  seats: Seat[];
  totalAmount: number;
}
