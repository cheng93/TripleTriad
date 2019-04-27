import { Component, Input } from '@angular/core';
import { Tile } from '../../models/tile';

@Component({
  selector: 'app-board',
  templateUrl: './board.component.html',
  styleUrls: ['./board.component.scss']
})
export class BoardComponent {
  @Input()
  tiles: Tile[];
}
