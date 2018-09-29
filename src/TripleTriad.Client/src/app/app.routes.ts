import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadChildren: './games/games.module#GamesModule' }
];
