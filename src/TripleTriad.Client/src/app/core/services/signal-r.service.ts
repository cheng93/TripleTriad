import { Injectable } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
  HubConnectionState
} from '@aspnet/signalr';
import { Store } from '@ngrx/store';

import * as fromCore from '../reducers/core.reducer';
import { ReceiveSignalRMessage } from '../actions/core.actions';

@Injectable()
export class SignalRService {
  constructor(private store: Store<fromCore.State>) {}

  connect(accessToken: string): Promise<boolean> {
    if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl('/gameHub', {
          accessTokenFactory: () => accessToken
        })
        .configureLogging(LogLevel.Information)
        .build();

      this.registerServerEvents();
    }

    if (this.hubConnection.state === HubConnectionState.Disconnected) {
      return this.hubConnection
        .start()
        .then(() => this.connected)
        .catch(err => {
          console.error(err.toString());
          return this.connected;
        });
    }

    return Promise.resolve(this.connected);
  }

  joinLobby() {
    return this.hubConnection.invoke('JoinLobby');
  }

  viewGame(gameId: number) {
    return this.hubConnection.invoke('ViewGame', gameId);
  }

  private hubConnection: HubConnection;

  private get connected(): boolean {
    return (
      this.hubConnection &&
      this.hubConnection.state === HubConnectionState.Connected
    );
  }

  private registerServerEvents() {
    this.hubConnection.on('Send', (message: string) => {
      this.store.dispatch(new ReceiveSignalRMessage(message));
      console.log(message);
    });
  }
}
