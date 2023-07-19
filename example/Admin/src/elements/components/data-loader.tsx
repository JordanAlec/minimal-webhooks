import {
  ReactNode,
  useMemo,
} from 'react';

import Error from '@/elements/components/error';
import Loading from '@/elements/components/loading';
import { Box } from '@mui/material';

type Props = {
    error: any,
    isLoading: boolean
    children: ReactNode;
}

const DataLoader = (props: Props) => {

  const element = useMemo(() => {
    if (props.isLoading) return <Box sx={{ my: 5, mx: 2}}><Loading /></Box>
    if (props.error) return <Box sx={{ my: 5, mx: 2}}><Error title={'Data fetch'} bodyText={props.error.message} /></Box>
    return props.children
  },[props.children, props.error, props.isLoading]);

  return element;
}


export default DataLoader;