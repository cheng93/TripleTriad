import { Action } from '@ngrx/store';
import { Card } from '../models/card';

export enum SelectCardsActionTypes {
  LoadAllCards = '[SelectCards] Load All Cards',
  LoadAllCardsSuccess = '[SelectCards] Load All Cards Success',
  LoadAllCardsFail = '[SelectCards] Load All Cards Fail'
}

export class LoadAllCards implements Action {
  readonly type = SelectCardsActionTypes.LoadAllCards;
}

export class LoadAllCardsSuccess implements Action {
  readonly type = SelectCardsActionTypes.LoadAllCardsSuccess;

  constructor(public payload: Card[]) {}
}

export class LoadAllCardsFail implements Action {
  readonly type = SelectCardsActionTypes.LoadAllCardsFail;

  constructor(public payload: any) {}
}

export type SelectCardsActions =
  | LoadAllCards
  | LoadAllCardsSuccess
  | LoadAllCardsFail;
