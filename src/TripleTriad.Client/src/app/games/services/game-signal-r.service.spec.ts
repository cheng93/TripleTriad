import { TestBed, inject } from '@angular/core/testing';

import { GameSignalRService } from './game-signal-r.service';

describe('GameSignalRService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GameSignalRService]
    });
  });

  it('should be created', inject([GameSignalRService], (service: GameSignalRService) => {
    expect(service).toBeTruthy();
  }));
});
