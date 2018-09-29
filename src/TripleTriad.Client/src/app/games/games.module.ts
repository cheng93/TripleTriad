import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { routes } from './games.routes';
import * as fromGames from './reducers';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('games', fromGames.reducers)
  ],
  declarations: []
})
export class GamesModule {}
