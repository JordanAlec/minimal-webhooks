import { TextField } from '@mui/material';

type Props = {
    id: string
    label: string
    value?: unknown
}

const ReadOnlyFilledField = ({id, label, value}: Props) => {
  return (
    <TextField variant="filled" sx={{marginTop: 1}} fullWidth id={id} label={label} defaultValue={value} InputProps={{readOnly: true,}} />
  )
}


export default ReadOnlyFilledField;