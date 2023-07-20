import axios from 'axios';

import CallApiButton from '@/elements/components/call-api-button';

type Props = {
    id: number
}

const DisableClient = ({id}: Props) => {
  return (
    <CallApiButton buttonColor='error' action={async () => await axios.delete(`/api/clients/disable/${id}`)} buttonText='Disable'/>
  )
}


export default DisableClient;