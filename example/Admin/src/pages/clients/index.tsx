import axios from 'axios';
import useSWR from 'swr';

import {
  WebhookClientsResponse,
} from '@/types/webhooks/webhook-clients-response';
import {
  Box,
  Typography,
} from '@mui/material';

const Clients = () => {
  const { data, error, isLoading } = useSWR('/api/clients/get-clients', axios.get<WebhookClientsResponse>);

  const dataElement = data ? <Box sx={{ my: 5, mx: 2}}><pre>{JSON.stringify(data.data, null, 2) }</pre></Box> : <></>;
  const errorElement = error ? <Typography sx={{ my: 5, mx: 2}} variant='body1'>Error: {error.message}</Typography> : <></>;
  const isLoadingElement = isLoading ? <Typography sx={{ my: 5, mx: 2}}variant='body1'>Loading data...</Typography> : <></>;

  return (
    <Box>
      <Typography sx={{ my: 5, mx: 2 }} color='text.secondary' align='center'>Example call to get clients. Results below</Typography>
      {dataElement}
      {errorElement}
      {isLoadingElement}
    </Box>
  )
}

Clients.currentPage = 'Clients';


export default Clients;