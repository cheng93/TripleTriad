import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection, LogLevel } from '@aspnet/signalr';
import { Store, select } from '@ngrx/store';
import * as fromStore from '../reducers';
import { UpdateGame } from '../actions/game-room.actions';
import { Observable, of, BehaviorSubject } from 'rxjs';
import { withLatestFrom, tap, filter, map } from 'rxjs/operators';
import { Room } from '../models/room';
import { Message } from '../models/message';

@Injectable({
  providedIn: 'root'
})
export class GameSignalRService {
  constructor(private store: Store<fromStore.GamesState>) {
    this.gameId$ = store.pipe(select(fromStore.getRoomGameId));
  }

  connect(accessToken: string) {
    if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl('/gameHub', {
          accessTokenFactory: () => accessToken
        })
        .configureLogging(LogLevel.Information)
        .build();

      this.registerOnServerEvents();
    }

    if (!this.connected) {
      return this.hubConnection
        .start()
        .then(() => (this.connected = true))
        .catch(err => {
          console.error(err.toString());
          return this.connected;
        });
    }
    return Promise.resolve(this.connected);
  }

  viewGame(gameId: number) {
    return this.hubConnection.invoke('ViewGame', gameId);
  }

  private gameId$: Observable<number>;

  private connected: boolean;

  private hubConnection: HubConnection;

  private registerOnServerEvents() {
    this.hubConnection.on('Send', (json: string) => {
      const message = <Message>JSON.parse(json);
      if (message.type == 'UpdateGame') {
        this.gameId$.subscribe(gameId => {
          var room = <Room>message.data;
          if (gameId == room.gameId) {
            this.store.dispatch(new UpdateGame(room));
          }
        });
      }
      console.log(json);
    });
  }
}
