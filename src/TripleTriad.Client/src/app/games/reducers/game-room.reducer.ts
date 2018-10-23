import {
  GameRoomActions,
  GameRoomActionTypes
} from '../actions/game-room.actions';
import { Card } from '../models/card';
import { Tile } from '../models/tile';

export interface State {
  gameId: number;
  playerOneCards: Card[];
  playerTwoCards: Card[];
  playerOneTurn: boolean;
  status: string;
  tiles: Tile[];
}

export const initialState: State = {
  gameId: null,
  playerOneCards: [],
  playerTwoCards: [],
  playerOneTurn: null,
  status: null,
  tiles: []
};

export function reducer(state = initialState, action: GameRoomActions): State {
  switch (action.type) {
    case GameRoomActionTypes.ViewGame: {
      return {
        ...state,
        gameId: action.payload
      };
    }
    case GameRoomActionTypes.UpdateGame: {
      return {
        ...state,
        status: action.payload.status
      };
    }
    default:
      return state;
  }
}

export const getGameId = (state: State) => state.gameId;

export const getGameStatus = (state: State) => state.status;
