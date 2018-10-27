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
    case SelectCardsActionTypes.ChangePage: {
      return {
        ...state,
        cardPage: action.payload
      };
    }
    case SelectCardsActionTypes.LoadAllCardsSuccess: {
      return {
        ...state,
        allCards: action.payload
      };
    }
    case SelectCardsActionTypes.SelectCard: {
      return {
        ...state,
        selectedCards: [...state.selectedCards, action.payload]
      };
    }
    case SelectCardsActionTypes.RemoveCard: {
      return {
        ...state,
        selectedCards: state.selectedCards.filter(
          x => x.name !== action.payload.name
        )
      };
    }
    default:
      return state;
  }
}

export const getAllCards = (state: State) => state.allCards;

export const getAllCardsLoaded = (state: State) => state.allCards.length != 0;

export const getLevelCards = (state: State) =>
  state.allCards.filter(x => x.level === state.cardPage + 1);

export const getSelectedCards = (state: State) => state.selectedCards;

export const getCardPage = (state: State) => state.cardPage;
