import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromStore from '../../reducers';
import { filter, tap, map } from 'rxjs/operators';
import {
  LoadAllCards,
  ChangePage,
  SelectCard,
  RemoveCard,
  SubmitCards
} from '../../actions/select-cards.actions';
import { Observable, Subscription, combineLatest } from 'rxjs';
import { Card, CardListCard, SelectedCardListCard } from '../../models/card';

@Component({
  selector: 'app-select-cards',
  templateUrl: './select-cards.component.html',
  styleUrls: ['./select-cards.component.scss']
})
export class SelectCardsComponent implements OnInit, OnDestroy {
  constructor(private store: Store<fromStore.GamesState>) {
    this.cardListCards$ = combineLatest(
      store.select(fromStore.getLevelCards),
      store.select(fromStore.hasSubmittedCards),
      store.select(fromStore.getSelectedCards)
    ).pipe(
      map(([cards, hasSubmittedCards, selectedCards]) => [
        ...cards.map(x => ({
          ...x,
          canSelect:
            !hasSubmittedCards &&
            selectedCards.every(y => y.name !== x.name) &&
            selectedCards.length < 5
        }))
      ])
    );

    this.selectedCardListCards$ = combineLatest(
      store.select(fromStore.getSelectedCards),
      store.select(fromStore.hasSubmittedCards)
    ).pipe(
      map(([cards, hasSubmittedCards]) => [
        ...cards.map(x => ({
          ...x,
          canRemove: !hasSubmittedCards
        }))
      ])
    );

    this.showSubmit$ = store.select(fromStore.showSelectCardSubmit);
  }

  cardListCards$: Observable<CardListCard[]>;

  selectedCardListCards$: Observable<SelectedCardListCard[]>;

  showSubmit$: Observable<boolean>;

  changePage($event: number) {
    this.store.dispatch(new ChangePage($event));
  }

  removeCard($event: Card) {
    this.store.dispatch(new RemoveCard($event));
  }

  selectCard($event: Card) {
    this.store.dispatch(new SelectCard($event));
  }

  submitCards() {
    this.store.dispatch(new SubmitCards());
  }

  ngOnInit() {
    this.subscription = this.store
      .select(fromStore.getAllCardsLoaded)
      .pipe(
        filter(x => !x),
        tap(x => this.store.dispatch(new LoadAllCards()))
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  private subscription: Subscription;
}
