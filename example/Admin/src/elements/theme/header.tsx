import { Fragment } from 'react';

import defaultThemeOptions from '@/elements/theme/default-theme-options.json';
import MenuIcon from '@mui/icons-material/Menu';
import {
  AppBar,
  Avatar,
  Grid,
  IconButton,
  Toolbar,
  Typography,
} from '@mui/material';

type Props = {
  onDrawerToggle: () => void;
  currentPage: string;
  currentPageTabs?: string[];
}

const Header = (props: Props) => {

  return (
    <Fragment>
      <AppBar color='primary' position='sticky' elevation={0}>
        <Toolbar>
          <Grid container spacing={1} alignItems='center'>
            <Grid sx={{ display: { sm: 'none', xs: 'block' } }} item>
              <IconButton
                color='inherit'
                aria-label='open drawer'
                onClick={props.onDrawerToggle}
                edge='start'
              >
                <MenuIcon />
              </IconButton>
            </Grid>
            <Grid item xs />
            <Grid item>
              <IconButton color='inherit' sx={{ p: 0.5 }}>
                <Avatar 
                  src={defaultThemeOptions.header.defaultAvatar} 
                  alt='My Avatar' />
              </IconButton>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>
      <AppBar
        component='div'
        color='primary'
        position='static'
        elevation={0}
        sx={{ zIndex: 0 }}
      >
        <Toolbar>
          <Grid container alignItems='center' spacing={1} sx={{paddingBottom: 2}}>
            <Grid item xs>
              <Typography color='inherit' variant='h5' component='h1'>
                {props.currentPage}
              </Typography>
            </Grid>
          </Grid>
        </Toolbar>
      </AppBar>
    </Fragment>
  );
}

export default Header;