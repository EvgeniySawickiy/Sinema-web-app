import {HttpHandlerFn, HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import {AuthService} from './auth.service';
import {inject} from '@angular/core';
import {catchError, switchMap, throwError} from 'rxjs';


let isRefreshing = false;
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService: AuthService = inject(AuthService);
  const isAuthenticated= inject(AuthService).isAuthenticated
  const token= inject(AuthService).accessToken

  if (!isAuthenticated) return next(req);

  if(isRefreshing){
    return refreshAndProceed(authService, req,next)
  }

  return next(addToken(req, <string>token))
    .pipe(
      catchError(error => {
        if(error.status === 401){
            return refreshAndProceed(authService, req,next)
        }
        return throwError(error)
      })
    )
}

const refreshAndProceed = (
  authService: AuthService,
  req:HttpRequest<any>,
  next: HttpHandlerFn
)=>{
  if(!isRefreshing){
    isRefreshing = true;

    return authService.refreshAuthToken()
      .pipe(
        switchMap(res => {
          isRefreshing = false;
          return next(addToken(req, res.accessToken))
        })
      )
  }

  return next(addToken(req, authService.accessToken!))
}

const addToken = (req:HttpRequest<any>, token:string) => {
  return  req.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`},
  })
}


