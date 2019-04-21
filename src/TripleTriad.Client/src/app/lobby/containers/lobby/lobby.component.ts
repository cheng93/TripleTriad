import { Component, OnInit } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { Observable } from 'rxjs';

import { SignalRFacade } from 'src/app/core/services/signal-r.facade';
import { LoadGames, CreateGame, JoinGame } from '../../actions/lobby.actions';
import * as fromLobby from '../../reducers/lobby.reducer';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.scss']
})
export class LobbyComponent implements OnInit {
  constructor(
    private store: Store<fromLobby.State>,
    private signalR: SignalRFacade
  ) {}

  ngOnInit() {
    this.gameIds$ = this.store.pipe(select(fromLobby.getGameIds));
    this.signalR.joinLobby();
    this.store.dispatch(new LoadGames());
  }

  gameIds$: Observable<number[]>;

  createGame() {
    this.store.dispatch(new CreateGame());
  }

  joinGame(gameId: number) {
    this.store.dispatch(new JoinGame(gameId));
  }
}
