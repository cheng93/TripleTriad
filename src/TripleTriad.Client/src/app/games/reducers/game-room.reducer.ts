import {
  CoreActions,
  CoreActionTypes
} from 'src/app/core/actions/core.actions';
import {
  GameRoomActions,
  GameRoomActionTypes
} from '../actions/game-room.actions';
import { Card } from '../models/card';
import { View } from '../models/room';
import { Tile } from '../models/tile';

export interface State {
  gameId: number;
  hostCards: Card[];
  challengerCards: Card[];
  hostTurn: boolean;
  status: string;
  tiles: Tile[];
  view: View;
}

export const initialState: State = {
  gameId: null,
  hostCards: [],
  challengerCards: [],
  hostTurn: null,
  status: null,
  tiles: [],
  view: {
    isHost: false,
    isChallenger: false
  }
};

export function reducer(
  state = initialState,
  action: GameRoomActions | CoreActions
): State {
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
    case CoreActionTypes.ReceiveSignalRMessage: {
      if (action.message.type == 'GameState') {
        return {
          ...state,
          ...action.message.data
        };
      }
    }
    default:
      return state;
  }
}

export const getGameId = (state: State) => state.gameId;

export const getGameStatus = (state: State) => state.status;

export const getGameTiles = (state: State) => state.tiles;
