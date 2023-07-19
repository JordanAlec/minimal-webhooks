import { WebhookClient } from '@/types/webhooks/webhook-client';
import { actionTypeConversion } from '@/utils/webhook-client-helper';
import PlayArrowIcon from '@mui/icons-material/PlayArrow';
import {
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from '@mui/material';

type Props = {
    clients?: WebhookClient[]
}
const ClientsDataTable = (props : Props) => {
 const data = props.clients ? props.clients : [];

  return (
    <TableContainer component={Paper}>
        <Table sx={{ minWidth: 250 }}>
            <TableHead>
                <TableRow>
                    <TableCell></TableCell>
                  <TableCell>Name</TableCell>
                  <TableCell>Entity Subscribed</TableCell>
                  <TableCell>Action</TableCell>
                  <TableCell>Url</TableCell>
                  <TableCell></TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {data.map((client) => (
                  <TableRow
                    key={client.id}
                    sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                  >
                    <TableCell></TableCell>
                    <TableCell component="th" scope="row">{client.name}</TableCell>
                    <TableCell component="th">{client.entityTypeName}</TableCell>
                    <TableCell component="th">{actionTypeConversion(client.actionType)}</TableCell>
                    <TableCell component="th">{client.webhookUrl}</TableCell>
                    <TableCell><IconButton href={`/clients/${client.id}`}><PlayArrowIcon /></IconButton></TableCell>
                  </TableRow>
                ))}
            </TableBody>
        </Table>
    </TableContainer>
  )
}


export default ClientsDataTable;