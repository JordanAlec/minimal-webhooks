import {
  Alert,
  AlertTitle,
} from '@mui/material';

type Props = {
    title: string, 
    bodyText: string
  }
  
  const Error = (props: Props) => {
    return (
      <Alert severity="error">
        <AlertTitle>
          <strong>{props.title}</strong>
        </AlertTitle>
        {props.bodyText}
      </Alert>
    )
  }
  
  
  export default Error;