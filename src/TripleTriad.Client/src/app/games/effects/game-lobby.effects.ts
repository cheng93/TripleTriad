import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { switchMap, map, catchError, tap } from 'rxjs/operators';
import {
  GameLobbyActionTypes,
  LoadGamesSuccess,
  LoadGamesFail,
  CreateGameSuccess,
  CreateGameFail
} from '../actions/game-lobby.actions';
import { GameLobbyService } from '../services/game-lobby.service';
import { of } from 'rxjs';
import { Router } from '@angular/router';

@Injectable()
export class GameLobbyEffects {
  @Effect()
  createGame$ = this.actions$.pipe(
    ofType(GameLobbyActionTypes.CreateGame),
    switchMap(() =>
      this.service.createGame().pipe(
        map(response => new CreateGameSuccess(response.gameId)),
        catchError(error => of(new CreateGameFail(error)))
      )
    )
  );

  @Effect({ dispatch: false })
  createGameSuccess$ = this.actions$.pipe(
    ofType<CreateGameSuccess>(GameLobbyActionTypes.CreateGameSuccess),
    tap(action => {
      console.log(action.payload);
      this.router.navigate([action.payload]);
    })
  );

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

  constructor(
    private actions$: Actions,
    private service: GameLobbyService,
    private router: Router
  ) {}
}
