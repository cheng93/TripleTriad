import { Action } from '@ngrx/store';

export enum LobbyActionTypes {
  CreateGame = '[Lobby] Create Game',
  CreateGameSuccess = '[Lobby] Create Game Success',
  JoinGame = '[Lobby] Join Game',
  LoadGames = '[Lobby] Load Games',
  LoadGamesSuccess = '[Lobby] Load Games Success'
}

export class CreateGame implements Action {
  readonly type = LobbyActionTypes.CreateGame;
}

export class CreateGameSuccess implements Action {
  readonly type = LobbyActionTypes.CreateGameSuccess;

  constructor(public gameId: number) {}
}

export class JoinGame implements Action {
  readonly type = LobbyActionTypes.JoinGame;

  constructor(public gameId: number) {}
}

export class LoadGames implements Action {
  readonly type = LobbyActionTypes.LoadGames;
}

export class LoadGamesSuccess implements Action {
  readonly type = LobbyActionTypes.LoadGamesSuccess;

  constructor(public gameIds: number[]) {}
}

export type LobbyActions =
  | CreateGame
  | CreateGameSuccess
  | LoadGames
  | LoadGamesSuccess;
