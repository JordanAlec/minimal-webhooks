import Link from 'next/link';

import defaultThemeOptions from '@/elements/theme/default-theme-options.json';
import { NavigationLinks } from '@/types/elements/navigation-links';
import {
  Box,
  Divider,
  Drawer,
  DrawerProps,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
} from '@mui/material';

type Props = {
  drawerProps: DrawerProps;
  appName: string;
  navigationLinks: NavigationLinks[];
  currentPage: string;
}

const Navigation = ({drawerProps, appName, navigationLinks, currentPage}: Props) => {

  const mavigationItemDefaultSx = {
    py: '2px',
    px: 3,
    color: defaultThemeOptions.navigation.linkItemTextColour,
    '&:hover, &:focus': {
      bgcolor: defaultThemeOptions.navigation.linkItemTextHoverFocusColour,
    }
  }

  const navigationHeaderDefaultSx = {
    boxShadow: '0 -1px 0 rgb(255,255,255,0.1) inset',
    py: 1.5,
    px: 3,
  }

  return (
    <Drawer variant='permanent' {...drawerProps}>
      <List disablePadding>
        <ListItem sx={{ ...mavigationItemDefaultSx, ...navigationHeaderDefaultSx, fontSize: 22, color: defaultThemeOptions.navigation.nonLinkItemTextColour }}>
          {appName}
        </ListItem>
        {navigationLinks.map(({ id, children }) => (
          <Box key={id} sx={{ bgcolor: defaultThemeOptions.navigation.navigationLinkBackgroundColour }}>
            <ListItem sx={{ py: 2, px: 3 }}>
              <ListItemText sx={{ color: defaultThemeOptions.navigation.linkItemTextColour }}>{id}</ListItemText>
            </ListItem>
            {children.map(({ id: childId, url, icon }) => (
              <ListItem disablePadding key={childId}>
                <ListItemButton selected={childId == currentPage} sx={mavigationItemDefaultSx}>
                  <ListItemIcon>{icon}</ListItemIcon>
                  <ListItemText><Link href={url} style={{textDecoration: 'none', color: defaultThemeOptions.navigation.linkItemTextColour}}>{childId}</Link></ListItemText>
                </ListItemButton>
              </ListItem>
            ))}
            <Divider sx={{ mt: 2 }} />
          </Box>
        ))}
      </List>
    </Drawer>
  );
}

export default Navigation;