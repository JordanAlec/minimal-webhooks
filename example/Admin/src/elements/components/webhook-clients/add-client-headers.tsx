import {
  FormEvent,
  useState,
} from 'react';

import FormField from '@/elements/components/form-field';
import { WebhookClientHeader } from '@/types/webhooks/webhook-client-header';
import {
  Button,
  FormControl,
} from '@mui/material';

type Props = {
    addedHeader: (header: WebhookClientHeader) => void;
}

const AddClientHeaders = ({addedHeader} : Props) => {
 const [header, setHeader] = useState({} as WebhookClientHeader);

 const onSubmit = (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    addedHeader(header);
    setHeader({} as WebhookClientHeader);
  }

  return (
    <form onSubmit={onSubmit}>
        <FormControl>   
            <FormField 
            label='Header Key' 
            placeholder='Enter Key' 
            isRequired={true}
            value={header.key ?? ''}
            isError={false}
            helperText={''}
            changeValueHandler={(e) => {
              const formValue = e.currentTarget.value;
              setHeader((currentFormData) => {
              return {
                ...currentFormData,
                key: formValue,
              };
              });
            }} />
    
            <FormField 
            label='Header Value' 
            placeholder='Enter value' 
            isRequired={true}
            value={header.value ?? ''}
            isError={false}
            helperText={''}
            changeValueHandler={(e) => {
              const formValue = e.currentTarget.value;
              setHeader((currentFormData) => {
              return {
                ...currentFormData,
                value: formValue,
              };
              });
            }} />
    
            <Button sx={{marginTop: 1}} variant="contained" color="primary" type="submit">Add Header</Button>
        
        </FormControl>
    </form>
  )
}


export default AddClientHeaders;