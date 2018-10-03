import { TestBed, inject } from '@angular/core/testing';

import { LogsHubService } from './logs-hub.service';

describe('LogsHubService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LogsHubService]
    });
  });

  it('should be created', inject([LogsHubService], (service: LogsHubService) => {
    expect(service).toBeTruthy();
  }));
});
