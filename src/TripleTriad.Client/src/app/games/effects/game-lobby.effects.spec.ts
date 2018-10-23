import { TestBed, inject } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { GameLobbyEffects } from './game-lobby.effects';

describe('GameLobbyEffects', () => {
  let actions$: Observable<any>;
  let effects: GameLobbyEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        GameLobbyEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.get(GameLobbyEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
