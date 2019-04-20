import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToolbarComponent } from './components/toolbar/toolbar.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenService } from './services/token.service';
import { AuthenticationInterceptorService } from './services/authentication-interceptor.service';
import { UnauthorizedInterceptorService } from './services/unauthorized-interceptor.service';
import { StoreModule } from '@ngrx/store';
import * as fromCore from './reducers/core.reducer';
import { EffectsModule } from '@ngrx/effects';
import { CoreEffects } from './effects/core.effects';
import { SignalRFacade } from './services/signal-r.facade';
import { SignalRService } from './services/signal-r.service';

@NgModule({
  imports: [
    CommonModule,
    StoreModule.forFeature('core', fromCore.reducer),
    EffectsModule.forFeature([CoreEffects])
  ],
  providers: [SignalRService],
  declarations: [ToolbarComponent],
  exports: [ToolbarComponent]
})
export class CoreModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: CoreModule,
      providers: [
        TokenService,
        SignalRFacade,
        {
          provide: HTTP_INTERCEPTORS,
          useClass: AuthenticationInterceptorService,
          multi: true
        },
        {
          provide: HTTP_INTERCEPTORS,
          useClass: UnauthorizedInterceptorService,
          multi: true
        }
      ]
    };
  }
}
