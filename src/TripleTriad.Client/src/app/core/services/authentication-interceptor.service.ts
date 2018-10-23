import { Injectable } from '@angular/core';
import { AuthenticationHttpInterceptor } from './authentication.httpinterceptor';
import { HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TokenService } from './token.service';
import { switchMap } from 'rxjs/operators';

@Injectable()
export class AuthenticationInterceptorService extends AuthenticationHttpInterceptor {
  constructor(private tokenService: TokenService) {
    super();
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (req.url == 'api/token/generate') {
      return next.handle(req);
    }
    return this.tokenService
      .getAccessToken()
      .pipe(
        switchMap(token =>
          next.handle(super.addAuthenticationHeader(req, token))
        )
      );
  }
}
