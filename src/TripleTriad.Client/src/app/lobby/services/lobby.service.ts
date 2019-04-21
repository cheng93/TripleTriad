import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class LobbyService {
  constructor(private http: HttpClient) {}

  createGame(): Observable<CreateGameResponse> {
    return this.http.post<CreateGameResponse>('api/games', {});
  }

  getGames(): Observable<GameListResponse> {
    return this.http.get<GameListResponse>('api/games');
  }

  joinGame(gameId: number): Observable<JoinGameResponse> {
    return this.http.put<JoinGameResponse>(`api/games/${gameId}/join`, {});
  }
}

export interface GameListResponse {
  gameIds: number[];
}

export interface CreateGameResponse {
  gameId: number;
}

export interface JoinGameResponse {
  gameId: number;
}
