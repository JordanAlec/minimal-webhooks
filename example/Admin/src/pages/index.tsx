import { Typography } from '@mui/material';

const Home = () => {
  return (
    <>
      <Typography sx={{ my: 5, mx: 2 }} color='text.secondary' align='center'>
        Welcome home
      </Typography>
    </>
    
  )
}

Home.currentPage = 'Home';


export default Home;