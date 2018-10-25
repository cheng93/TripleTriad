import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import * as fromStore from '../../reducers';
import { filter, tap } from 'rxjs/operators';
import { LoadAllCards } from '../../actions/select-cards.actions';
import { Observable } from 'rxjs';
import { Card } from '../../models/card';

@Component({
  selector: 'app-select-cards',
  templateUrl: './select-cards.component.html',
  styleUrls: ['./select-cards.component.scss']
})
export class SelectCardsComponent implements OnInit {
  constructor(private store: Store<fromStore.GamesState>) {
    this.cards$ = store.select(fromStore.getAllCards);
  }

  cards$: Observable<Card[]>;

  ngOnInit() {
    this.store
      .select(fromStore.getAllCardsLoaded)
      .pipe(
        filter(x => !x),
        tap(x => this.store.dispatch(new LoadAllCards()))
      )
      .subscribe();
  }
}
