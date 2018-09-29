import { TestBed, inject } from '@angular/core/testing';

import { GameLobbyService } from './game-lobby.service';

describe('GameLobbyService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GameLobbyService]
    });
  });

  it('should be created', inject([GameLobbyService], (service: GameLobbyService) => {
    expect(service).toBeTruthy();
  }));
});
