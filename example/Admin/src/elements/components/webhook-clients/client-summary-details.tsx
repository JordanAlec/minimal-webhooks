import ReadOnlyFilledField from '@/elements/components/readonly-filled-field';
import { WebhookClient } from '@/types/webhooks/webhook-client';
import { actionTypeConversion } from '@/utils/webhook-client-helper';
import { Box } from '@mui/material';

type Props = {
    client: WebhookClient
}

const ClientSummaryDetails = ({client}: Props) => {
  return (
    <Box component="form" noValidate autoComplete="off">
         <ReadOnlyFilledField id={"client-id"} label={"Id"} value={client.id} />
         <ReadOnlyFilledField id={"client-name"} label={"Name"} value={client.name} />
         <ReadOnlyFilledField id={"client-entity"} label={"Entity"} value={client.entityTypeName} />
         <ReadOnlyFilledField id={"client-actionType"} label={"Action"} value={actionTypeConversion(client.actionType)} />
         <ReadOnlyFilledField id={"client-url"} label={"Url"} value={client.webhookUrl} />
    </Box>
  )
}


export default ClientSummaryDetails;