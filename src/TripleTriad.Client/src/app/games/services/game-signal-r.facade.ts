import { Injectable } from '@angular/core';
import { TokenService } from 'src/app/core/services/token.service';
import { GameSignalRService } from './game-signal-r.service';

@Injectable()
export class GameSignalRFacade {
  constructor(
    private tokenService: TokenService,
    private gameSignalRService: GameSignalRService
  ) {}

  joinLobby() {
    return this.getTokenAndConnect().then(() =>
      this.gameSignalRService.joinLobby()
    );
  }

  viewGame(gameId: number) {
    return this.getTokenAndConnect().then(() =>
      this.gameSignalRService.viewGame(gameId)
    );
  }

  private getTokenAndConnect() {
    return this.tokenService
      .getAccessToken()
      .toPromise()
      .then(token => this.gameSignalRService.connect(token));
  }
}
