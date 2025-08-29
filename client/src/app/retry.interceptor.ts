import { HttpErrorResponse, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { retry, timer } from 'rxjs';

// These variables have to stay outside the class because of 'this' keyword binding issues
const maxRetries = 2;
const delayMs = 2000;

@Injectable()
export class RetryInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<unknown>, next: HttpHandler) {
    // Pass the request to the next request handler and retry HTTP 0 errors 2 times with a 2-second delay
    return next.handle(request).pipe(retry({ count: maxRetries, delay: this.delayNotifier }));
  }

  delayNotifier(error: any, retryCount: number) {
    if (error instanceof HttpErrorResponse && error.status === 0) {
      return timer(delayMs);
    }

    // re-throw other errors for anything else that might be catching them
    throw error;
  }
}