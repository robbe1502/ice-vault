import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { InformationState } from '../../../store';

import { AuthenticationStore } from '../../store/authentication.store';

@Component({
  selector: 'ice-vault-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LayoutComponent implements OnInit {
  information$!: Observable<InformationState | undefined>;
  loading$!: Observable<boolean>;
  isRetrying$!: Observable<boolean>;

  constructor(private store: AuthenticationStore) {}

  ngOnInit() {
    this.information$ = this.store.information$;
    this.loading$ = this.store.loading$;
    this.isRetrying$ = this.store.isRetrying$;
  }

  dismissInformation() {
    this.store.dismissInformation();
  }
}
