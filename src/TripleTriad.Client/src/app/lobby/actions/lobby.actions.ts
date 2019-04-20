import { Action } from '@ngrx/store';

export enum LobbyActionTypes {
  LoadLobbys = '[Lobby] Load Lobbys',
  
  
}

export class LoadLobbys implements Action {
  readonly type = LobbyActionTypes.LoadLobbys;
}


export type LobbyActions = LoadLobbys;
