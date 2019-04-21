import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadChildren: './lobby/lobby.module#LobbyModule',
    pathMatch: 'full'
  },
  { path: '', loadChildren: './games/games.module#GamesModule' }
];
