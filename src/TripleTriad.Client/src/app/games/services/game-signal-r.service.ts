import { Injectable } from '@angular/core';
import { HubConnectionBuilder, HubConnection, LogLevel } from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class GameSignalRService {
  constructor() {}

  connect() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/gameHub')
      .configureLogging(LogLevel.Information)
      .build();

    this.registerOnServerEvents();

    return this.hubConnection
      .start()
      .catch(err => console.error(err.toString()));
  }

  viewGame(gameId: number) {
    this.hubConnection.invoke('ViewGame', gameId);
  }

  private connected: boolean;

  private hubConnection: HubConnection;

  private registerOnServerEvents() {
    this.hubConnection.on('Send', data => {
      console.log(data);
    });
  }
}
