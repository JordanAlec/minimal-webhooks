import ConfigurableAlert from '@/elements/components/configurable-alert';

type Props = {
      title: string, 
      bodyText: string
    }
    
const Success = ({title, bodyText}: Props) => {
  return (
    <ConfigurableAlert severity="success" title={title} bodyText={bodyText} />
  )
}


export default Success;