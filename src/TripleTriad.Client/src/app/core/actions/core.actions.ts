import { Action } from '@ngrx/store';

export enum CoreActionTypes {
  ReceiveSignalRMessage = '[Core] Receive SignalR Message'
}

export class ReceiveSignalRMessage implements Action {
  readonly type = CoreActionTypes.ReceiveSignalRMessage;

  constructor(public message: string) {}
}

export type CoreActions = ReceiveSignalRMessage;
