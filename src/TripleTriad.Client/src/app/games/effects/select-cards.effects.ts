import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import {
  catchError,
  filter,
  map,
  switchMap,
  withLatestFrom
} from 'rxjs/operators';
import {
  LoadAllCards,
  LoadAllCardsFail,
  LoadAllCardsSuccess,
  SelectCardsActionTypes,
  SubmitCardsFail,
  SubmitCardsSuccess
} from '../actions/select-cards.actions';
import * as fromGame from '../reducers';
import { SelectCardsService } from '../services/select-cards.service';

@Injectable()
export class SelectCardsEffects {
  @Effect()
  loadAllCards$ = this.actions$.pipe(
    ofType<LoadAllCards>(SelectCardsActionTypes.LoadAllCards),
    withLatestFrom(this.store.select(fromGame.getAllCardsLoaded)),
    filter(([action, allCardsLoaded]) => !allCardsLoaded),
    switchMap(action =>
      this.selectCardsService.getAllCards().pipe(
        map(response => new LoadAllCardsSuccess(response.cards)),
        catchError(error => of(new LoadAllCardsFail(error)))
      )
    )
  );

  @Effect()
  submitCards$ = this.actions$.pipe(
    ofType(SelectCardsActionTypes.SubmitCards),
    withLatestFrom(
      this.store
        .select(fromGame.getSelectedCards)
        .pipe(withLatestFrom(this.store.select(fromGame.getRoomGameId)))
    ),
    switchMap(([action, [cards, gameId]]) =>
      this.selectCardsService.submitCards(gameId, cards).pipe(
        map(response => new SubmitCardsSuccess()),
        catchError(error => of(new SubmitCardsFail(error)))
      )
    )
  );

  constructor(
    private actions$: Actions,
    private store: Store<fromGame.GamesState>,
    private selectCardsService: SelectCardsService
  ) {}
}
