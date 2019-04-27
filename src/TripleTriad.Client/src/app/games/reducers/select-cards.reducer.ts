import {
  CoreActions,
  CoreActionTypes
} from 'src/app/core/actions/core.actions';
import {
  SelectCardsActions,
  SelectCardsActionTypes
} from '../actions/select-cards.actions';
import { Card } from '../models/card';

export interface State {
  allCards: Card[];
  selectedCards: string[];
  cardPage: number;
  cardsSubmitted: boolean;
}

export const initialState: State = {
  allCards: [],
  selectedCards: [],
  cardPage: 0,
  cardsSubmitted: false
};

export function reducer(
  state = initialState,
  action: SelectCardsActions | CoreActions
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
        selectedCards: state.selectedCards.filter(x => x !== action.payload)
      };
    }
    case SelectCardsActionTypes.SubmitCardsSuccess: {
      return {
        ...state,
        cardsSubmitted: true
      };
    }
    case CoreActionTypes.ReceiveSignalRMessage: {
      if (
        action.message.type == 'GameState' &&
        action.message.data.status === 'ChooseCards'
      ) {
        return {
          ...state,
          selectedCards: action.message.data.selectedCards,
          cardsSubmitted: action.message.data.selectedCards.length > 0
        };
      }
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

export const hasSubmittedCards = (state: State) => state.cardsSubmitted;

export const showSubmit = (state: State) =>
  !state.cardsSubmitted && state.selectedCards.length === 5;
