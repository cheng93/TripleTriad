import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';

import { concatMap } from 'rxjs/operators';
import { EMPTY } from 'rxjs';
import { LobbyActionTypes, LobbyActions } from '../actions/lobby.actions';


@Injectable()
export class LobbyEffects {


  @Effect()
  loadLobbys$ = this.actions$.pipe(
    ofType(LobbyActionTypes.LoadLobbys),
    /** An EMPTY observable only emits completion. Replace with your own observable API request */
    concatMap(() => EMPTY)
  );


  constructor(private actions$: Actions<LobbyActions>) {}

}
