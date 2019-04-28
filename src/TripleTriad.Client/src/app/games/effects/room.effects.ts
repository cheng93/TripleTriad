import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import {
  RoomActionTypes,
  ViewGame,
  ViewGameFail,
  ViewGameSuccess
} from '../actions/room.actions';
import { RoomService } from '../services/room.service';

@Injectable()
export class RoomEffects {
  @Effect()
  viewGame$ = this.actions$.pipe(
    ofType<ViewGame>(RoomActionTypes.ViewGame),
    switchMap(action =>
      this.roomService.viewGame(action.payload).pipe(
        map(response => new ViewGameSuccess(response)),
        catchError(error => of(new ViewGameFail(error)))
      )
    )
  );

  constructor(private actions$: Actions, private roomService: RoomService) {}
}
