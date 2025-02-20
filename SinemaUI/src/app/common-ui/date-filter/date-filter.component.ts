import { Component , EventEmitter, Output} from '@angular/core';
import { format, addDays } from 'date-fns';
import {NgForOf, NgIf} from '@angular/common';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatDatepickerInput, MatDatepickerModule} from '@angular/material/datepicker';
import {MatInputModule} from '@angular/material/input';
import {MatNativeDateModule} from '@angular/material/core';

@Component({
  selector: 'app-date-filter',
  imports: [
    NgForOf,
    NgIf,
    MatNativeDateModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatFormFieldModule,
  ],
  templateUrl: './date-filter.component.html',
  styleUrl: './date-filter.component.scss'
})
export class DateFilterComponent {
  dates: { day: string; month: string; weekday: string }[] = [];
  selectedDateIndex: number = 0;
  calendarVisible: boolean = false;
  selectedDate: Date = new Date();
  minDate: Date = new Date();

  @Output() dateSelected = new EventEmitter<Date>();

  constructor() {
    this.generateDates();
  }

  generateDates(): void {
    const today = new Date();
    for (let i = 0; i < 7; i++) {
      const date = addDays(today, i);
      this.dates.push({
        day: format(date, 'dd'),
        month: format(date, 'MMMM'),
        weekday: format(date, 'EEEE'),
      });
    }
  }

  selectDate(index: number): void {
    this.selectedDateIndex = index;
    const date = addDays(new Date(), index);
    this.selectedDate = date;
    this.dateSelected.emit(date);
  }

  openCalendar(): void {
    this.calendarVisible = true;
  }

  closeCalendar(): void {
    this.calendarVisible = false;
  }

  onDateChange(selectedDate: Date | null): void {
    if (selectedDate) {
      this.selectedDate = selectedDate;
      this.dateSelected.emit(selectedDate);
    }
  }

}
