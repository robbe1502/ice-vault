import { ChangeDetectionStrategy, Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'base',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: ``
})
export class BaseComponent implements OnDestroy {
  destroy$ = new Subject<void>();

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
