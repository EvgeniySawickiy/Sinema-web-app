import { Component } from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-confirm-email',
  imports: [],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.scss'
})
export class ConfirmEmailComponent {
  message: string = 'Подтверждение email...';

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit(): void {
    const token = this.route.snapshot.queryParamMap.get('token');
    if (token) {
      console.log(token)
      this.http
        .get(`http://localhost:7000/users/confirm-email?token=${token}`)
        .subscribe(
          () => (this.message = 'Ваш email успешно подтвержден!'),
          () => (this.message = 'Ошибка подтверждения email.')
        );
    } else {
      this.message = 'Отсутствует токен для подтверждения.';
    }
  }
}
