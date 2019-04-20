import { Injectable } from '@angular/core';
import { TokenService } from './token.service';
import { SignalRService } from './signal-r.service';

@Injectable()
export class SignalRFacade {
  constructor(
    private tokenService: TokenService,
    private signalRService: SignalRService
  ) {}

  connect() {
    return this.tokenService
      .getAccessToken()
      .toPromise()
      .then(token => this.signalRService.connect(token));
  }

  joinLobby() {
    return this.connect().then(() => {
      this.signalRService.joinLobby();
    });
  }

  viewGame(gameId: number) {
    return this.connect().then(() => {
      this.signalRService.viewGame(gameId);
    });
  }
}
