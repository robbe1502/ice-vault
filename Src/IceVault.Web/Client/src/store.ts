import { BehaviorSubject, map } from 'rxjs';

import { InformationStyle } from './shared/components/information/information.component';

export interface AuthenticationState {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
}

export interface InformationState {
  style: InformationStyle;
  message: string;
}

export interface AppState {
  locale: string;
  defaultLocale: string;
  loading: boolean;
  isRetrying: boolean;
  information: InformationState | undefined;
  authentication: AuthenticationState | undefined;
}

const initialState: AppState = {
  locale: 'en-GB',
  defaultLocale: 'en-GB',
  loading: false,
  isRetrying: false,
  information: undefined,
  authentication: undefined
};

export class StoreFactory {
  private static store: BehaviorSubject<AppState> | undefined;

  static getState() {
    if (this.store === undefined) this.store = new BehaviorSubject<AppState>(initialState);
    return this.store;
  }
}

export abstract class Store {
  private store = StoreFactory.getState();

  locale$ = this.state$.pipe(map((state) => state.locale));
  loading$ = this.state$.pipe(map((state) => state.loading));
  information$ = this.state$.pipe(map((state) => state.information));
  isRetrying$ = this.state$.pipe(map((state) => state.isRetrying));

  get state$() {
    return this.store.asObservable();
  }

  get value() {
    return this.store.value;
  }

  commit(key: keyof AppState, state: any) {
    const featureState = this.value[key] as any;
    this.store.next({ ...this.value, [key]: { ...featureState, ...state } });
  }

  setLoading(loading: boolean) {
    this.store.next({ ...this.value, loading });
  }

  setRetrying(retrying: boolean) {
    this.store.next({ ...this.value, isRetrying: retrying });
  }

  setInformation(message: string, style: InformationStyle) {
    this.store.next({ ...this.value, information: { message, style } });
  }

  dismissInformation() {
    this.store.next({ ...this.value, information: undefined });
  }
}
