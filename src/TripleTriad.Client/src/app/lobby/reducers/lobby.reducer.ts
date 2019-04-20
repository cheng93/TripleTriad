
import { LobbyActions, LobbyActionTypes } from '../actions/lobby.actions';

export interface State {

}

export const initialState: State = {

};

export function reducer(state = initialState, action: LobbyActions): State {
  switch (action.type) {

    case LobbyActionTypes.LoadLobbys:
      return state;

    default:
      return state;
  }
}
