import { Component, Input } from '@angular/core';
import { Tile } from '../../models/tile';

@Component({
  selector: 'app-game-tile',
  templateUrl: './game-tile.component.html',
  styleUrls: ['./game-tile.component.scss']
})
export class GameTileComponent {
  @Input()
  tile: Tile;

  get styles() {
    return (
      this.tile.card && {
        host: this.tile.card.isHost,
        playerTwo: !this.tile.card.isHost
      }
    );
  }
}
