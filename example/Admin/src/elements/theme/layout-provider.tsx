import { ReactElement } from 'react';

import Layout from '@/elements/theme/layout';

type Props = {
    appName: string;
    currentPage: string;
    children: ReactElement;
}

const LayoutProvider = ({appName, currentPage, children}: Props) => { 

    return (
        <Layout appName={appName} 
            currentPage={currentPage} 
            contentChildren={children}
        />
    );
}  
  
export default LayoutProvider;