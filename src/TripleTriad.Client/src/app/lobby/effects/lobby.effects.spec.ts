import { TestBed, inject } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { LobbyEffects } from './lobby.effects';

describe('LobbyEffects', () => {
  let actions$: Observable<any>;
  let effects: LobbyEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        LobbyEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.get(LobbyEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
