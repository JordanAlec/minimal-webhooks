import { ReactElement } from 'react';

import {
  Paper,
  Table,
  TableBody,
  TableContainer,
  TableHead,
} from '@mui/material';

type Props = {
    tableMinWidth: number
    tableHeaders: ReactElement,
    tableDataRows: ReactElement
}
const ConfigurableTable = ({tableMinWidth, tableHeaders, tableDataRows} : Props) => {

  return (
    <TableContainer component={Paper}>
        <Table sx={{ minWidth: tableMinWidth }}>
            <TableHead>
                {tableHeaders}
            </TableHead>
            <TableBody>
                {tableDataRows}
            </TableBody>
        </Table>
    </TableContainer>
  )
}


export default ConfigurableTable;