import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class NotificationService {
  private baseUrl = '/api/v1/notifications';

  constructor(private client: HttpClient) {}

  getAll() {
    return this.client.get<Array<any>>(this.baseUrl);
  }
}
