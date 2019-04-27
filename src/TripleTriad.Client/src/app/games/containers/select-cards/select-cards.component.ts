import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { combineLatest, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import {
  ChangePage,
  LoadAllCards,
  RemoveCard,
  SelectCard,
  SubmitCards
} from '../../actions/select-cards.actions';
import { Card, CardListCard, SelectedCardListCard } from '../../models/card';
import * as fromStore from '../../reducers';

@Component({
  selector: 'app-select-cards',
  templateUrl: './select-cards.component.html',
  styleUrls: ['./select-cards.component.scss']
})
export class SelectCardsComponent implements OnInit {
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
    this.store.dispatch(new LoadAllCards());
  }
}
