import { InjectionToken } from '@angular/core';

export const SESSION_STORAGE = new InjectionToken('SESSION_STORAGE', {
  providedIn: 'root',
  factory: () => window && window.sessionStorage
});
