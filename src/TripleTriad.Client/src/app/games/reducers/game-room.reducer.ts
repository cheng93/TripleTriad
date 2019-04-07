import {
  GameRoomActions,
  GameRoomActionTypes
} from '../actions/game-room.actions';
import { Card } from '../models/card';
import { Tile } from '../models/tile';
import { View } from '../models/room';

export interface State {
  gameId: number;
  hostCards: Card[];
  playerTwoCards: Card[];
  hostTurn: boolean;
  status: string;
  tiles: Tile[];
  view: View;
}

export const initialState: State = {
  gameId: null,
  hostCards: [],
  playerTwoCards: [],
  hostTurn: null,
  status: null,
  tiles: [],
  view: {
    isHost: false,
    isPlayerTwo: false
  }
};

export function reducer(state = initialState, action: GameRoomActions): State {
  switch (action.type) {
    case GameRoomActionTypes.ViewGame: {
      return {
        ...state,
        gameId: action.payload
      };
    }
    case GameRoomActionTypes.ViewGameSuccess: {
      return {
        ...state,
        view: action.payload
      };
    }
    case GameRoomActionTypes.UpdateGame: {
      return {
        ...state,
        status: action.payload.status,
        tiles: action.payload.tiles
      };
    }
    default:
      return state;
  }
}

export const getGameId = (state: State) => state.gameId;

export const getGameStatus = (state: State) => state.status;

export const getGameTiles = (state: State) => state.tiles;
