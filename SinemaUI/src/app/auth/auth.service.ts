import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {TokenResponse} from '../data/Interfaces/auth.interface';
import {BehaviorSubject, catchError, Observable, tap, throwError} from 'rxjs';
import {CookieService} from 'ngx-cookie-service';
import {Router} from '@angular/router';
import {SignUpResponse} from '../data/Interfaces/sign-up-response';
import {User} from '../data/Interfaces/user.interface';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  http = inject(HttpClient);
  router = inject(Router);
  cookieService = inject(CookieService);

  baseUrl:string = 'http://localhost:7000/users/';


  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  accessToken: string | null = null;
  refreshToken: string | null = null;
  userId: string | null = null;

  private loadTokens(): void {
    this.accessToken = this.cookieService.get('access_token');
    this.refreshToken = this.cookieService.get('refresh_token');
    this.userId = this.cookieService.get('userId');
    this.isAuthenticatedSubject.next(!!this.accessToken);
  }

  get isAuthenticated(): boolean {
    if (!this.accessToken) {
    this.loadTokens()
    }
    return this.isAuthenticatedSubject.value;
  }

  getUserRole(): string | null {
    return this.cookieService.get('user_role') || null;
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }

  login(payload:{login: string, password: string}) {
  return this.http.post<TokenResponse>(`${this.baseUrl}login`,payload).pipe(
      tap(value => {
        this.saveTokens(value);
        console.log(value);
        this.isAuthenticatedSubject.next(true);
      })
    );
  }

  checkLoginAvailability(login: string): Observable<{ available: boolean }> {
    return this.http.get<{ available: boolean }>(`${this.baseUrl}check/login/${login}`);
  }

  register(payload: {
    login: string;
    password: string;
    email: string;
    phoneNumber: string;
    name: string;
    surname: string;
  }): Observable<any> {
    return this.http.post<SignUpResponse>(`${this.baseUrl}register`, payload);
  }

  sendConfirmationEmail(userId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}${userId}/send-confirmation-email`, {});
  }

  refreshAuthToken(){
    return this.http.post<TokenResponse>(`${this.baseUrl}token/refresh`, {
        accessToken: this.accessToken,
        refreshToken: this.refreshToken,
      }).pipe(
      tap(value => {
        this.saveTokens(value)
      }),
      catchError(err => {
        this.logout()
        return throwError(err)
      })
    );
  }

  requestPasswordReset(email: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}password-reset/request`, { email });
  }

  resetPassword(token: string, newPassword: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}password-reset`, { token, newPassword });
  }


  logout(){
    this.cookieService.delete('access_token');
    this.cookieService.delete('refresh_token');
    this.cookieService.delete('userId');
    this.cookieService.delete('user_role');

    this.accessToken = null;
    this.refreshToken = null;
    this.userId = null;
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/login']);
  }

  saveTokens(res:TokenResponse){
    this.accessToken = res.accessToken;
    this.refreshToken = res.refreshToken;
    this.userId = res.userId;

    this.cookieService.set('access_token', this.accessToken);
    this.cookieService.set('refresh_token', this.refreshToken);
    this.cookieService.set('userId', this.userId);

    try {
      const decodedToken: any = jwtDecode(this.accessToken);

      const userRole =
          decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || "User";

      this.cookieService.set('user_role', userRole);
    } catch (error) {
      console.error("Ошибка декодирования токена:", error);
    }

    this.isAuthenticatedSubject.next(true);
  }

  getUserId(){
    if (this.userId) {
      return this.userId;
    }
    else {
      return this.userId = this.cookieService.get('userId');
    }
  }

  getUserProfile(): Observable<User> {
    return this.http.get<User>(`${this.baseUrl}me`);
  }

  updateUserProfile(userData: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.baseUrl}me`, userData);
  }
}


