import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Card } from '../models/card';

interface GetAllCardsResponse {
  cards: Card[];
}

@Injectable()
export class SelectCardsService {
  constructor(private http: HttpClient) {}

  getAllCards(): Observable<GetAllCardsResponse> {
    return this.http.get<GetAllCardsResponse>(`api/cards`);
  }

  submitCards(gameId: number, cards: string[]) {
    return this.http.put(`api/games/${gameId}/selectCards`, cards);
  }
}
