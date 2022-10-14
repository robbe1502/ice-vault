import { registerLocaleData } from '@angular/common';
import { LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule, Routes } from '@angular/router';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

// Locales
import enGb from '@angular/common/locales/en-GB';
registerLocaleData(enGb);

// Containers
import { AppComponent } from './containers/app/app.component';

// Services
import { TranslationService } from '../shared/services/translation.service';

// Interceptors
import { LoadingInterceptor } from './interceptors/loading.interceptor';
import { RetryInterceptor } from './interceptors/retry.interceptor';

// Store
import { AppStore } from './store/app.store';

// Routes
const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('../authentication/authentication.module').then((m) => m.AuthenticationModule)
  }
];

// Factories
const translationLoaderFactory: (http: HttpClient) => TranslateHttpLoader = (http: HttpClient) => new TranslateHttpLoader(http, 'assets/i18n/');

@NgModule({
  imports: [
    BrowserModule,
    RouterModule.forRoot(routes),
    HttpClientModule,
    TranslateModule.forRoot({ loader: { provide: TranslateLoader, useFactory: translationLoaderFactory, deps: [HttpClient] }, useDefaultLang: true })
  ],
  declarations: [AppComponent],
  providers: [
    { provide: LOCALE_ID, useValue: 'en-GB' },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: RetryInterceptor, multi: true },
    TranslationService,
    AppStore
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
