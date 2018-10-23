import { Injectable } from '@angular/core';
import { AuthenticationHttpInterceptor } from './authentication.httpinterceptor';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import {
  catchError,
  switchMap,
  finalize,
  filter,
  take,
  mergeMap
} from 'rxjs/operators';
import { TokenService } from './token.service';

@Injectable()
export class UnauthorizedInterceptorService extends AuthenticationHttpInterceptor {
  constructor(private tokenService: TokenService) {
    super();
  }

  private tokenSubject = new BehaviorSubject<string>(null);
  private isRefreshing = false;

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          if (!this.isRefreshing) {
            this.isRefreshing = true;
            this.tokenSubject.next(null);

            return this.tokenService.refreshToken().pipe(
              mergeMap(token => {
                this.tokenSubject.next(token);
                return next.handle(super.addAuthenticationHeader(req, token));
              }),
              finalize(() => {
                this.isRefreshing = false;
              })
            );
          } else {
            return this.tokenSubject.pipe(
              filter(token => token != null),
              take(1),
              switchMap(token =>
                next.handle(super.addAuthenticationHeader(req, token))
              )
            );
          }
        } else {
          return throwError(error);
        }
      })
    );
  }
}
