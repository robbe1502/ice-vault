import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, switchMap, tap } from 'rxjs';

// Store
import { AuthenticationStore } from '../store/authentication.store';

// Services
import { DataService } from '../../shared/services/data.service';
import { TranslationService } from '../../shared/services/translation.service';
import { StorageService } from '../../shared/services/storage.service';

// Models
import { Login } from '../models/login.model';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthenticationService extends DataService {
  private baseUrl = '/api/v1/auth';

  constructor(
    private client: HttpClient,
    private translationService: TranslationService,
    private store: AuthenticationStore,
    private router: Router,
    private storage: StorageService
  ) {
    super(translationService);
  }

  login(email: string, password: string) {
    const url = `${this.baseUrl}/login`;
    return this.client.post<Login>(url, { email, password }).pipe(
      tap((response) => this.store.login(response)),
      switchMap(() => this.getProfile().pipe(tap(() => this.router.navigateByUrl('/')))),
      catchError(() => this.handleError('login.failed'))
    );
  }

  getProfile() {
    const url = `${this.baseUrl}/profile`;
    return this.client.get<User>(url).pipe(
      tap((response) => this.store.setUser(response)),
      tap((response) => this.translationService.use(response.locale)),
      catchError(() => this.handleError('login.failed'))
    );
  }

  refreshToken() {
    const token = this.storage.get('ice-vault.refresh_token');
    const url = `${this.baseUrl}/refresh/${token}`;

    return this.client.get<Login>(url).pipe(
      tap((response) => this.store.login(response)),
      switchMap(() => this.getProfile()),
      catchError(() => this.router.navigateByUrl('/auth/login'))
    );
  }
}
