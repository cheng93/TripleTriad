import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpHeaders
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export abstract class AuthenticationHttpInterceptor implements HttpInterceptor {
  abstract intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>>;

  constructor() {}

  addAuthenticationHeader(
    req: HttpRequest<any>,
    token: string
  ): HttpRequest<any> {
    var headers = {};
    for (const header of req.headers.keys()) {
      headers = {
        ...headers,
        [header]: req.headers.getAll(header)
      };
    }
    headers = {
      ...headers,
      Authorization: `Bearer ${token}`
    };

    var httpHeaders = new HttpHeaders(headers);

    return req.clone({ headers: httpHeaders });
  }
}
