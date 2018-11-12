import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { GameSignalRService } from '../../services/game-signal-r.service';
import { ActivatedRoute } from '@angular/router';
import { ViewGame } from '../../actions/game-room.actions';
import { TokenService } from 'src/app/core/services/token.service';
import { Observable } from 'rxjs';
import * as fromStore from '../../reducers';
import { map } from 'rxjs/operators';
import { Tile } from '../../models/tile';

@Component({
  selector: 'app-game-room',
  templateUrl: './game-room.component.html',
  styleUrls: ['./game-room.component.scss']
})
export class GameRoomComponent implements OnInit {
  constructor(
    private store: Store<fromStore.GamesState>,
    private route: ActivatedRoute,
    private gameSignalRService: GameSignalRService,
    private tokenService: TokenService
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
    this.tokenService.getAccessToken().subscribe(token => {
      this.gameSignalRService.connect(token).then(() => {
        this.gameSignalRService.viewGame(gameId);
        this.store.dispatch(new ViewGame(gameId));
      });
    });
  }

  status$: Observable<string>;
  tiles$: Observable<Tile[]>;
  chooseCardStatus$: Observable<boolean>;
  showBoard$: Observable<boolean>;
}
