import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import {
  GameRoomActionTypes,
  ViewGame,
  ViewGameSuccess,
  ViewGameFail
} from '../actions/game-room.actions';
import { switchMap, map, catchError } from 'rxjs/operators';
import { GameRoomService } from '../services/game-room.service';
import { of } from 'rxjs';

@Injectable()
export class GameRoomEffects {
  @Effect()
  viewGame$ = this.actions$.pipe(
    ofType<ViewGame>(GameRoomActionTypes.ViewGame),
    switchMap(action =>
      this.gameRoomService.viewGame(action.payload).pipe(
        map(response => new ViewGameSuccess(response)),
        catchError(error => of(new ViewGameFail(error)))
      )
    )
  );

  constructor(
    private actions$: Actions,
    private gameRoomService: GameRoomService
  ) {}
}
