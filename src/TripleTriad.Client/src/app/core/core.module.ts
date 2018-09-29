import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToolbarComponent } from './components/toolbar/toolbar.component';

@NgModule({
  imports: [CommonModule],
  declarations: [ToolbarComponent],
  exports: [ToolbarComponent]
})
export class CoreModule {
  static forRoot() {
    return {
      ngModule: CoreModule
    };
  }
}
