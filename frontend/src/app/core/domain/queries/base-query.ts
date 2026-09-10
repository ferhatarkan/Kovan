import { Observable } from 'rxjs';

export interface IQuery<TRequest = void, TResult = void> {
  execute(request: TRequest): Observable<TResult>;
}