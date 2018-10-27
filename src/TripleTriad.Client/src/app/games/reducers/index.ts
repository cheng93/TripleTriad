import {
  ActionReducerMap,
  createFeatureSelector,
  createSelector
} from '@ngrx/store';

import * as fromLobby from './game-lobby.reducer';
import * as fromRoot from '../../reducers';
import * as fromRoom from './game-room.reducer';
import * as fromSelectCards from './select-cards.reducer';

export interface GamesState {
  lobby: fromLobby.State;
  room: fromRoom.State;
  selectCards: fromSelectCards.State;
}

export interface State extends fromRoot.State {
  game: GamesState;
}

export const reducers: ActionReducerMap<GamesState> = {
  lobby: fromLobby.reducer,
  room: fromRoom.reducer,
  selectCards: fromSelectCards.reducer
};

export const getGameState = createFeatureSelector<GamesState>('games');

export const getGameLobbyState = createSelector(
  getGameState,
  state => state.lobby
);

export const getLobbyGameIds = createSelector(
  getGameLobbyState,
  fromLobby.getGameIds
);

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
  fromRoom.getGameStatus
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
