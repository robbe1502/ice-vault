import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, tap } from 'rxjs';

// Store
import { AuthenticationStore } from '../store/authentication.store';

// Services
import { DataService } from '../../shared/services/data.service';
import { TranslationService } from '../../shared/services/translation.service';

// Models
import { Login } from '../models/login.model';

@Injectable({ providedIn: 'root' })
export class AuthenticationService extends DataService {
  private baseUrl = '/api/v1/auth';

  constructor(private client: HttpClient, private translationService: TranslationService, private store: AuthenticationStore) {
    super(translationService);
  }

  login(email: string, password: string) {
    const url = `${this.baseUrl}/login`;
    return this.client.post<Login>(url, { email, password }).pipe(
      tap((response) => this.store.login(response)),
      catchError(() => this.handleError('login.failed'))
    );
  }
}
