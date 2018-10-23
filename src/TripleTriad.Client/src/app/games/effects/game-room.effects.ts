import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { GameRoomActionTypes, ViewGame } from '../actions/game-room.actions';
import { switchMap } from 'rxjs/operators';
import { GameRoomService } from '../services/game-room.service';

@Injectable()
export class GameRoomEffects {
  @Effect({ dispatch: false })
  viewGame$ = this.actions$.pipe(
    ofType<ViewGame>(GameRoomActionTypes.ViewGame),
    switchMap(action => this.gameRoomService.viewGame(action.payload))
  );

  constructor(
    private actions$: Actions,
    private gameRoomService: GameRoomService
  ) {}
}
