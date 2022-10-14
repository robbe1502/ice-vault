import { Component, OnInit } from '@angular/core';

// Store
import { AppStore } from '../../store/app.store';

// Services
import { TranslationService } from '../../../shared/services/translation.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  constructor(private store: AppStore, private translateService: TranslationService) {}

  ngOnInit() {
    this.translateService.setDefaultLanguage(this.store.value.defaultLocale);
    this.translateService.use(this.store.value.locale);
  }
}
