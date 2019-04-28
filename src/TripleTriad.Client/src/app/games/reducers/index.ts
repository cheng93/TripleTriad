import {
  ActionReducerMap,
  createFeatureSelector,
  createSelector
} from '@ngrx/store';
import * as fromRoot from '../../reducers';
import * as fromRoom from './room.reducer';
import * as fromSelectCards from './select-cards.reducer';

export interface GamesState {
  room: fromRoom.State;
  selectCards: fromSelectCards.State;
}

export interface State extends fromRoot.State {
  game: GamesState;
}

export const reducers: ActionReducerMap<GamesState> = {
  room: fromRoom.reducer,
  selectCards: fromSelectCards.reducer
};

export const getGameState = createFeatureSelector<GamesState>('games');

export const getGameRoomState = createSelector(
  getGameState,
  state => state.room
);

export const getRoomGameId = createSelector(
  getGameRoomState,
  fromRoom.getGameId
);

export const getRoomStatus = createSelector(
  getGameRoomState,
  fromRoom.getStatus
);

export const getRoomTiles = createSelector(
  getGameRoomState,
  fromRoom.getTiles
);

export const getSelectCardsState = createSelector(
  getGameState,
  state => state.selectCards
);

export const getAllCards = createSelector(
  getSelectCardsState,
  fromSelectCards.getAllCards
);

export const getAllCardsLoaded = createSelector(
  getSelectCardsState,
  fromSelectCards.getAllCardsLoaded
);

export const getLevelCards = createSelector(
  getSelectCardsState,
  fromSelectCards.getLevelCards
);

export const getSelectedCards = createSelector(
  getSelectCardsState,
  fromSelectCards.getSelectedCards
);

export const getSelectCardPage = createSelector(
  getSelectCardsState,
  fromSelectCards.getCardPage
);

export const hasSubmittedCards = createSelector(
  getSelectCardsState,
  fromSelectCards.hasSubmittedCards
);

export const showSelectCardSubmit = createSelector(
  getSelectCardsState,
  fromSelectCards.showSubmit
);
