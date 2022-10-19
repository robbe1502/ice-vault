import { Injectable, Inject } from '@angular/core';

// Tokens
import { SESSION_STORAGE } from '../tokens/tokens';

type StorageKey = 'ice-vault.access_token' | 'ice-vault.refresh_token' | 'ice-vault.expires_in';

@Injectable({ providedIn: 'root' })
export class StorageService {
  constructor(@Inject(SESSION_STORAGE) private store: Storage) {}

  save(key: StorageKey, value: string) {
    this.store.setItem(key, value);
  }

  get<T>(key: StorageKey) {
    const result = this.store.getItem(key);
    if (result === null) return null;
    return result as unknown as T;
  }
}
