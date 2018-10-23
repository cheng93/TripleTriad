import { TestBed, inject } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { GameRoomEffects } from './game-room.effects';

describe('GameRoomEffects', () => {
  let actions$: Observable<any>;
  let effects: GameRoomEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        GameRoomEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.get(GameRoomEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
