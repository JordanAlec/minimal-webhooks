import axios from 'axios';

import CallApiButton from '@/elements/components/call-api-button';

type Props = {
    id: number
}

const EnableClient = ({id}: Props) => {
  return (
    <CallApiButton buttonColor='primary' action={async () => await axios.patch(`/api/clients/enable/${id}`)} buttonText='Enable'/>
  )
}


export default EnableClient;