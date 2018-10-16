import { Action } from '@ngrx/store';
import { Card } from '../models/card';
import { Tile } from '../models/tile';

export interface State {
  playerOneCards: Card[];
  playerTwoCards: Card[];
  playerOneTurn: boolean;
  tiles: Tile[];
}

export const initialState: State = {
  playerOneCards: [],
  playerTwoCards: [],
  playerOneTurn: null,
  tiles: []
};

export function reducer(state = initialState, action: Action): State {
  switch (action.type) {
    default:
      return state;
  }
}
