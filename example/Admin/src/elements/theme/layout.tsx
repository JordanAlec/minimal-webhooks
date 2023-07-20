import {
  ReactNode,
  useState,
} from 'react';

import Content from '@/elements/theme/content';
import defaultThemeOptions from '@/elements/theme/default-theme-options.json';
import Header from '@/elements/theme/header';
import Navigation from '@/elements/theme/navigation';
import { navigationLinks } from '@/elements/theme/navigation-links';
import {
  baseThemeOptions,
  createThemeOnBaseTheme,
} from '@/elements/theme/theme-options-builder';
import { Box } from '@mui/material';
import CssBaseline from '@mui/material/CssBaseline';
import {
  createTheme,
  ThemeProvider,
} from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';

type Props = {
  appName: string;
  currentPage: string;
  contentChildren: ReactNode;
}

const Layout = ({appName, currentPage, contentChildren}: Props) => {
  const theme = createTheme(baseThemeOptions);

  const [mobileOpen, setMobileOpen] = useState(false);
  const isSmUp = useMediaQuery(theme.breakpoints.up('sm'));

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  return (
    <ThemeProvider theme={createThemeOnBaseTheme(theme)}>
      <Box sx={{ display: 'flex', minHeight: '100vh' }}>
        <CssBaseline />
        <Box
          component='nav'
          sx={{ width: { sm: defaultThemeOptions.navigation.drawerWidth }, flexShrink: { sm: 0 } }}
        >
          {isSmUp ? null : (
            <Navigation
              drawerProps={{
                PaperProps: {
                  style: { width: defaultThemeOptions.navigation.drawerWidth }
                },
                variant: 'temporary',
                open: mobileOpen,
                onClose: handleDrawerToggle
              }}
              appName={appName}
              navigationLinks={navigationLinks()}
              currentPage={currentPage}
            />
          )}
          <Navigation
            drawerProps={{
              PaperProps: {
                style: { width: defaultThemeOptions.navigation.drawerWidth }
              },
              sx: {
                display: { sm: 'block', xs: 'none' }
              }
            }}
            appName={appName}
            navigationLinks={navigationLinks()}
            currentPage={currentPage}
          />
        </Box>
        <Box sx={{ flex: 1, display: 'flex', flexDirection: 'column' }}>
          <Header onDrawerToggle={handleDrawerToggle} currentPage={currentPage} />
          <Box component='main' sx={{ flex: 1, py: 6, px: 4, bgcolor: defaultThemeOptions.content.background }}>
            <Content contentChildren={contentChildren} />
          </Box>
        </Box>
      </Box>
    </ThemeProvider>
  );
}

export default Layout;