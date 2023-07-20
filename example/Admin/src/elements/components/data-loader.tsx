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

const DataLoader = ({error, isLoading, children}: Props) => {

  const element = useMemo(() => {
    if (isLoading) return <Box sx={{ my: 5, mx: 2}}><Loading /></Box>
    if (error) return <Box sx={{ my: 5, mx: 2}}><Error title={'Data fetch'} bodyText={error.message} /></Box>
    return children
  },[children, error, isLoading]);

  return element;
}


export default DataLoader;