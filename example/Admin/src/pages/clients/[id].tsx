import axios from 'axios';
import { useRouter } from 'next/router';
import useSWR from 'swr';

import DataLoader from '@/elements/components/data-loader';
import ClientSummary
  from '@/elements/components/webhook-clients/client-summary';
import { WebhookClient } from '@/types/webhooks/webhook-client';
import {
  WebhookClientsResponse,
} from '@/types/webhooks/webhook-clients-response';
import { Box } from '@mui/material';

const ClientDetail = () => {
  const router = useRouter();
  const { id } = router.query;

  const { data, error, isLoading } = useSWR(`/api/clients/${id ? id : "0"}`, axios.get<WebhookClientsResponse>);
  const client = data?.data?.data ? data?.data?.data[0] : {} as WebhookClient;

  return (
    <Box>
      <DataLoader error={error} isLoading={isLoading}>
        <ClientSummary client={client} />
      </DataLoader>
    </Box>
  )
}

ClientDetail.currentPage = 'Clients';


export default ClientDetail;