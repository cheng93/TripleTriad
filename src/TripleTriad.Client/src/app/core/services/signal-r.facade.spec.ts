import { TestBed } from '@angular/core/testing';

import { SignalRFacade } from './signal-r.facade';

describe('SignalRFacade', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SignalRFacade = TestBed.get(SignalRFacade);
    expect(service).toBeTruthy();
  });
});
