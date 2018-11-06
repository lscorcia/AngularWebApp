export const navItems = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer'
  },
  {
    title: true,
    name: 'Weather'
  },
  {
    name: 'Weather',
    url: '/weather',
    icon: 'icon-drop'
  },
  {
    name: 'Orders',
    url: '/orders',
    icon: 'icon-notebook',
    badge: {
      variant: 'info',
      text: 'NEW'
    }
  },
  {
    title: true,
    name: 'Settings'
  },
  {
    name: 'Users',
    icon: 'icon-people',
    children: [
      {
        name: 'Roles',
        url: '/settings/users/roles',
        icon: 'icon-organization'
      },
      {
        name: 'Users in Roles',
        url: '/settings/users/userRoles',
        icon: 'icon-user-following'
      },
      {
        name: 'Refresh Tokens',
        url: '/settings/users/refreshTokens',
        icon: 'icon-login'
      }
    ]
  }
];
