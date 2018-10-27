import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromStore from '../../reducers';
import { filter, tap, map } from 'rxjs/operators';
import {
  LoadAllCards,
  ChangePage,
  SelectCard,
  RemoveCard
} from '../../actions/select-cards.actions';
import { Observable, Subscription, combineLatest } from 'rxjs';
import { Card } from '../../models/card';
import { CardListCard } from '../../components/card-list/card-list.component';
import { SelectedCardListCard } from '../../components/selected-card-list/selected-card-list.component';

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
  }

  cardListCards$: Observable<CardListCard[]>;

  selectedCardListCards$: Observable<SelectedCardListCard[]>;

  changePage($event: number) {
    this.store.dispatch(new ChangePage($event));
  }

  removeCard($event: Card) {
    this.store.dispatch(new RemoveCard($event));
  }

  selectCard($event: Card) {
    this.store.dispatch(new SelectCard($event));
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
