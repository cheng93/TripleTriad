import { Action } from '@ngrx/store';
import { GameLobbyActions, GameLobbyActionTypes } from '../actions/game-lobby.actions';

export interface State {

}

export const initialState: State = {

};

export function reducer(state = initialState, action: GameLobbyActions): State {
  switch (action.type) {

    case GameLobbyActionTypes.LoadGameLobbys:
      return state;


    default:
      return state;
  }
}
