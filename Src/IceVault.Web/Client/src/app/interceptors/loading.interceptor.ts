import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { finalize, Observable } from 'rxjs';

import { AppStore } from '../store/app.store';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
  private total = 0;

  constructor(private store: AppStore) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.total = this.total + 1;
    this.store.setLoading(true);
    return next.handle(req).pipe(
      finalize(() => {
        this.total = this.total - 1;
        if (this.total === 0) {
          this.store.setLoading(false);
          this.store.setRetrying(false);
        }
      })
    );
  }
}
