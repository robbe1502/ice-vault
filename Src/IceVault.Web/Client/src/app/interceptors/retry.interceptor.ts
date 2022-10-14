import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable, retry, timer, tap } from 'rxjs';

import { AppStore } from '../store/app.store';

@Injectable()
export class RetryInterceptor implements HttpInterceptor {
  readonly retries: number = 2;
  readonly delay: number = 2000;

  constructor(private store: AppStore) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      retry({
        count: this.retries,
        delay: (_, attempts) => {
          if (attempts === 1) this.store.setRetrying(true);
          return timer(this.delay);
        }
      })
    );
  }
}
