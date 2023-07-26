import ClientActivityLogTable
  from '@/elements/components/webhook-clients/client-activity-log-table';
import {
  WebhookClientActivityLog,
} from '@/types/webhooks/webhook-client-activity-log';
import {
  Paper,
  SxProps,
  Theme,
  Typography,
} from '@mui/material';

type Props = {
    titleHeader: string,
    clientActivityLogs?: WebhookClientActivityLog[],
    additionalSx?: SxProps<Theme>
}
const ClientActivityLogTableElevated = ({titleHeader, clientActivityLogs, additionalSx} : Props) => {
  return (
    <Paper elevation={1} sx={{...additionalSx, padding: 0.5}}>
      <Typography color='text.secondary' align="center" sx={{marginBottom: 2}}>{titleHeader}</Typography>
      <ClientActivityLogTable clientActivityLogs={clientActivityLogs} />
    </Paper>
  )
}


export default ClientActivityLogTableElevated;