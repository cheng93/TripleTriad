import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class GameRoomService {
  constructor(private http: HttpClient) {}

  viewGame(gameId: number): Observable<object> {
    return this.http.put(`api/games/${gameId}/view`, {});
  }
}
