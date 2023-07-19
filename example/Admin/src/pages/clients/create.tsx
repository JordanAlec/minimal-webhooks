import {
  FormEvent,
  useMemo,
  useState,
} from 'react';

import axios from 'axios';

import Error from '@/elements/components/error';
import Success from '@/elements/components/success';
import { WebhookClient } from '@/types/webhooks/webhook-client';
import {
  Box,
  Button,
  FormControl,
  MenuItem,
  Stack,
  TextField,
  Typography,
} from '@mui/material';

const CreateClient = () => {
  const [newClient, setNewClient] = useState({actionType: 0} as WebhookClient);
  const resetForm = () => {setNewClient({actionType: 0} as WebhookClient);};
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const errorElement = error ? <Error title='Failed to create client' bodyText={error} /> : <></>
  const successElement = success ? <Success title={success} bodyText={''} /> : <></>

  const nameValid = useMemo(() => Boolean(newClient.name), [newClient]);
  const nameError = !nameValid ? 'Please supply a name' : '';

  const urlValid = useMemo(() => Boolean(newClient.webhookUrl), [newClient]);
  const urlError = !urlValid ? 'Please supply a url' : '';

  const entityValid = useMemo(() => Boolean(newClient.entityTypeName), [newClient]);
  const entityError = !entityValid ? 'Please supply an entity type name' : '';

  const onSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    try {
        await axios.post('/api/clients/create', newClient);
        resetForm();
        setError('');
        setSuccess('Created Client');
    }
    catch (e: Error | any) {
        setError(e.response.data.message);
    }
  }
  
  return (
    <Box>
      <Typography variant='h5' component='h1' color='text.secondary' align='center' sx={{marginTop: 1, marginBottom: 2}}>Create Client</Typography>
      {errorElement}
      {successElement}
      <form style={{width: '100%'}} onSubmit={onSubmit}>
        <Stack spacing={1} sx={{margin: 5}}>
            <FormControl>
            <TextField
                sx={{ width: '100%' }}
                label="Name"
                variant="standard"
                placeholder="Enter Name"
                required={true}
                value={newClient.name ?? ''}
                error={!nameValid}
                helperText={nameError}
                onChange={(e) => {
                  const formValue = e.currentTarget.value;
                  setNewClient((currentFormData) => {
                    return {
                      ...currentFormData,
                      name: formValue,
                    };
                  });
                }}
            />

            <TextField
                sx={{ width: '100%' }}
                label="Url"
                variant="standard"
                placeholder="Enter Url"
                required={true}
                value={newClient.webhookUrl ?? ''}
                error={!urlValid}
                helperText={urlError}
                onChange={(e) => {
                  const formValue = e.currentTarget.value;
                  setNewClient((currentFormData) => {
                    return {
                      ...currentFormData,
                      webhookUrl: formValue,
                    };
                  });
                }}
            />

            <TextField
                sx={{ width: '100%' }}
                label="Entity"
                variant="standard"
                placeholder="Enter Entity to subscribe to"
                required={true}
                value={newClient.entityTypeName ?? ''}
                error={!entityValid}
                helperText={entityError}
                onChange={(e) => {
                  const formValue = e.currentTarget.value;
                  setNewClient((currentFormData) => {
                    return {
                      ...currentFormData,
                      entityTypeName: formValue,
                    };
                  });
                }}
            />

            <TextField
              sx={{ width: '100%' }}
              label="Action"
              variant="standard"
              placeholder="Enter subscription action"
              required={true}
              value={newClient.actionType}
              select
              onChange={(e) => {
                const formValue = e.target.value;
                setNewClient((currentFormData) => {
                  return {
                    ...currentFormData,
                    actionType: Number.parseInt(formValue),
                  };
                });
              }}
            >
              <MenuItem value={0}>Create</MenuItem>
              <MenuItem value={1}>Update</MenuItem>
              <MenuItem value={2}>Delete</MenuItem>
            </TextField>

            <Button sx={{marginTop: 1}} variant="contained" color="primary" type="submit">Submit</Button>
            </FormControl>
        </Stack>     
      </form>  
     
      
    </Box>
  )
}

CreateClient.currentPage = 'Create';


export default CreateClient;