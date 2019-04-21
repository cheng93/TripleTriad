import { LobbyActions, LobbyActionTypes } from '../actions/lobby.actions';

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

export function reducer(state = initialState, action: LobbyActions): State {
  switch (action.type) {
    case LobbyActionTypes.CreateGame:
      return {
        ...state,
        creating: true
      };
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
    case LobbyActionTypes.LoadGamesSuccess:
      return {
        ...state,
        gameIds: action.gameIds,
        loading: false
      };

    default:
      return state;
  }
}

export const getGameIds = (state: State) => state.gameIds;
