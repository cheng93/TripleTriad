import { Component, Input } from '@angular/core';
import { Card } from '../../models/card';

@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.scss']
})
export class CardListComponent {
  constructor() {}

  @Input()
  cards: Card[];

  @Input()
  selectedCards: Card[];

  isCardSelected(card: Card): boolean {
    return this.selectedCards.some(x => x.name === card.name);
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
