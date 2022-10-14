import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({ providedIn: 'root' })
export class TranslationService {
  constructor(private service: TranslateService) {}

  setDefaultLanguage(language: string) {
    this.service.setDefaultLang(language);
  }

  use(language: string) {
    this.service.use(language);
  }

  translate(key: string, data?: any) {
    return this.service.instant(key, data);
  }
}
