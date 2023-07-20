import Error from '@/elements/components/error';
import ClientSummaryDetails
  from '@/elements/components/webhook-clients/client-summary-details';
import DisableClient
  from '@/elements/components/webhook-clients/disable-client';
import EnableClient from '@/elements/components/webhook-clients/enable-client';
import { WebhookClient } from '@/types/webhooks/webhook-client';
import {
  Grid,
  Paper,
  Stack,
  Typography,
} from '@mui/material';

import ClientHeadersTableElevated from './client-headers-table-elevated';

type Props = {
    client: WebhookClient
}

const ClientSummary = ({client}: Props) => {
  const clientDisabledWarning = client.disabled ? <Error title="Client Disabled" bodyText="" /> : <></>;


  return (
    <Grid container spacing={2} sx={{padding: 2}}>
      <Grid item xs={12}>
        {clientDisabledWarning}
        <Typography variant='h5' component='h1' color='text.secondary' align='center' sx={{marginTop: 1, marginBottom: 2}}>Client Details</Typography>
        
      </Grid>
        <Grid item xs={7}>
          <Paper elevation={3} sx={{padding:0.5}}>
            <Typography color='text.secondary' align="center" sx={{marginBottom: 2}}>Summary</Typography>
            <ClientSummaryDetails client={client} />
          </Paper>
        </Grid>
        <Grid item xs={5}>
          <ClientHeadersTableElevated titleHeader='Headers' clientHeaders={client.clientHeaders} />
        </Grid>
        <Grid item xs={12}>
          <Typography color='text.secondary' align="center" sx={{marginBottom: 2}}>Actions</Typography>
          <Stack spacing={1}>
            <DisableClient id={client.id} />
            <EnableClient id={client.id} />
          </Stack>
        </Grid>
    </Grid>
  )
}


export default ClientSummary;