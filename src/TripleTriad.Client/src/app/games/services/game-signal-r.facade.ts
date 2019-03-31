import { Injectable } from '@angular/core';
import { TokenService } from 'src/app/core/services/token.service';
import { GameSignalRService } from './game-signal-r.service';

@Injectable()
export class GameSignalRFacade {
  constructor(
    private tokenService: TokenService,
    private gameSignalRService: GameSignalRService
  ) {}

  viewGame(gameId: number) {
    return this.tokenService
      .getAccessToken()
      .toPromise()
      .then(token => this.gameSignalRService.connect(token))
      .then(() => this.gameSignalRService.viewGame(gameId));
  }
}
