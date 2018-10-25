import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Card } from '../models/card';
import { Observable } from 'rxjs';

interface GetAllCardsResponse {
  Cards: Card[];
}

@Injectable()
export class SelectCardsService {
  constructor(private http: HttpClient) {}

  getAllCards(): Observable<GetAllCardsResponse> {
    return this.http.get<GetAllCardsResponse>(`api/cards`);
  }
}
