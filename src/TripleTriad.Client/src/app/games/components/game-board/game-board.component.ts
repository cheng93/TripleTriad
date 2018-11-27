import { Component, Input } from '@angular/core';
import { Tile } from '../../models/tile';

@Component({
  selector: 'app-game-board',
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.scss']
})
export class GameBoardComponent {
  @Input()
  tiles: Tile[];
}
