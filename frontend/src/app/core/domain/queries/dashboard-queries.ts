import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { DashboardRepository } from '../repositories/dashboard.repository';
import { DashboardDto } from '../models/dashboard.model';
import { IQuery } from './base-query';

@Injectable({ providedIn: 'root' })
export class GetDashboardStatsQuery implements IQuery<void, DashboardDto> {
  private readonly dashboardRepository = inject(DashboardRepository);

  execute(): Observable<DashboardDto> {
    return this.dashboardRepository.getStats();
  }
}
