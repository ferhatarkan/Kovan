import { Observable } from 'rxjs';

export interface ICommand<TRequest = void, TResult = void> {
  execute(request: TRequest): Observable<TResult>;
}