import {
  Alert,
  AlertColor,
  AlertTitle,
} from '@mui/material';

type Props = {
  severity: AlertColor
  title: string, 
  bodyText: string
}
      
const ConfigurableAlert = ({severity, title, bodyText}: Props) => {
  return (
    <Alert severity={severity}>
      <AlertTitle>
        <strong>{title}</strong>
      </AlertTitle>
      {bodyText}
    </Alert>
  )
}


export default ConfigurableAlert;