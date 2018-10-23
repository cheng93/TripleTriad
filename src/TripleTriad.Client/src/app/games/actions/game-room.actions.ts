import { Action } from '@ngrx/store';
import { Room } from '../models/room';

export enum GameRoomActionTypes {
  ViewGame = '[GameRoom] View Game',
  UpdateGame = '[GameRoom] Update Game'
}

export class ViewGame implements Action {
  readonly type = GameRoomActionTypes.ViewGame;

  constructor(public payload: number) {}
}

export class UpdateGame implements Action {
  readonly type = GameRoomActionTypes.UpdateGame;

  constructor(public payload: Room) {}
}

export type GameRoomActions = ViewGame | UpdateGame;
