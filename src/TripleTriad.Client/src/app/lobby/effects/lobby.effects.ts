import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { switchMap, map, tap } from 'rxjs/operators';

import {
  LobbyActionTypes,
  LobbyActions,
  LoadGamesSuccess,
  CreateGameSuccess,
  JoinGame
} from '../actions/lobby.actions';
import { LobbyService } from '../services/lobby.service';

@Injectable()
export class LobbyEffects {
  @Effect()
  createGame$ = this.actions$.pipe(
    ofType(LobbyActionTypes.CreateGame),
    switchMap(() =>
      this.lobbyService
        .createGame()
        .pipe(map(response => new CreateGameSuccess(response.gameId)))
    )
  );

  @Effect({ dispatch: false })
  createGameSuccess$ = this.actions$.pipe(
    ofType<CreateGameSuccess>(LobbyActionTypes.CreateGameSuccess),
    tap(action => {
      this.router.navigate([action.gameId]);
    })
  );

  @Effect({ dispatch: false })
  joinGame$ = this.actions$.pipe(
    ofType<JoinGame>(LobbyActionTypes.JoinGame),
    switchMap(action =>
      this.lobbyService
        .joinGame(action.gameId)
        .pipe(tap(response => this.router.navigate([response.gameId])))
    )
  );

  @Effect()
  loadGames$ = this.actions$.pipe(
    ofType(LobbyActionTypes.LoadGames),
    switchMap(() =>
      this.lobbyService
        .getGames()
        .pipe(map(response => new LoadGamesSuccess(response.gameIds)))
    )
  );

  constructor(
    private actions$: Actions<LobbyActions>,
    private router: Router,
    private lobbyService: LobbyService
  ) {}
}
