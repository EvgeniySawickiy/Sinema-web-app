import { Component } from '@angular/core';
import {StatisticsService} from '../../../data/services/statistics.service';
import Chart from 'chart.js/auto';


@Component({
  selector: 'app-admin-statistics',
  imports: [],
  templateUrl: './admin-statistics.component.html',
  styleUrl: './admin-statistics.component.scss'
})
export class AdminStatisticsComponent {
  statistics = {
    totalShowtimes: 0,
    totalMovies: 0,
    totalHalls: 0,
    totalBookings: 0,
    totalRevenue: 0,
    bookingsByDay: [] as { date: string; count: number }[]
  };

  constructor(private statisticsService: StatisticsService) {}

  ngOnInit(): void {
    this.loadStatistics();
  }

  ngAfterViewInit(): void {
    this.renderChart();
  }

  loadStatistics() {
    this.statisticsService.getStatistics().subscribe(
      (data) => {
        this.statistics = data;
        this.renderChart();
      },
      (error) => {
        console.error('Ошибка загрузки статистики:', error);
      }
    );
  }

  renderChart() {
    const ctx = document.getElementById('bookingsChart') as HTMLCanvasElement;
    if (!ctx || !this.statistics.bookingsByDay.length) return;

    new Chart(ctx, {
      type: 'line',
      data: {
        labels: this.statistics.bookingsByDay.map((b) => b.date),
        datasets: [
          {
            label: 'Бронирования',
            data: this.statistics.bookingsByDay.map((b) => b.count),
            borderColor: '#ff9800',
            fill: false,
            tension: 0.1
          }
        ]
      }
    });
  }
}
