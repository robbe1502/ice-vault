import { Injectable } from '@angular/core';

// Store
import { Store } from '../../store';

// Services
import { StorageService } from '../../shared/services/storage.service';

// Models
import { Login } from '../models/login.model';
import { User } from '../models/user.model';

@Injectable({ providedIn: 'root' })
export class AuthenticationStore extends Store {
  constructor(private storage: StorageService) {
    super();
  }

  login(response: Login) {
    const { accessToken, refreshToken, expiresIn } = response;
    this.storage.save('ice-vault.access_token', accessToken);
    this.storage.save('ice-vault.refresh_token', refreshToken);
    this.storage.save('ice-vault.expires_in', expiresIn.toString());
  }

  setUser(user: User) {
    this.commit('user', { ...user });
  }
}
