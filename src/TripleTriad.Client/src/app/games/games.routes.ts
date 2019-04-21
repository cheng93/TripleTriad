import { Routes } from '@angular/router';
import { GameRoomComponent } from './containers/game-room/game-room.component';

export const routes: Routes = [
  { path: ':gameId', component: GameRoomComponent }
];
