import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface GameListResponse {
  gameIds: number[];
}

@Injectable()
export class GameLobbyService {
  constructor(private http: HttpClient) {}

  getGames(): Observable<GameListResponse> {
    return this.http.get<GameListResponse>('api/games');
  }
}
