import { TestBed, inject } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable } from 'rxjs';

import { SelectCardsEffects } from './select-cards.effects';

describe('SelectCardsEffects', () => {
  let actions$: Observable<any>;
  let effects: SelectCardsEffects;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        SelectCardsEffects,
        provideMockActions(() => actions$)
      ]
    });

    effects = TestBed.get(SelectCardsEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});
