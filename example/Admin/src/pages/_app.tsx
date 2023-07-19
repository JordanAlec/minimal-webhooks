import type { NextComponentType } from 'next';
import type { AppProps } from 'next/app';

import LayoutProvider from '@/elements/theme/layout-provider';

type CustomAppProps = AppProps & {
  Component: NextComponentType & {
    currentPage: string;
  }
}

export default function App({ Component, pageProps }: CustomAppProps) {
  return (
    <LayoutProvider appName='Admin' currentPage={Component.currentPage} >
      <Component {...pageProps} />
    </LayoutProvider>
  );
}