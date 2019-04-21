import { Action } from '@ngrx/store';
import { Message } from '../services/signal-r.service';

export enum CoreActionTypes {
  ReceiveSignalRMessage = '[Core] Receive SignalR Message'
}

export class ReceiveSignalRMessage implements Action {
  readonly type = CoreActionTypes.ReceiveSignalRMessage;

  constructor(public message: Message) {}
}

export type CoreActions = ReceiveSignalRMessage;
