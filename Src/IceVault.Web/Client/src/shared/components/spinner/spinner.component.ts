import { ChangeDetectionStrategy, Component, Input } from '@angular/core';

@Component({
  selector: 'ice-vault-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class SpinnerComponent {
  @Input() isRetrying!: boolean;
}
