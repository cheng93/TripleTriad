import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { View } from '../models/room';

@Injectable()
export class GameRoomService {
  constructor(private http: HttpClient) {}

  viewGame(gameId: number): Observable<View> {
    return this.http.put<View>(`api/games/${gameId}/view`, {});
  }
}
