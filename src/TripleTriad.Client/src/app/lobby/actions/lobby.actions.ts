import { Action } from '@ngrx/store';

export enum LobbyActionTypes {
  CreateGame = '[Lobby] Create Game',
  CreateGameFail = '[Lobby] Create Game Fail',
  CreateGameSuccess = '[Lobby] Create Game Success',
  JoinGame = '[Lobby] Join Game',
  JoinGameFail = '[Lobby] Join Game Fail',
  LoadGames = '[Lobby] Load Games',
  LoadGamesFail = '[Lobby] Load Games Fail',
  LoadGamesSuccess = '[Lobby] Load Games Success'
}

export class CreateGame implements Action {
  readonly type = LobbyActionTypes.CreateGame;
}

export class CreateGameFail implements Action {
  readonly type = LobbyActionTypes.CreateGameFail;

  constructor(public error: any) {}
}

export class CreateGameSuccess implements Action {
  readonly type = LobbyActionTypes.CreateGameSuccess;

  constructor(public gameId: number) {}
}

export class JoinGame implements Action {
  readonly type = LobbyActionTypes.JoinGame;

  constructor(public gameId: number) {}
}

export class JoinGameFail implements Action {
  readonly type = LobbyActionTypes.JoinGameFail;

  constructor(public error: any) {}
}

export class LoadGames implements Action {
  readonly type = LobbyActionTypes.LoadGames;
}

export class LoadGamesFail implements Action {
  readonly type = LobbyActionTypes.LoadGamesFail;

  constructor(public error: any) {}
}

export class LoadGamesSuccess implements Action {
  readonly type = LobbyActionTypes.LoadGamesSuccess;

  constructor(public gameIds: number[]) {}
}

export type LobbyActions =
  | CreateGame
  | CreateGameFail
  | CreateGameSuccess
  | JoinGame
  | JoinGameFail
  | LoadGames
  | LoadGamesFail
  | LoadGamesSuccess;
