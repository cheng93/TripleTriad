import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { select, Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ViewGame } from '../../actions/game-room.actions';
import { Tile } from '../../models/tile';
import * as fromGame from '../../reducers';
import { GameSignalRFacade } from '../../services/game-signal-r.facade';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})
export class RoomComponent implements OnInit {
  constructor(
    private store: Store<fromGame.State>,
    private route: ActivatedRoute,
    private signalR: GameSignalRFacade
  ) {}

  ngOnInit() {
    this.status$ = this.store.pipe(select(fromGame.getRoomStatus));
    this.tiles$ = this.store.pipe(select(fromGame.getRoomTiles));
    this.chooseCardStatus$ = this.status$.pipe(map(x => x === 'ChooseCards'));
    this.showBoard$ = this.status$.pipe(
      map(x => x === 'InProgress' || x === 'Finished')
    );

    var gameId = +this.route.snapshot.paramMap.get('gameId');
    this.signalR
      .viewGame(gameId)
      .then(() => this.store.dispatch(new ViewGame(gameId)));
  }

  status$: Observable<string>;
  tiles$: Observable<Tile[]>;
  chooseCardStatus$: Observable<boolean>;
  showBoard$: Observable<boolean>;
}
