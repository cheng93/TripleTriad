import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { GameLobbyActionTypes } from '../actions/game-lobby.actions';

@Injectable()
export class GameLobbyEffects {

  @Effect()
  loadFoos$ = this.actions$.pipe(ofType(GameLobbyActionTypes.LoadGameLobbys));

  constructor(private actions$: Actions) {}
}
