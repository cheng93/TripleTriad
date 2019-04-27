import { Component, Input } from '@angular/core';
import { Tile } from '../../models/tile';

@Component({
  selector: 'app-tile',
  templateUrl: './tile.component.html',
  styleUrls: ['./tile.component.scss']
})
export class TileComponent {
  @Input()
  tile: Tile;

  get styles() {
    return (
      this.tile.card && {
        host: this.tile.card.isHost,
        challenger: !this.tile.card.isHost
      }
    );
  }
}
