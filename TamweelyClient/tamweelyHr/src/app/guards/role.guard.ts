import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return false;
    }

    const requiredRoles = route.data['roles'] as string[];
    const userRoles = this.authService.getRoles();

    if (requiredRoles && requiredRoles.length > 0) {
      const hasRole = requiredRoles.some(role => userRoles.includes(role));
      if (hasRole) {
        return true;
      }
      this.router.navigate(['/unauthorized']);
      return false;
    }

    return true;
  }
}
