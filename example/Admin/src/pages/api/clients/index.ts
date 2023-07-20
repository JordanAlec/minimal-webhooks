import axios from 'axios';
import {
  NextApiRequest,
  NextApiResponse,
} from 'next';

import log from '@/utils/log';

export default async function handler(req: NextApiRequest, res: NextApiResponse) {
 if (req.method !== 'GET') {
   res.status(405).send({ message: 'Only GET requests allowed' });
   return;
 }

 const webhooksUrl = process.env.WEBHOOKS_API_URL;
 if (!webhooksUrl) return res.status(400).json({status: 'WEBHOOKS_API_URL env var not set'});

 try {
    const response = await axios.get(`${webhooksUrl}/webhooks/clients`);
    return res.status(response.status).json(response.data);
 }
 catch (e: Error | any) {
  log.error(JSON.stringify(e.response.data));
  return res.status(500).json({status: 'error', message: e.response.data.message});
 }
}
