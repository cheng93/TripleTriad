import { Card } from './card';

export interface Tile {
  tileId: number;
  card: TileCard;
  element: string;
}

export interface TileCard extends Card {
  isPlayerOne: boolean;
  modifier: number;
}
