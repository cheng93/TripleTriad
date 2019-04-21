import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { LobbyComponent } from './containers/lobby/lobby.component';
import { LobbyEffects } from './effects/lobby.effects';
import { LobbyService } from './services/lobby.service';
import * as fromLobby from './reducers/lobby.reducer';
import { routes } from './lobby.routes';

@NgModule({
  declarations: [LobbyComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('lobby', fromLobby.reducer),
    EffectsModule.forFeature([LobbyEffects])
  ],
  providers: [LobbyService]
})
export class LobbyModule {}
