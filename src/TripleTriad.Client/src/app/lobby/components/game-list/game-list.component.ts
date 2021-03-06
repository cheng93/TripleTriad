import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.scss']
})
export class GameListComponent {
  @Input()
  gameIds: number[];

  @Output()
  join = new EventEmitter<number>();

  columns: string[] = ['gameId', 'rules', 'actions'];
}
