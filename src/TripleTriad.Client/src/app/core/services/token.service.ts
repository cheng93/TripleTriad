import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { of, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';

const ACCESS_TOKEN_KEY = 'tripletriad_access_token';

@Injectable()
export class TokenService {
  constructor(private http: HttpClient) {}

  getAccessToken(): Observable<string> {
    var accessToken = localStorage.getItem(ACCESS_TOKEN_KEY);
    if (accessToken) {
      return of(accessToken);
    }

    return this.http.post<TokenResponse>('api/token/generate', {}).pipe(
      map(response => response.token),
      tap(token => {
        localStorage.setItem(ACCESS_TOKEN_KEY, token);
      })
    );
  }

  refreshToken(): Observable<string> {
    // TODO: Use a refresh token instead of generating a new one.
    return this.http.post<TokenResponse>('api/token/generate', {}).pipe(
      map(response => response.token),
      tap(token => {
        localStorage.setItem(ACCESS_TOKEN_KEY, token);
      })
    );
  }
}

interface TokenResponse {
  token: string;
}
