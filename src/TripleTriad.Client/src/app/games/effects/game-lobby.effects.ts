import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { switchMap, map, catchError } from 'rxjs/operators';
import {
  GameLobbyActionTypes,
  LoadGamesSuccess,
  LoadGamesFail
} from '../actions/game-lobby.actions';
import { GameLobbyService } from '../services/game-lobby.service';
import { of } from 'rxjs';

@Injectable()
export class GameLobbyEffects {
  @Effect()
  loadGames$ = this.actions$.pipe(
    ofType(GameLobbyActionTypes.LoadGames),
    switchMap(() =>
      this.service.getGames().pipe(
        map(response => new LoadGamesSuccess(response.gameIds)),
        catchError(error => of(new LoadGamesFail(error)))
      )
    )
  );

  constructor(private actions$: Actions, private service: GameLobbyService) {}
}
