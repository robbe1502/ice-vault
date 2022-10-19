import { Inject, Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, Observable, of, throwError, switchMap, map } from 'rxjs';

// Tokens
import { SESSION_STORAGE } from '../../shared/tokens/tokens';

// Services
import { AuthenticationService } from '../../authentication/services/authentication.service';

@Injectable()
export class AuthenticationInterceptor implements HttpInterceptor {
  constructor(@Inject(SESSION_STORAGE) private storage: Storage, private service: AuthenticationService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.storage.getItem('ice-vault.access_token');
    if (token === null) return next.handle(req);

    const request = this.getRequest(req, token);
    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === 401) return this.refreshToken(next, request);
        return throwError(() => error);
      })
    );
  }

  private refreshToken(next: HttpHandler, request: HttpRequest<any>) {
    return this.service.refreshToken().pipe(
      map(() => this.storage.getItem('ice-vault.access_token')),
      switchMap((token) => next.handle(this.getRequest(request, token))),
      catchError(() => {
        this.router.navigateByUrl('/auth/login');
        return of();
      })
    );
  }

  private getRequest(request: HttpRequest<any>, token: string | null) {
    return request.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }
}
