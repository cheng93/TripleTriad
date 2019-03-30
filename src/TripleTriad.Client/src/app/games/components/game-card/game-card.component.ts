import { Component, Input } from '@angular/core';
import { TileCard } from '../../models/tile';

@Component({
  selector: 'app-game-card',
  templateUrl: './game-card.component.html',
  styleUrls: ['./game-card.component.scss']
})
export class GameCardComponent {
  @Input()
  card: TileCard;
}
