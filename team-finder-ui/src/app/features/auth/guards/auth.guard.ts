import { Inject, inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router, RouterStateSnapshot } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from '../services/auth.service';
import {jwtDecode} from "jwt-decode";
import { SharedService } from 'src/app/shared/shared.service';

// Common function to validate token and user roles
const validateUserRole = (roles: string[], route: ActivatedRouteSnapshot, state: RouterStateSnapshot, router: Router, authService: AuthService, cookieService: CookieService): boolean | any => {
  const sharedService = inject(SharedService)
  const user = authService.getUser();
  const token = cookieService.get('Authorization')?.replace('Bearer', '');

  if (token && user) {
    const decodedToken: any = jwtDecode(token);

    // Check if token has expired
    const expirationDate = decodedToken.exp * 1000;
    const currentTime = new Date().getTime();

    if (expirationDate < currentTime) {
      // Logout
      authService.logout();
      return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
    } else {
      // Token is still valid
      if (roles.some(role => user.roles.includes(role))) {
        return true;
      } else {
        
        return false;
      }
    }
  } else {
    // Logout
    authService.logout();
    return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
  }
};

// Auth Guard for Admin role
export const adminAuthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const cookieService = inject(CookieService);
  return validateUserRole(['Admin'], route, state, router, authService, cookieService);
};

// Auth Guard for Organizer role
export const organizerAuthGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const cookieService = inject(CookieService);
  return validateUserRole(['Organizer', 'Admin'], route, state, router, authService, cookieService);
};
