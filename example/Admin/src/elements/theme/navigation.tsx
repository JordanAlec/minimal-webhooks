import Link from 'next/link';

import defaultThemeOptions from '@/elements/theme/default-theme-options.json';
import { NavigationLinks } from '@/types/elements/navigation-links';
import Box from '@mui/material/Box';
import Divider from '@mui/material/Divider';
import Drawer, { DrawerProps } from '@mui/material/Drawer';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';

type Props = {
  drawerProps: DrawerProps;
  appName: string;
  navigationLinks: NavigationLinks[];
  currentPage: string;
}

const Navigation = (props: Props) => {

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
    <Drawer variant='permanent' {...props.drawerProps}>
      <List disablePadding>
        <ListItem sx={{ ...mavigationItemDefaultSx, ...navigationHeaderDefaultSx, fontSize: 22, color: defaultThemeOptions.navigation.nonLinkItemTextColour }}>
          {props.appName}
        </ListItem>
        {props.navigationLinks.map(({ id, children }) => (
          <Box key={id} sx={{ bgcolor: defaultThemeOptions.navigation.navigationLinkBackgroundColour }}>
            <ListItem sx={{ py: 2, px: 3 }}>
              <ListItemText sx={{ color: defaultThemeOptions.navigation.linkItemTextColour }}>{id}</ListItemText>
            </ListItem>
            {children.map(({ id: childId, url, icon }) => (
              <ListItem disablePadding key={childId}>
                <ListItemButton selected={childId == props.currentPage} sx={mavigationItemDefaultSx}>
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