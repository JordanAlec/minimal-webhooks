import ConfigurableAlert from '@/elements/components/configurable-alert';

type Props = {
      title: string, 
      bodyText: string
    }
    
const Error = ({title, bodyText}: Props) => {
  return (
    <ConfigurableAlert severity="error" title={title} bodyText={bodyText} />
  )
}


export default Error;