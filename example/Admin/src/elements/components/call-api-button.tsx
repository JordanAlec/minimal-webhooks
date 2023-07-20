import { AxiosResponse } from 'axios';
import { useRouter } from 'next/router';

import log from '@/utils/log';
import { Button } from '@mui/material';

type Props = {
    buttonColor: 'info' | 'error' | 'inherit' | 'primary' | 'secondary' | 'success' | 'warning'
    action: () => Promise<AxiosResponse<any, any>>
    buttonText: string
}

const CallApiButton = ({buttonColor, action, buttonText}: Props) => {
  const router = useRouter();

  const handleOnClick = async () => {
    try {
        const response = await action();
        if (response.status === 200) router.reload();
    }
    catch (e: Error | any) {
        log.error(JSON.stringify(e));
    }
  };

  return (
    <Button variant="contained" color={buttonColor} onClick={handleOnClick}>{buttonText}</Button>
  )
}


export default CallApiButton;