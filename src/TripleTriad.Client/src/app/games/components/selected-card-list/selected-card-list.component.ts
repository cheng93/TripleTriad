import { Component, Input, EventEmitter, Output } from '@angular/core';
import { Card } from '../../models/card';

export interface SelectedCardListCard extends Card {
  canRemove: boolean;
}

@Component({
  selector: 'app-selected-card-list',
  templateUrl: './selected-card-list.component.html',
  styleUrls: ['./selected-card-list.component.scss']
})
export class SelectedCardListComponent {
  constructor() {}

  @Input()
  cards: SelectedCardListCard[];

  @Output()
  removeCard: EventEmitter<Card> = new EventEmitter();

  remove(card: Card) {
    this.removeCard.emit(card);
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
}
