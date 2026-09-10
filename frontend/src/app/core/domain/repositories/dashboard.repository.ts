import { Observable } from 'rxjs';

import { DashboardDto } from '../models/dashboard.model';

/**
 * Dashboard işlemleri için domain port'u.
 */
export abstract class DashboardRepository {
  abstract getStats(): Observable<DashboardDto>;
}
