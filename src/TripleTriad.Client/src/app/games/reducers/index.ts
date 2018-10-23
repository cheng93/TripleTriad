import {
  ActionReducerMap,
  createFeatureSelector,
  createSelector
} from '@ngrx/store';

import * as fromLobby from './game-lobby.reducer';
import * as fromRoot from '../../reducers';
import * as fromRoom from './game-room.reducer';

export interface GamesState {
  lobby: fromLobby.State;
  room: fromRoom.State;
}

export interface State extends fromRoot.State {
  game: GamesState;
}

export const reducers: ActionReducerMap<GamesState> = {
  lobby: fromLobby.reducer,
  room: fromRoom.reducer
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
