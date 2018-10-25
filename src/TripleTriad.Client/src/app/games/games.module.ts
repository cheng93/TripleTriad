import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { GameLobbyComponent } from './containers/game-lobby/game-lobby.component';
import { routes } from './games.routes';
import * as fromGames from './reducers';
import { GameLobbyService } from './services/game-lobby.service';
import { GameListComponent } from './components/game-list/game-list.component';
import { GameLobbyEffects } from './effects/game-lobby.effects';
import { MatTableModule } from '@angular/material';
import { GameRoomComponent } from './containers/game-room/game-room.component';
import { GameSignalRService } from './services/game-signal-r.service';
import { GameRoomService } from './services/game-room.service';
import { GameRoomEffects } from './effects/game-room.effects';
import { SelectCardsEffects } from './effects/select-cards.effects';
import { SelectCardsComponent } from './containers/select-cards/select-cards.component';
import { SelectCardsService } from './services/select-cards.service';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    StoreModule.forFeature('games', fromGames.reducers),
    EffectsModule.forFeature([
      GameLobbyEffects,
      GameRoomEffects,
      SelectCardsEffects
    ]),
    MatTableModule
  ],
  providers: [
    GameLobbyService,
    GameSignalRService,
    GameRoomService,
    SelectCardsService
  ],
  declarations: [
    GameLobbyComponent,
    GameListComponent,
    GameRoomComponent,
    SelectCardsComponent
  ]
})
export class GamesModule {}
