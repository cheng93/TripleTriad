import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Card } from '../models/card';
import { Observable } from 'rxjs';

interface GetAllCardsResponse {
  cards: Card[];
}

@Injectable()
export class SelectCardsService {
  constructor(private http: HttpClient) {}

  getAllCards(): Observable<GetAllCardsResponse> {
    return this.http.get<GetAllCardsResponse>(`api/cards`);
  }

  submitCards(gameId: number, cards: Card[]) {
    var body = cards.map(x => x.name);
    return this.http.put(`api/games/${gameId}/selectCards`, body);
  }
}
