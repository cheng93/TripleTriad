import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SelectCardsComponent } from './select-cards.component';
import { Store, StoreModule } from '@ngrx/store';

describe('SelectCardsComponent', () => {
  let component: SelectCardsComponent;
  let fixture: ComponentFixture<SelectCardsComponent>;
  let store: Store<any>;

  beforeEach(async() => {
    TestBed.configureTestingModule({
      imports: [ StoreModule.forRoot({}) ],
      declarations: [ SelectCardsComponent ]
    });

    await TestBed.compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SelectCardsComponent);
    component = fixture.componentInstance;
    store = TestBed.get(Store);

    spyOn(store, 'dispatch').and.callThrough();
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
