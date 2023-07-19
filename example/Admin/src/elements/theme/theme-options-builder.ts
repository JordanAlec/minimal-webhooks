import defaultThemeOptions from '@/elements/theme/default-theme-options.json';
import {
  Theme,
  ThemeOptions,
} from '@mui/material/styles';

export const baseThemeOptions: ThemeOptions = {
    palette: {
        primary: {
          main: defaultThemeOptions.shared.colours.primary.main,
          light: defaultThemeOptions.shared.colours.primary.light,
          dark: defaultThemeOptions.shared.colours.primary.dark,
        },
      secondary: {
        main: defaultThemeOptions.shared.colours.secondary.main,
          light: defaultThemeOptions.shared.colours.secondary.light,
          dark: defaultThemeOptions.shared.colours.secondary.dark,
      }
    },
    typography: {
        h5: {
          fontWeight: 500,
          fontSize: 26,
          letterSpacing: 0.5,
        },
    },
    shape: {
      borderRadius: 8,
    },
    components: {
      MuiTab: {
        defaultProps: {
          disableRipple: true,
        },
      }
    },
    mixins: {
      toolbar: {
        minHeight: 48,
      },
    }
}
export const createThemeOnBaseTheme = (baseTheme: Theme): Theme => {
    return {
      ...baseTheme,
      components: {
        MuiDrawer: {
          styleOverrides: {
            paper: {
              backgroundColor: defaultThemeOptions.navigation.drawerBackgroundColour,
            },
          },
        },
        MuiButton: {
          styleOverrides: {
            root: {
              textTransform: 'none',
            },
            contained: {
              boxShadow: 'none',
              '&:active': {
                boxShadow: 'none',
              },
            },
          },
        },
        MuiTabs: {
          styleOverrides: {
            root: {
              marginLeft: baseTheme.spacing(1),
            },
            indicator: {
              height: 3,
              borderTopLeftRadius: 3,
              borderTopRightRadius: 3,
              backgroundColor: baseTheme.palette.common.white,
            },
          },
        },
        MuiTab: {
          styleOverrides: {
            root: {
              textTransform: 'none',
              margin: '0 16px',
              minWidth: 0,
              padding: 0,
              [baseTheme.breakpoints.up('md')]: {
                padding: 0,
                minWidth: 0,
              },
            },
          },
        },
        MuiIconButton: {
          styleOverrides: {
            root: {
              padding: baseTheme.spacing(1),
            },
          },
        },
        MuiTooltip: {
          styleOverrides: {
            tooltip: {
              borderRadius: 4,
            },
          },
        },
        MuiDivider: {
          styleOverrides: {
            root: {
              backgroundColor: defaultThemeOptions.navigation.navigationLinkDividerColour,
            },
          },
        },
        MuiListItemButton: {
          styleOverrides: {
            root: {
              '&.Mui-selected': {
                color: defaultThemeOptions.navigation.linkItemIconSelectedColour,
              },
            },
          },
        },
        MuiListItemText: {
          styleOverrides: {
            primary: {
              fontSize: 14,
              fontWeight: baseTheme.typography.fontWeightMedium,
            },
          },
        },
        MuiListItemIcon: {
          styleOverrides: {
            root: {
              color: 'inherit',
              minWidth: 'auto',
              marginRight: baseTheme.spacing(2),
              '& svg': {
                fontSize: 20,
              },
            },
          },
        },
        MuiAvatar: {
          styleOverrides: {
            root: {
              width: 32,
              height: 32,
            },
          },
        },
      },
    }
}