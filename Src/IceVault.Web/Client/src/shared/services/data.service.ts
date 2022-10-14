import { throwError } from 'rxjs';

import { TranslationService } from './translation.service';

export abstract class DataService {
  constructor(protected translateService: TranslationService) {}

  protected handleError(translationKey: string, data?: any) {
    const message = this.translateService.translate(translationKey, data);
    return throwError(() => new Error(message));
  }
}
