import { ReactNode } from 'react';

import defaultThemeOptions from '@/elements/theme/default-theme-options.json';
import Paper from '@mui/material/Paper';

type Props = {
  toolBarChildren?: ReactNode;
  contentChildren: ReactNode;
}
const Content = (props: Props) => {
  return (
    <Paper sx={{ maxWidth: defaultThemeOptions.content.maxWidth, margin: 'auto', overflow: 'hidden' }}>
      {props.contentChildren}
    </Paper>
  );
}

export default Content;