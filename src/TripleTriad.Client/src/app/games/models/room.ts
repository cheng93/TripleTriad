import { Tile } from './tile';

export interface Room {
  gameId: number;
  status: string;
  tiles: Tile[];
}

export interface View {
  isPlayerOne: boolean;
  isPlayerTwo: boolean;
}
