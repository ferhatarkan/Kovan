import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { DashboardRepository } from '../repositories/dashboard.repository';
import { DashboardDto } from '../models/dashboard.model';

@Injectable({ providedIn: 'root' })
export class GetDashboardStatsUseCase {
  private readonly dashboardRepository = inject(DashboardRepository);

  execute(): Observable<DashboardDto> {
    return this.dashboardRepository.getStats();
  }
}
