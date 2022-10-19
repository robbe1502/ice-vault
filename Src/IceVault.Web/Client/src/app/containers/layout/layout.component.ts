import { Component, OnInit } from '@angular/core';
import { takeUntil } from 'rxjs';

// Components
import { BaseComponent } from '../../../shared/components/base/base.component';

// Services
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'ice-vault-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent extends BaseComponent implements OnInit {
  constructor(private notifications: NotificationService) {
    super();
  }

  ngOnInit() {
    this.notifications.getAll().pipe(takeUntil(this.destroy$)).subscribe();
  }
}
