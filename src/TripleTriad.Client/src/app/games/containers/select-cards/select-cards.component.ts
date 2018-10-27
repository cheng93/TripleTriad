import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromStore from '../../reducers';
import { filter, tap } from 'rxjs/operators';
import {
  LoadAllCards,
  ChangePage,
  SelectCard,
  RemoveCard
} from '../../actions/select-cards.actions';
import { Observable, Subscription } from 'rxjs';
import { Card } from '../../models/card';

@Component({
  selector: 'app-select-cards',
  templateUrl: './select-cards.component.html',
  styleUrls: ['./select-cards.component.scss']
})
export class SelectCardsComponent implements OnInit, OnDestroy {
  constructor(private store: Store<fromStore.GamesState>) {
    this.cards$ = store.select(fromStore.getLevelCards);
    this.selectedCards$ = store.select(fromStore.getSelectedCards);
  }

  cards$: Observable<Card[]>;

  selectedCards$: Observable<Card[]>;

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
