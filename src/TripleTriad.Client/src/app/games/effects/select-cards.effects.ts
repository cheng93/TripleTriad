import { Injectable } from '@angular/core';
import { Actions, Effect, ofType } from '@ngrx/effects';
import { SelectCardsService } from '../services/select-cards.service';
import {
  LoadAllCards,
  SelectCardsActionTypes,
  LoadAllCardsSuccess,
  LoadAllCardsFail,
  SubmitCardsSuccess,
  SubmitCardsFail
} from '../actions/select-cards.actions';
import { switchMap, map, catchError, withLatestFrom } from 'rxjs/operators';
import { of } from 'rxjs';
import * as fromStore from '../reducers';
import { Store } from '@ngrx/store';

@Injectable()
export class SelectCardsEffects {
  @Effect()
  loadAllCards$ = this.actions$.pipe(
    ofType<LoadAllCards>(SelectCardsActionTypes.LoadAllCards),
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
        .select(fromStore.getSelectedCards)
        .pipe(withLatestFrom(this.store.select(fromStore.getRoomGameId)))
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
    private store: Store<fromStore.GamesState>,
    private selectCardsService: SelectCardsService
  ) {}
}
