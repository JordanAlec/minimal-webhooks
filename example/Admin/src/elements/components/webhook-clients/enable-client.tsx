import axios from 'axios';

import ActionButton from '@/elements/components/action-button';

type Props = {
    id: number
}

const EnableClient = ({id}: Props) => {
  return (
    <ActionButton buttonColor='primary' action={async () => await axios.patch(`/api/clients/enable/${id}`)} buttonText='Enable'/>
  )
}


export default EnableClient;