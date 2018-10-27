import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectedCardListComponent } from './selected-card-list.component';

describe('SelectedCardListComponent', () => {
  let component: SelectedCardListComponent;
  let fixture: ComponentFixture<SelectedCardListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SelectedCardListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectedCardListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
