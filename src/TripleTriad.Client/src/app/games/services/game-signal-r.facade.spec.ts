import { TestBed } from '@angular/core/testing';

import { GameSignalRFacade } from './game-signal-r.facade';

describe('GameSignalR.AdapterService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: GameSignalRFacade = TestBed.get(GameSignalRFacade);
    expect(service).toBeTruthy();
  });
});
