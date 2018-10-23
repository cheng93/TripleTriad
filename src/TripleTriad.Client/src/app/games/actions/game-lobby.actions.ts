import { Action } from '@ngrx/store';

export enum GameLobbyActionTypes {
  CreateGame = '[GameLobby] Create Game',
  CreateGameSuccess = '[GameLobby] Create Game Success',
  CreateGameFail = '[GameLobby] Create Game Fail',
  LoadGames = '[GameLobby] Load Games',
  LoadGamesSuccess = '[GameLobby] Load Games Success',
  LoadGamesFail = '[GameLobby] Load Games Fail',
  JoinGame = '[GameLobby] Join Game',
  JoinGameSuccess = '[GameLobby] Join Game Success',
  JoinGameFail = '[GameLobby] Join Game Fail'
}

export class CreateGame implements Action {
  readonly type = GameLobbyActionTypes.CreateGame;
}

export class CreateGameSuccess implements Action {
  readonly type = GameLobbyActionTypes.CreateGameSuccess;

  constructor(public payload: number) {}
}

export class CreateGameFail implements Action {
  readonly type = GameLobbyActionTypes.CreateGameFail;

  constructor(public payload: any) {}
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

export class JoinGame implements Action {
  readonly type = GameLobbyActionTypes.JoinGame;

  constructor(public payload: number) {}
}

export class JoinGameSuccess implements Action {
  readonly type = GameLobbyActionTypes.JoinGameSuccess;

  constructor(public payload: number) {}
}

export class JoinGameFail implements Action {
  readonly type = GameLobbyActionTypes.JoinGameFail;

  constructor(public payload: any) {}
}

export type GameLobbyActions =
  | LoadGames
  | LoadGamesSuccess
  | LoadGamesFail
  | CreateGame
  | CreateGameSuccess
  | CreateGameFail
  | JoinGame
  | JoinGameSuccess
  | JoinGameFail;
