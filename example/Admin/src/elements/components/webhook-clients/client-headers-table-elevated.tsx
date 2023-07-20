import ClientHeadersTable
  from '@/elements/components/webhook-clients/client-headers-table';
import { WebhookClientHeader } from '@/types/webhooks/webhook-client-header';
import {
  Paper,
  SxProps,
  Theme,
  Typography,
} from '@mui/material';

type Props = {
    titleHeader: string,
    clientHeaders?: WebhookClientHeader[],
    additionalSx?: SxProps<Theme>
}
const ClientHeadersTableElevated = ({titleHeader, clientHeaders, additionalSx} : Props) => {
  return (
    <Paper elevation={1} sx={{...additionalSx, padding: 0.5}}>
      <Typography color='text.secondary' align="center" sx={{marginBottom: 2}}>{titleHeader}</Typography>
      <ClientHeadersTable clientHeaders={clientHeaders} />
    </Paper>
  )
}


export default ClientHeadersTableElevated;