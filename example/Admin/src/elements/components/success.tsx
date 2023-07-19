import {
  Alert,
  AlertTitle,
} from '@mui/material';

type Props = {
      title: string, 
      bodyText: string
    }
    
    const Success = (props: Props) => {
      return (
        <Alert severity="success">
          <AlertTitle>
            <strong>{props.title}</strong>
          </AlertTitle>
          {props.bodyText}
        </Alert>
      )
    }
    
    
    export default Success;