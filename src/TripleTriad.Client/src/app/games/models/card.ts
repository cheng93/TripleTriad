export interface Card {
  name: string;
  level: number;
  rank: Rank;
  element: string;
}

export interface Rank {
  top: number;
  right: number;
  bottom: number;
  left: number;
}
