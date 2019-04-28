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
import { CardComponent } from './components/card/card.component';
import { SelectedCardListComponent } from './components/selected-card-list/selected-card-list.component';
import { TileComponent } from './components/tile/tile.component';
import { RoomComponent } from './containers/room/room.component';
import { SelectCardsComponent } from './containers/select-cards/select-cards.component';
import { RoomEffects } from './effects/room.effects';
import { SelectCardsEffects } from './effects/select-cards.effects';
import { routes } from './games.routes';
import * as fromGames from './reducers';
import { RoomService } from './services/room.service';
import { SelectCardsService } from './services/select-cards.service';

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('games', fromGames.reducers),
    EffectsModule.forFeature([RoomEffects, SelectCardsEffects]),
    MatPaginatorModule,
    MatTableModule,
    MatCardModule,
    RouterModule.forChild(routes)
  ],
  providers: [RoomService, SelectCardsService],
  declarations: [
    RoomComponent,
    SelectCardsComponent,
    CardListComponent,
    SelectedCardListComponent,
    BoardComponent,
    TileComponent,
    CardComponent
  ]
})
export class GamesModule {}
