import { WebhookClientHeader } from '@/types/webhooks/webhook-client-header';
import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from '@mui/material';

type Props = {
    clientHeaders?: WebhookClientHeader[]
}
const ClientHeadersTable = (props : Props) => {
 const data = props.clientHeaders ? props.clientHeaders : [];

  return (
    <TableContainer component={Paper}>
        <Table sx={{ minWidth: 250 }}>
            <TableHead>
                <TableRow>
                    <TableCell></TableCell>
                  <TableCell>Key</TableCell>
                  <TableCell>Value</TableCell>
                  <TableCell></TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {data.map((clientHeader) => (
                  <TableRow
                    key={clientHeader.id}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell></TableCell>
                    <TableCell component="th" scope="row">{clientHeader.key}</TableCell>
                    <TableCell component="th">{clientHeader.value}</TableCell>
                    <TableCell></TableCell>
                  </TableRow>
                ))}
            </TableBody>
        </Table>
    </TableContainer>
  )
}


export default ClientHeadersTable;