import { Injectable } from '@angular/core';

// Store
import { Store } from '../../store';

// Models
import { Login } from '../models/login.model';

@Injectable({ providedIn: 'root' })
export class AuthenticationStore extends Store {
  login(response: Login) {
    const { accessToken, refreshToken, expiresIn } = response;
    this.commit('authentication', { accessToken, refreshToken, expiresIn });
  }
}
