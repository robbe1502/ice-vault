import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';

// Services
import { StorageService } from '../../shared/services/storage.service';

@Injectable()
export class AuthenticationGuard implements CanActivate {
  constructor(private storage: StorageService, private router: Router) {}

  canActivate() {
    const accessToken = this.storage.get<string>('ice-vault.access_token');
    const refreshToken = this.storage.get<string>('ice-vault.refresh_token');
    const expiresIn = this.storage.get<number>('ice-vault.expires_in');

    if (accessToken === null || refreshToken === null || expiresIn === null) return this.router.navigateByUrl('/auth/login');

    return true;
  }
}
