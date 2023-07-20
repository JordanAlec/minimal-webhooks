import {
  ChangeEventHandler,
  ReactNode,
} from 'react';

import { TextField } from '@mui/material';

type Props = {
    label: string
    placeholder: string
    isRequired: boolean
    value: unknown
    isError?: boolean | undefined
    isSelect?: boolean | undefined
    helperText?: ReactNode
    changeValueHandler?: ChangeEventHandler<HTMLInputElement>
    children?: ReactNode
}

const FormField = ({label, placeholder, isRequired, value, isError, isSelect, helperText, changeValueHandler, children} : Props) => {
  return (
    <TextField
        sx={{ width: '100%' }}
        label={label}
        variant='standard'
        placeholder={placeholder}
        required={isRequired}
        value={value}
        error={isError}
        select={isSelect}
        helperText={helperText}
        onChange={changeValueHandler}>{children}</TextField>
  )
}


export default FormField;