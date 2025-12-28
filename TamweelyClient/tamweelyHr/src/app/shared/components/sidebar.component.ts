import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  isAuthenticated$: Observable<boolean>;
  isAdmin$: Observable<boolean>;

  constructor(private authService: AuthService) {
    this.isAuthenticated$ = this.authService.authState$.pipe(
      map((state: any) => state.isAuthenticated)
    );
    this.isAdmin$ = this.authService.authState$.pipe(
      map((state: any) => state.roles.includes('Admin'))
    );
  }
}
