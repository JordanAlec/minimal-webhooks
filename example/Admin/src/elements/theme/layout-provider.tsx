import { ReactElement } from 'react';

import Layout from '@/elements/theme/layout';

type Props = {
    appName: string;
    currentPage: string;
    children: ReactElement;
}

const LayoutProvider = (props: Props) => { 

    return (
        <Layout appName={props.appName} 
            currentPage={props.currentPage} 
            contentChildren={props.children}
        />
    );
}  
  
export default LayoutProvider;