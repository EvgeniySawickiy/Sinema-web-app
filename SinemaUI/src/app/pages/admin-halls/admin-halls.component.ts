import { Component } from '@angular/core';
import {FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {NgForOf, NgIf} from '@angular/common';
import {Hall} from '../../data/Interfaces/hall.interface';
import {HallService} from '../../data/services/hall.service';


@Component({
  selector: 'app-admin-halls',
  imports: [
    ReactiveFormsModule,
    NgIf,
    NgForOf
  ],
  templateUrl: './admin-halls.component.html',
  styleUrl: './admin-halls.component.scss'
})
export class AdminHallsComponent {
  hallForm: FormGroup;
  halls: Hall[] = [];
  isEditing = false;
  editingHallId: string | null = null;

  constructor(private fb: FormBuilder, private hallService: HallService) {
    this.hallForm = this.fb.group({
      name: ['', Validators.required],
      totalSeats: [0, [Validators.required, Validators.min(1)]],
      numberOfRows: [0, [Validators.required, Validators.min(1)]],
      seatsPerRow: this.fb.array([]), // Массив контролов для мест в ряду
    });

    this.loadHalls();
  }

  get seatsPerRow(): FormArray {
    return this.hallForm.get('seatsPerRow') as FormArray;
  }

  loadHalls() {
    this.hallService.getHalls().subscribe(
      (halls) => (this.halls = halls),
      (error) => console.error('Ошибка загрузки залов:', error)
    );
  }

  updateSeatsPerRow() {
    const numberOfRows = this.hallForm.get('numberOfRows')?.value || 0;
    const totalSeats = this.hallForm.get('totalSeats')?.value || 0;

    this.seatsPerRow.clear(); // Очистка перед обновлением

    for (let i = 0; i < numberOfRows; i++) {
      const seatsInRow = Math.floor(totalSeats / numberOfRows);
      this.seatsPerRow.push(this.fb.control(seatsInRow, [Validators.required, Validators.min(1)]));
    }
  }

  addHall() {
    if (this.hallForm.invalid) return;

    const formData = this.hallForm.value;
    const seatLayout = this.generateSeatLayout(formData.numberOfRows, formData.seatsPerRow);

    const hallData: Partial<Hall> = {
      id:"",
      name: formData.name,
      totalSeats: formData.totalSeats,
      numberOfRows: formData.numberOfRows,
      seatsPerRow: formData.seatsPerRow,
      seatLayoutJson: JSON.stringify(seatLayout),
    };

    this.hallService.addHall(hallData).subscribe(() => {
      this.loadHalls();
      this.hallForm.reset();
      this.seatsPerRow.clear();
    });
  }

  editHall(hall: Hall) {
    this.isEditing = true;
    this.editingHallId = hall.id;

    const parsedLayout = JSON.parse(hall.seatLayoutJson);
    const seatsPerRowArray = parsedLayout.Rows.map((row: any) => row.Seats.length);

    this.hallForm.patchValue({
      name: hall.name,
      totalSeats: hall.totalSeats,
      numberOfRows: seatsPerRowArray.length,
    });

    this.seatsPerRow.clear();
    seatsPerRowArray.forEach((seats: any) => {
      this.seatsPerRow.push(this.fb.control(seats, [Validators.required, Validators.min(1)]));
    });
  }

  updateHall() {
    if (this.hallForm.invalid || !this.editingHallId) return;

    const formData = this.hallForm.value;
    const seatLayout = this.generateSeatLayout(formData.numberOfRows, formData.seatsPerRow);

    const updatedHall: Hall = {
      id: this.editingHallId,
      name: formData.name,
      totalSeats: formData.totalSeats,
      numberOfRows: formData.numberOfRows,
      seatsPerRow: formData.seatsPerRow,
      seatLayoutJson: JSON.stringify(seatLayout),
    };

    this.hallService.updateHall(this.editingHallId, updatedHall).subscribe(() => {
      this.isEditing = false;
      this.editingHallId = null;
      this.loadHalls();
      this.hallForm.reset();
      this.seatsPerRow.clear();
    });
  }

  cancelEdit() {
    this.isEditing = false;
    this.editingHallId = null;
    this.hallForm.reset();
    this.seatsPerRow.clear();
  }

  deleteHall(id: string) {
    this.hallService.deleteHall(id).subscribe(() => {
      this.loadHalls();
    });
  }

  generateSeatLayout(rows: number, seatsPerRow: number[]): { Rows: { RowNumber: number; Seats: number[] }[] } {
    return {
      Rows: Array.from({ length: rows }, (_, index) => ({
        RowNumber: index + 1,
        Seats: Array.from({ length: seatsPerRow[index] }, (_, seatIndex) => seatIndex + 1),
      })),
    };
  }

  getNumberOfRows(hall: Hall): number {
    try {
      return JSON.parse(hall.seatLayoutJson)?.Rows.length || 0;
    } catch {
      return 0;
    }
  }

  getSeatsPerRow(hall: Hall): string {
    try {
      return JSON.parse(hall.seatLayoutJson)?.Rows.map((row: any) => row.Seats.length).join(', ') || 'Неизвестно';
    } catch {
      return 'Неизвестно';
    }
  }
}
