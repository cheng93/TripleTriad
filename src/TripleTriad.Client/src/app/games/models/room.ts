import { Tile } from './tile';

export interface Room {
  gameId: number;
  status: string;
  tiles: Tile[];
}

export interface View {
  isHost: boolean;
  isPlayerTwo: boolean;
}
