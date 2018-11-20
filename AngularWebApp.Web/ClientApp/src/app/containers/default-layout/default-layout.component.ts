import { Component, Input } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';
import { navTree } from './../../_nav';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html'
})
export class DefaultLayoutComponent {
  public navItems: any[];
  public sidebarMinimized = true;
  private changes: MutationObserver;
  public element: HTMLElement = document.body;

  constructor(public authenticationService: AuthenticationService) {
    this.changes = new MutationObserver((mutations) => {
      this.sidebarMinimized = document.body.classList.contains('sidebar-minimized');
    });

    this.changes.observe(<Element>this.element, {
      attributes: true
    });

    var authInfo = authenticationService.AuthInfo;
    this.navItems = this.buildNavigationTree(navTree, (authInfo.Roles || []));
  }

  private buildNavigationTree(navItems: any[], userRoles: string[]): any[] {
    var isAdministrator = this.authenticationService.isAdministrator();

    var userNav = [];

    navItems.forEach(item => {
      if (isAdministrator ||
        item.roles.includes("*") ||
        this.authenticationService.isInAnyRole(item.roles)
      ) {
        userNav.push({
          name: item.name,
          url: item.url,
          icon: item.icon,
          badge: item.badge,
          title: item.title,
          children: item.children ? this.buildNavigationTree(item.children, userRoles): null
        });
      }
    });

    return userNav;
  }
}
