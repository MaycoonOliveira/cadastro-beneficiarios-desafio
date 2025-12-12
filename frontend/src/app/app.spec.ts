import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { NZ_ICONS } from 'ng-zorro-antd/icon';
import {
  MedicineBoxFill,
  ScheduleOutline,
  TeamOutline,
  MenuFoldOutline,
  MenuUnfoldOutline
} from '@ant-design/icons-angular/icons';
import { App } from './app';

const icons = [
  MedicineBoxFill,
  ScheduleOutline,
  TeamOutline,
  MenuFoldOutline,
  MenuUnfoldOutline
];

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        App,
        NoopAnimationsModule,
        HttpClientModule
      ],
      providers: [
        provideRouter([]),
        { provide: NZ_ICONS, useValue: icons }
      ]
    }).compileComponents();
  });

  it('deve criar o componente app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`deve ter o título correto no sidebar`, () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.sidebar-logo h1')?.textContent).toBeTruthy();
  });
});
