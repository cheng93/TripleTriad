import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadChildren: './lobby/lobby.module#LobbyModule' },
  { path: ':gameId', loadChildren: './games/games.module#GamesModule' }
];
