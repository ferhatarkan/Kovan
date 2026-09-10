import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { DashboardRepository } from '../../domain/repositories/dashboard.repository';
import { DashboardDto } from '../../domain/models/dashboard.model';
import { ApiBase } from '../api/api-base';

/**
 * DashboardRepository'nin REST API implementasyonu.
 * Backend endpoint'leri: /api/v1/dashboard
 */
@Injectable({ providedIn: 'root' })
export class DashboardApiRepository extends ApiBase implements DashboardRepository {
  private readonly versionedBase = `${this.baseUrl}/v1`;

  getStats(): Observable<DashboardDto> {
    return this.toApiError(this.http.get<DashboardDto>(`${this.versionedBase}/dashboard/stats`));
  }
}
