import axios from 'axios';

import ActionButton from '@/elements/components/action-button';

type Props = {
    id: number
}

const DisableClient = ({id}: Props) => {
  return (
    <ActionButton buttonColor='error' action={async () => await axios.delete(`/api/clients/disable/${id}`)} buttonText='Disable'/>
  )
}


export default DisableClient;