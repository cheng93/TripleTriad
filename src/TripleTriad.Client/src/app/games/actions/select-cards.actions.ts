import { Action } from '@ngrx/store';
import { Card } from '../models/card';

export enum SelectCardsActionTypes {
  ChangePage = '[SelectCards] Change Page',
  LoadAllCards = '[SelectCards] Load All Cards',
  LoadAllCardsSuccess = '[SelectCards] Load All Cards Success',
  LoadAllCardsFail = '[SelectCards] Load All Cards Fail',
  SelectCard = '[SelectCards] Select Card',
  RemoveCard = '[SelectCards] Remove Card'
}

export class ChangePage implements Action {
  readonly type = SelectCardsActionTypes.ChangePage;

  constructor(public payload: number) {}
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

export class SelectCard implements Action {
  readonly type = SelectCardsActionTypes.SelectCard;

  constructor(public payload: Card) {}
}

export class RemoveCard implements Action {
  readonly type = SelectCardsActionTypes.RemoveCard;

  constructor(public payload: Card) {}
}

export type SelectCardsActions =
  | ChangePage
  | LoadAllCards
  | LoadAllCardsSuccess
  | LoadAllCardsFail
  | SelectCard
  | RemoveCard;
