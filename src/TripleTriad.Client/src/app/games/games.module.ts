import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { GameLobbyComponent } from './game-lobby/game-lobby.component';
import { routes } from './games.routes';
import * as fromGames from './reducers';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('games', fromGames.reducers)
  ],
  declarations: [GameLobbyComponent]
})
export class GamesModule {}
