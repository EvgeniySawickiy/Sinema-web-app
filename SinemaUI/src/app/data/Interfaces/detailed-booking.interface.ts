import {Seat} from './seat.interface';

export interface DetailedBooking {
  id: string;
  userId: string;
  movieTitle: string;
  movieImage: string;
  hallName: string;
  startTime: string;
  seats: Seat[];
  status : number;
  totalAmount: number;
}
