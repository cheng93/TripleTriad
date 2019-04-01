import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import * as fromStore from '../../reducers';
import {
  LoadGames,
  CreateGame,
  JoinGame
} from '../../actions/game-lobby.actions';
import { Observable } from 'rxjs';
import { GameSignalRFacade } from '../../services/game-signal-r.facade';

@Component({
  selector: 'app-game-lobby',
  templateUrl: './game-lobby.component.html',
  styleUrls: ['./game-lobby.component.scss']
})
export class GameLobbyComponent implements OnInit {
  gameIds$: Observable<number[]>;

  constructor(
    private store: Store<fromStore.GamesState>,
    private gameSignalRFacade: GameSignalRFacade
  ) {
    this.gameIds$ = store.pipe(select(fromStore.getLobbyGameIds));
  }

  ngOnInit() {
    this.store.dispatch(new LoadGames());
    this.gameSignalRFacade.joinLobby();
  }

  createGame() {
    this.store.dispatch(new CreateGame());
  }

  joinGame(gameId: number) {
    this.store.dispatch(new JoinGame(gameId));
  }
}
