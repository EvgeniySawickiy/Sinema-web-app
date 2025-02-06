export interface Hall {
  id: string;
  name: string;
  totalSeats: number;
  seatLayoutJson: string;
  numberOfRows: number;
  seatsPerRow: number[];
}

export interface SeatRow {
  RowNumber: number;
  Seats: number[];
}
