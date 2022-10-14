import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';

// Components
import { BaseComponent } from './components/base/base.component';
import { InformationComponent } from './components/information/information.component';
import { SpinnerComponent } from './components/spinner/spinner.component';

@NgModule({
  imports: [CommonModule, TranslateModule],
  declarations: [BaseComponent, InformationComponent, SpinnerComponent],
  exports: [InformationComponent, SpinnerComponent]
})
export class SharedModule {}
