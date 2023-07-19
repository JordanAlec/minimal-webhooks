import axios from 'axios';
import { useRouter } from 'next/router';

import log from '@/utils/log';
import { Button } from '@mui/material';

type Props = {
    id: number
}

const DisableClient = (props: Props) => {
  const router = useRouter();

  const handleOnClick = async () => {
    try {
        const response = await axios.delete(`/api/clients/disable/${props.id}`);
        if (response.status === 200) router.reload();
    }
    catch (e: Error | any) {
        log.error(JSON.stringify(e));
    }
  };

  return (
    <Button variant="contained" color="error" onClick={handleOnClick}>Disable</Button>
  )
}


export default DisableClient;