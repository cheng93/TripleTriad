import { LobbyActions, LobbyActionTypes } from '../actions/lobby.actions';
import {
  CoreActions,
  CoreActionTypes
} from 'src/app/core/actions/core.actions';

export interface State {
  creating: boolean;
  gameIds: number[];
  loading: boolean;
}

export const initialState: State = {
  creating: false,
  gameIds: [],
  loading: false
};

export function reducer(
  state = initialState,
  action: LobbyActions | CoreActions
): State {
  switch (action.type) {
    case LobbyActionTypes.CreateGame:
      return {
        ...state,
        creating: true
      };
    case LobbyActionTypes.CreateGameFail:
    case LobbyActionTypes.CreateGameSuccess:
      return {
        ...state,
        creating: false
      };
    case LobbyActionTypes.LoadGames:
      return {
        ...state,
        loading: true
      };
    case LobbyActionTypes.LoadGamesFail:
      return {
        ...state,
        loading: false
      };
    case LobbyActionTypes.LoadGamesSuccess:
      return {
        ...state,
        gameIds: action.gameIds,
        loading: false
      };
    case CoreActionTypes.ReceiveSignalRMessage:
      if (action.message.type == 'GameList') {
        return {
          ...state,
          gameIds: action.message.data
        };
      }

    default:
      return state;
  }
}

export const getGameIds = (state: State) => state.gameIds;
