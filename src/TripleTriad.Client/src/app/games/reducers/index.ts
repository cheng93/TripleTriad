import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector
} from '@ngrx/store';

import * as fromLobby from './game-lobby.reducer';

export interface GameState {
  lobby: fromLobby.State;
}

export const reducers: ActionReducerMap<GameState> = {
  lobby: fromLobby.reducer
};
