import {
  FormEvent,
  useMemo,
  useState,
} from 'react';

import axios from 'axios';

import Error from '@/elements/components/error';
import FormField from '@/elements/components/form-field';
import Success from '@/elements/components/success';
import AddClientHeaders
  from '@/elements/components/webhook-clients/add-client-headers';
import ClientHeadersTableElevated
  from '@/elements/components/webhook-clients/client-headers-table-elevated';
import { WebhookClient } from '@/types/webhooks/webhook-client';
import {
  addHeaderToClient,
  removeHeaderFromClient,
} from '@/utils/webhook-client-helper';
import {
  Button,
  FormControl,
  Grid,
  MenuItem,
  Stack,
  Typography,
} from '@mui/material';

const NewClientSubmission = () => {
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
    <Grid container spacing={2} sx={{padding: 2}}>
      <Grid item xs={12}>
        <Typography variant='h5' component='h1' color='text.secondary' align='center' sx={{marginTop: 1, marginBottom: 2}}>Create Client</Typography>
        {errorElement}
        {successElement}
      </Grid>
      <Grid item xs={8}>
        <form style={{width: '100%'}} onSubmit={onSubmit}>
          <Stack>
              <FormControl>
                <FormField 
                label='Name' 
                placeholder='Enter Name' 
                isRequired={true}
                value={newClient.name ?? ''}
                isError={!nameValid}
                helperText={nameError}
                changeValueHandler={(e) => {
                  const formValue = e.currentTarget.value;
                  setNewClient((currentFormData) => {
                  return {
                    ...currentFormData,
                    name: formValue,
                  };
                  });
                }} />

                <FormField 
                label='Url' 
                placeholder='Enter Url' 
                isRequired={true}
                value={newClient.webhookUrl ?? ''}
                isError={!urlValid}
                helperText={urlError}
                changeValueHandler={(e) => {
                  const formValue = e.currentTarget.value;
                  setNewClient((currentFormData) => {
                    return {
                      ...currentFormData,
                      webhookUrl: formValue,
                    };
                  });
                }} />

                <FormField 
                label='Entity' 
                placeholder='Enter Entity to subscribe to' 
                isRequired={true}
                value={newClient.entityTypeName ?? ''}
                isError={!entityValid}
                helperText={entityError}
                changeValueHandler={(e) => {
                  const formValue = e.currentTarget.value;
                  setNewClient((currentFormData) => {
                    return {
                      ...currentFormData,
                      entityTypeName: formValue,
                    };
                  });
                }} />

                <FormField 
                label='Action' 
                placeholder='Enter subscription action' 
                isRequired={true}
                isSelect={true}
                value={newClient.actionType}
                changeValueHandler={(e) => {
                  const formValue = e.target.value;
                  setNewClient((currentFormData) => {
                    return {
                      ...currentFormData,
                      actionType: Number.parseInt(formValue),
                    };
                  });
                }}>
                  <MenuItem value={0}>Create</MenuItem>
                  <MenuItem value={1}>Update</MenuItem>
                  <MenuItem value={2}>Delete</MenuItem>
                </FormField>

              <ClientHeadersTableElevated
               titleHeader='Headers added' 
               clientHeaders={newClient.clientHeaders} 
               additionalSx={{marginTop: 2}}
               removeHeader={(header) => {
                setNewClient((currentFormData) => {
                  removeHeaderFromClient(currentFormData, header);
                  return {
                    ...currentFormData
                  };
                });
              }} />

              <Button sx={{marginTop: 1}} variant="contained" color="primary" type="submit">Submit</Button>
              </FormControl>
          </Stack>     
        </form>
      </Grid>
      <Grid item xs={4}>
        <AddClientHeaders addedHeader={(header) => {
          setNewClient((currentFormData) => {
            addHeaderToClient(currentFormData, header);
            return {
              ...currentFormData
            };
          });
        }} />
      </Grid>
    </Grid>
  )
}


export default NewClientSubmission;