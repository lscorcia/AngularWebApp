import { TestBed } from '@angular/core/testing';

import { OrderDetailResolverService } from './order-detail-resolver.service';

describe('OrderDetailResolverService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: OrderDetailResolverService = TestBed.get(OrderDetailResolverService);
    expect(service).toBeTruthy();
  });
});
