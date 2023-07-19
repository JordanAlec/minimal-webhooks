import { NavigationLinks } from '@/types/elements/navigation-links';
import PeopleIcon from '@mui/icons-material/People';

export const navigationLinks = (): NavigationLinks[] => {
    return [
        {
          id: 'Home',
          children: [
            {
              id: 'Home',
              url: '/',
              icon: <PeopleIcon />
            }
          ]
        },
        {
          id: 'Clients',
          children: [
            {
              id: 'Clients',
              url: '/clients',
              icon: <PeopleIcon />
            }
          ]
        }
    ]
}