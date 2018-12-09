import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OrdersDxgridComponent } from './orders-dxgrid.component';

describe('OrdersDxgridComponent', () => {
  let component: OrdersDxgridComponent;
  let fixture: ComponentFixture<OrdersDxgridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OrdersDxgridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OrdersDxgridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
