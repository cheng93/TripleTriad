import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface GameListResponse {
  gameIds: number[];
}

export interface CreateGameResponse {
  gameId: number;
}

export interface JoinGameResponse {
  gameId: number;
}

@Injectable()
export class GameLobbyService {
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
