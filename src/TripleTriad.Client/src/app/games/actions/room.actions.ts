import { Action } from '@ngrx/store';
import { Room, View } from '../models/room';

export enum RoomActionTypes {
  ViewGame = '[Room] View Game',
  ViewGameSuccess = '[Room] View Game Success',
  ViewGameFail = '[Room] View Game Fail',
  UpdateGame = '[Room] Update Game'
}

export class ViewGame implements Action {
  readonly type = RoomActionTypes.ViewGame;

  constructor(public payload: number) {}
}

export class ViewGameSuccess implements Action {
  readonly type = RoomActionTypes.ViewGameSuccess;

  constructor(public payload: View) {}
}

export class ViewGameFail implements Action {
  readonly type = RoomActionTypes.ViewGameFail;

  constructor(public payload: any) {}
}

export class UpdateGame implements Action {
  readonly type = RoomActionTypes.UpdateGame;

  constructor(public payload: Room) {}
}

export type RoomActions =
  | ViewGame
  | ViewGameSuccess
  | ViewGameFail
  | UpdateGame;
