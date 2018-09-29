import { Action } from '@ngrx/store';
import {
  GameLobbyActions,
  GameLobbyActionTypes
} from '../actions/game-lobby.actions';

export interface State {
  gameIds: number[];
  loading: boolean;
  loaded: boolean;
}

export const initialState: State = {
  gameIds: [],
  loading: false,
  loaded: false
};

export function reducer(state = initialState, action: GameLobbyActions): State {
  switch (action.type) {
    case GameLobbyActionTypes.LoadGames:
      return {
        ...state,
        loading: true
      };
    case GameLobbyActionTypes.LoadGamesSuccess:
      return {
        gameIds: action.payload,
        loading: false,
        loaded: true
      };

    default:
      return state;
  }
}

export const getGameIds = (state: State) => state.gameIds;
