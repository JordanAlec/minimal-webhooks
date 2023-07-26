import ConfigurableTable from '@/elements/components/configurable-table';
import {
  WebhookClientActivityLog,
} from '@/types/webhooks/webhook-client-activity-log';
import { logTypeConversion } from '@/utils/webhook-client-helper';
import {
  TableCell,
  TableRow,
} from '@mui/material';

type Props = {
  clientActivityLogs?: WebhookClientActivityLog[]
}
    
const ClientActivityLogTable = ({clientActivityLogs}: Props) => {
  const data = clientActivityLogs ?? [];
  return (
    <ConfigurableTable
    tableMinWidth={250}
    tableHeaders={
      <TableRow>
        <TableCell></TableCell>
        <TableCell>Timestamp</TableCell>
        <TableCell>Type</TableCell>
        <TableCell>Log</TableCell>
        <TableCell></TableCell>
      </TableRow>
    } 
    tableDataRows={
      <>
        {data.map((clientLog) => (
          <TableRow
            key={clientLog.id}
            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
          >
            <TableCell></TableCell>
            <TableCell component="th" scope="row">{clientLog.timeStamp}</TableCell>
            <TableCell component="th">{logTypeConversion(clientLog.logType)}</TableCell>
            <TableCell component="th">{clientLog.log}</TableCell>
            <TableCell></TableCell>
          </TableRow>
        ))}
      </>
    } />
  )
}


export default ClientActivityLogTable;