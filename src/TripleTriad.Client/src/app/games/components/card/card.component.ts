import { Component, Input } from '@angular/core';
import { TileCard } from '../../models/tile';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.scss']
})
export class CardComponent {
  @Input()
  card: TileCard;
}
