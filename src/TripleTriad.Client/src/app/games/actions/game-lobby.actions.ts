import { Action } from '@ngrx/store';

export enum GameLobbyActionTypes {
  LoadGames = '[GameLobby] Load Games',
  LoadGamesSuccess = '[GameLobby] Load Games Success',
  LoadGamesFail = '[GameLobby] Load Games Fail'
}

export class LoadGames implements Action {
  readonly type = GameLobbyActionTypes.LoadGames;
}

export class LoadGamesSuccess implements Action {
  readonly type = GameLobbyActionTypes.LoadGamesSuccess;

  constructor(public payload: number[]) {}
}

export class LoadGamesFail implements Action {
  readonly type = GameLobbyActionTypes.LoadGamesFail;

  constructor(public payload: any) {}
}

export type GameLobbyActions = LoadGames | LoadGamesSuccess | LoadGamesFail;
