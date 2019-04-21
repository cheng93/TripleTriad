import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { routes } from './games.routes';
import * as fromGames from './reducers';
import {
  MatTableModule,
  MatPaginatorModule,
  MatCardModule
} from '@angular/material';
import { GameRoomComponent } from './containers/game-room/game-room.component';
import { GameSignalRService } from './services/game-signal-r.service';
import { GameSignalRFacade } from './services/game-signal-r.facade';
import { GameRoomService } from './services/game-room.service';
import { SelectCardsService } from './services/select-cards.service';
import { GameRoomEffects } from './effects/game-room.effects';
import { SelectCardsEffects } from './effects/select-cards.effects';
import { SelectCardsComponent } from './containers/select-cards/select-cards.component';
import { CardListComponent } from './components/card-list/card-list.component';
import { SelectedCardListComponent } from './components/selected-card-list/selected-card-list.component';
import { GameBoardComponent } from './components/game-board/game-board.component';
import { GameTileComponent } from './components/game-tile/game-tile.component';
import { GameCardComponent } from './components/game-card/game-card.component';

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('games', fromGames.reducers),
    EffectsModule.forFeature([GameRoomEffects, SelectCardsEffects]),
    MatPaginatorModule,
    MatTableModule,
    MatCardModule,
    RouterModule.forChild(routes)
  ],
  providers: [
    GameSignalRService,
    GameSignalRFacade,
    GameRoomService,
    SelectCardsService
  ],
  declarations: [
    GameRoomComponent,
    SelectCardsComponent,
    CardListComponent,
    SelectedCardListComponent,
    GameBoardComponent,
    GameTileComponent,
    GameCardComponent
  ]
})
export class GamesModule {}
