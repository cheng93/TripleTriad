import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {
  MatCardModule,
  MatPaginatorModule,
  MatTableModule
} from '@angular/material';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { BoardComponent } from './components/board/board.component';
import { CardListComponent } from './components/card-list/card-list.component';
import { GameCardComponent } from './components/game-card/game-card.component';
import { GameTileComponent } from './components/game-tile/game-tile.component';
import { SelectedCardListComponent } from './components/selected-card-list/selected-card-list.component';
import { RoomComponent } from './containers/room/room.component';
import { SelectCardsComponent } from './containers/select-cards/select-cards.component';
import { GameRoomEffects } from './effects/game-room.effects';
import { SelectCardsEffects } from './effects/select-cards.effects';
import { routes } from './games.routes';
import * as fromGames from './reducers';
import { GameRoomService } from './services/game-room.service';
import { GameSignalRFacade } from './services/game-signal-r.facade';
import { GameSignalRService } from './services/game-signal-r.service';
import { SelectCardsService } from './services/select-cards.service';

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
    RoomComponent,
    SelectCardsComponent,
    CardListComponent,
    SelectedCardListComponent,
    BoardComponent,
    GameTileComponent,
    GameCardComponent
  ]
})
export class GamesModule {}
