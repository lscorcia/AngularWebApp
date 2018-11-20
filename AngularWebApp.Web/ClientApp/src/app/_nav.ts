export var navTree = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'icon-speedometer',
    roles: ['*']
  },
  {
    title: true,
    name: 'Weather',
    roles: ['*']
  },
  {
    name: 'Weather',
    url: '/weather',
    icon: 'icon-drop',
    roles: ['*']
  },
  {
    title: true,
    name: 'Orders',
    roles: ['Super User']
  },
  {
    name: 'Orders',
    url: '/orders',
    icon: 'icon-notebook',
    badge: {
      variant: 'info',
      text: 'NEW'
    },
    roles: ['Super User']
  },
  {
    title: true,
    name: 'Settings',
    roles: []
  },
  {
    name: 'Users',
    icon: 'icon-people',
    roles: [],
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
