import { Action } from '@ngrx/store';

export enum GameLobbyActionTypes {
  LoadGameLobbys = '[GameLobby] Load GameLobbys'
}

export class LoadGameLobbys implements Action {
  readonly type = GameLobbyActionTypes.LoadGameLobbys;
}

export type GameLobbyActions = LoadGameLobbys;
