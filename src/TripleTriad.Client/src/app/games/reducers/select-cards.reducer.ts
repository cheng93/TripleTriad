import { Action } from '@ngrx/store';
import { Card } from '../models/card';

export interface State {
  allCards: Card[];
  selectedCards: Card[];
  cardPage: number;
}

export const initialState: State = {
  allCards: [],
  selectedCards: [],
  cardPage: 0
};

export function reducer(state = initialState, action: Action): State {
  switch (action.type) {
    default:
      return state;
  }
}

export const getAllCards = (state: State) => state.allCards;

export const getSelectedCards = (state: State) => state.selectedCards;

export const getCardPage = (state: State) => state.cardPage;
