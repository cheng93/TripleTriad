import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Card, CardListCard } from '../../models/card';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.scss']
})
export class CardListComponent {
  constructor() {}

  @Input()
  cards: CardListCard[];

  @Output()
  selectCard: EventEmitter<Card> = new EventEmitter();

  @Output()
  changePage: EventEmitter<number> = new EventEmitter<number>();

  page($event: PageEvent) {
    this.changePage.emit($event.pageIndex);
  }

  select(card: Card) {
    this.selectCard.emit(card);
  }

  columns: string[] = [
    'name',
    'top',
    'right',
    'bottom',
    'left',
    'element',
    'actions'
  ];

  length: number = 110;
  pageSize: number = 11;
}
