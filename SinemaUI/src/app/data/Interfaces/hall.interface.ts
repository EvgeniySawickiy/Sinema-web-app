export interface Hall {
  id: string;
  name: string;
  totalSeats: number;
  seatLayoutJson: string;
}

export interface SeatRow {
  RowNumber: number;
  Seats: number[];
}
