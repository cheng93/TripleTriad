import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector
} from '@ngrx/store';

import * as fromLobby from './game-lobby.reducer';
import * as fromRoot from '../../reducers';

export interface GamesState {
  lobby: fromLobby.State;
}

export interface State extends fromRoot.State {
  game: GamesState;
}

export const reducers: ActionReducerMap<GamesState> = {
  lobby: fromLobby.reducer
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
