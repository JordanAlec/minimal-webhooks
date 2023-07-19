import { TextField } from '@mui/material';

type Props = {
    id: string
    label: string
    value?: unknown
}

const ReadOnlyFilledField = (props: Props) => {
  return (
    <TextField variant="filled" sx={{marginTop: 1}} fullWidth id={props.id} label={props.label} defaultValue={props.value} InputProps={{readOnly: true,}} />
  )
}


export default ReadOnlyFilledField;