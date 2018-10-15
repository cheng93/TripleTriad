import { Action } from '@ngrx/store';
import {
  GameLobbyActions,
  GameLobbyActionTypes
} from '../actions/game-lobby.actions';

export interface State {
  gameIds: number[];
  loading: boolean;
  loaded: boolean;
  creating: boolean;
  created: boolean;
}

export const initialState: State = {
  gameIds: [],
  loading: false,
  loaded: false,
  creating: false,
  created: false
};

export function reducer(state = initialState, action: GameLobbyActions): State {
  switch (action.type) {
    case GameLobbyActionTypes.CreateGame:
      return {
        ...state,
        creating: true
      };
    case GameLobbyActionTypes.CreateGameSuccess:
      return {
        ...state,
        creating: false,
        created: true
      };
    case GameLobbyActionTypes.LoadGames:
      return {
        ...state,
        loading: true
      };
    case GameLobbyActionTypes.LoadGamesSuccess:
      return {
        ...state,
        gameIds: <number[]>action.payload,
        loading: false,
        loaded: true
      };

    default:
      return state;
  }
}

export const getGameIds = (state: State) => state.gameIds;
