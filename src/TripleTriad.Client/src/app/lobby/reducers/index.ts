import * as fromLobby from './lobby.reducer';
import * as fromRoot from '../../reducers';
import { createFeatureSelector, createSelector } from '@ngrx/store';

export const reducer = fromLobby.reducer;

export interface State extends fromRoot.State {
  lobby: fromLobby.State;
}

export const getLobbyState = createFeatureSelector<State, fromLobby.State>(
  'lobby'
);

export const getGameIds = createSelector(
  getLobbyState,
  fromLobby.getGameIds
);
