import { Routes } from '@angular/router';
import { GameLobbyComponent } from './containers/game-lobby/game-lobby.component';
import { GameRoomComponent } from './containers/game-room/game-room.component';

export const routes: Routes = [
  { path: '', component: GameLobbyComponent },
  { path: ':gameId', component: GameRoomComponent }
];
