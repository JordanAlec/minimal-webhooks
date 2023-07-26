import ConfigurableTable from '@/elements/components/configurable-table';
import { WebhookClientHeader } from '@/types/webhooks/webhook-client-header';
import {
  Button,
  TableCell,
  TableRow,
} from '@mui/material';

type Props = {
    clientHeaders?: WebhookClientHeader[]
    removeHeader?: (header: WebhookClientHeader) => void;
}
const ClientHeadersTable = ({clientHeaders, removeHeader} : Props) => {
 const data = clientHeaders ?? [];

  return (
    <ConfigurableTable 
    tableMinWidth={250} 
    tableHeaders={
      <TableRow>
        <TableCell></TableCell>
        <TableCell>Key</TableCell>
        <TableCell>Value</TableCell>
        {removeHeader ? <TableCell></TableCell> : <></>}
        <TableCell></TableCell>
      </TableRow>
    } 
    tableDataRows={
      <>
        {data.map((clientHeader) => (
          <TableRow
            key={clientHeader.id}
            sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
          >
            <TableCell></TableCell>
            <TableCell component="th" scope="row">{clientHeader.key}</TableCell>
            <TableCell component="th">{clientHeader.value}</TableCell>
            {removeHeader ? 
              <TableCell component="th">
                <Button variant="contained" color="error" onClick={() => removeHeader(clientHeader)}>Delete</Button>
              </TableCell> : <></>
            }
            <TableCell></TableCell>
          </TableRow>
        ))}
      </>
    } />
  )
}


export default ClientHeadersTable;