import { Card } from '../models/card';
import {
  SelectCardsActionTypes,
  SelectCardsActions
} from '../actions/select-cards.actions';

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

export function reducer(
  state = initialState,
  action: SelectCardsActions
): State {
  switch (action.type) {
    case SelectCardsActionTypes.LoadAllCardsSuccess: {
      return {
        ...state,
        allCards: action.payload
      };
    }
    default:
      return state;
  }
}

export const getAllCards = (state: State) => state.allCards;

export const getSelectedCards = (state: State) => state.selectedCards;

export const getCardPage = (state: State) => state.cardPage;