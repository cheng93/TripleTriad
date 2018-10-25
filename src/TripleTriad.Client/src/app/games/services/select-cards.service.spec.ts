import { TestBed, inject } from '@angular/core/testing';

import { SelectCardsService } from './select-cards.service';

describe('SelectCardsService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SelectCardsService]
    });
  });

  it('should be created', inject([SelectCardsService], (service: SelectCardsService) => {
    expect(service).toBeTruthy();
  }));
});
