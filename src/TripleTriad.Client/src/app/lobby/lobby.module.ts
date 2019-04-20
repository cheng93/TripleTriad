import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StoreModule } from '@ngrx/store';
import * as fromLobby from './reducers/lobby.reducer';
import { EffectsModule } from '@ngrx/effects';
import { LobbyEffects } from './effects/lobby.effects';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    StoreModule.forFeature('lobby', fromLobby.reducer),
    EffectsModule.forFeature([LobbyEffects])
  ]
})
export class LobbyModule { }
