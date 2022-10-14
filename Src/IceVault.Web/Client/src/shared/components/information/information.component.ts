import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

export type InformationStyle = 'info' | 'warning' | 'error' | 'success';

@Component({
  selector: 'ice-vault-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class InformationComponent {
  @Input() style: InformationStyle = 'info';
  @Input() message!: string;

  @Output() dismissed = new EventEmitter();
}
