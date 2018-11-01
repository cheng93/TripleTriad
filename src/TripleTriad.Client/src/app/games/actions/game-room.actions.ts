import { Action } from '@ngrx/store';
import { Room, View } from '../models/room';

export enum GameRoomActionTypes {
  ViewGame = '[GameRoom] View Game',
  ViewGameSuccess = '[GameRoom] View Game Success',
  ViewGameFail = '[GameRoom] View Game Fail',
  UpdateGame = '[GameRoom] Update Game'
}

export class ViewGame implements Action {
  readonly type = GameRoomActionTypes.ViewGame;

  constructor(public payload: number) {}
}

export class ViewGameSuccess implements Action {
  readonly type = GameRoomActionTypes.ViewGameSuccess;

  constructor(public payload: View) {}
}

export class ViewGameFail implements Action {
  readonly type = GameRoomActionTypes.ViewGameFail;

  constructor(public payload: any) {}
}

export class UpdateGame implements Action {
  readonly type = GameRoomActionTypes.UpdateGame;

  constructor(public payload: Room) {}
}

export type GameRoomActions =
  | ViewGame
  | ViewGameSuccess
  | ViewGameFail
  | UpdateGame;
