import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material';
import { RouterModule } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { GameListComponent } from './components/game-list/game-list.component';
import { LobbyComponent } from './containers/lobby/lobby.component';
import { LobbyEffects } from './effects/lobby.effects';
import { LobbyService } from './services/lobby.service';
import * as fromLobby from './reducers';
import { routes } from './lobby.routes';

@NgModule({
  declarations: [LobbyComponent, GameListComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('lobby', fromLobby.reducer),
    EffectsModule.forFeature([LobbyEffects]),
    MatTableModule
  ],
  providers: [LobbyService]
})
export class LobbyModule {}
