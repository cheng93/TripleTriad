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

export interface CardListCard extends Card {
  canSelect: boolean;
}

export interface SelectedCardListCard extends Card {
  canRemove: boolean;
}
