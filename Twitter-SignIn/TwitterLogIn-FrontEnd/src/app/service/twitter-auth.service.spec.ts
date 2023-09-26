import { TestBed } from '@angular/core/testing';

import { TwitterAuthService } from './twitter-auth.service';

describe('TwitterAuthService', () => {
  let service: TwitterAuthService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TwitterAuthService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
