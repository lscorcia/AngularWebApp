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
        url: '/roles',
        icon: 'icon-organization'
      },
      {
        name: 'Users in Roles',
        url: '/userRoles',
        icon: 'icon-organization'
      },
      {
        name: 'Refresh Tokens',
        url: '/refreshTokens',
        icon: 'icon-login'
      }
    ]
  }
];
