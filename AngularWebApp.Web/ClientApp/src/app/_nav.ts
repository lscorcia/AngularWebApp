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
    name: 'Refresh Tokens',
    url: '/refreshTokens',
    icon: 'icon-login'
  }
];
