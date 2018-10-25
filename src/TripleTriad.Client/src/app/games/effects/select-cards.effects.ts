import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { SelectCardsService } from '../services/select-cards.service';
import {
  LoadAllCards,
  SelectCardsActionTypes,
  LoadAllCardsSuccess,
  LoadAllCardsFail
} from '../actions/select-cards.actions';
import { switchMap, map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class SelectCardsEffects {
  @Effect()
  loadAllCards$ = this.actions$.pipe(
    ofType<LoadAllCards>(SelectCardsActionTypes.LoadAllCards),
    switchMap(action =>
      this.selectCardsService.getAllCards().pipe(
        map(response => new LoadAllCardsSuccess(response.Cards)),
        catchError(error => of(new LoadAllCardsFail(error)))
      )
    )
  );

  constructor(
    private actions$: Actions,
    private selectCardsService: SelectCardsService
  ) {}
}
