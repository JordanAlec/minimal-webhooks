import axios from 'axios';
import useSWR from 'swr';

import DataLoader from '@/elements/components/data-loader';
import ClientsDataTable
  from '@/elements/components/webhook-clients/clients-data-table';
import {
  WebhookClientsResponse,
} from '@/types/webhooks/webhook-clients-response';
import { Box } from '@mui/material';

const Clients = () => {
  const { data, error, isLoading } = useSWR('/api/clients', axios.get<WebhookClientsResponse>);

  return (
    <Box>
      <DataLoader error={error} isLoading={isLoading}>
        <ClientsDataTable clients={data?.data.data} />
      </DataLoader>
    </Box>
  )
}

Clients.currentPage = 'Clients';


export default Clients;