import { ChangeDetectionStrategy, Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs';

// Components
import { BaseComponent } from '../../../shared/components/base/base.component';

// Services
import { AuthenticationService } from '../../services/authentication.service';

// Store
import { AuthenticationStore } from '../../store/authentication.store';

@Component({
  selector: 'ice-vault-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoginComponent extends BaseComponent {
  form = this.builder.group({
    email: this.builder.control<string | null>(null, [Validators.required, Validators.email]),
    password: this.builder.control<string | null>(null, [Validators.required])
  });

  constructor(private builder: FormBuilder, private store: AuthenticationStore, private service: AuthenticationService) {
    super();
  }

  onSubmit() {
    if (!this.form.valid) return;

    const email: string | null = this.form.controls.email.value;
    const password: string | null = this.form.controls.password.value;

    if (email === null || password === null) return;

    this.service
      .login(email, password)
      .pipe(takeUntil(this.destroy$))
      .subscribe({ error: (error) => this.store.setInformation(error.message, 'error') });
  }
}
