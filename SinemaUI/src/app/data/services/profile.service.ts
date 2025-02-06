import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
http = inject(HttpClient)

  baseUrl:string = 'http://localhost:7000/users/';

getMyAccount(){
  return this.http.get(this.baseUrl+'me')
}
}
