import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogsDashboardComponent } from './logs-dashboard.component';

describe('LogsDashboardComponent', () => {
  let component: LogsDashboardComponent;
  let fixture: ComponentFixture<LogsDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogsDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogsDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
