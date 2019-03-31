import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ActivatedRoute } from '@angular/router';
import { ViewGame } from '../../actions/game-room.actions';
import { Observable } from 'rxjs';
import * as fromStore from '../../reducers';
import { map } from 'rxjs/operators';
import { Tile } from '../../models/tile';
import { GameSignalRFacade } from '../../services/game-signal-r.facade';

@Component({
  selector: 'app-game-room',
  templateUrl: './game-room.component.html',
  styleUrls: ['./game-room.component.scss']
})
export class GameRoomComponent implements OnInit {
  constructor(
    private store: Store<fromStore.GamesState>,
    private route: ActivatedRoute,
    private gameSignalRFacade: GameSignalRFacade
  ) {
    this.status$ = store.pipe(select(fromStore.getRoomStatus));
    this.tiles$ = store.pipe(select(fromStore.getRoomTiles));
    this.chooseCardStatus$ = this.status$.pipe(map(x => x === 'ChooseCards'));
    this.showBoard$ = this.status$.pipe(
      map(x => x === 'InProgress' || x === 'Finished')
    );
  }

  ngOnInit() {
    var gameId = +this.route.snapshot.paramMap.get('gameId');
    this.gameSignalRFacade
      .viewGame(gameId)
      .then(() => this.store.dispatch(new ViewGame(gameId)));
  }

  status$: Observable<string>;
  tiles$: Observable<Tile[]>;
  chooseCardStatus$: Observable<boolean>;
  showBoard$: Observable<boolean>;
}
