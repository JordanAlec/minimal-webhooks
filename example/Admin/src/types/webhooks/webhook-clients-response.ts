import { WebhookClient } from '@/types/webhooks/webhook-client';

export type WebhookClientsResponse = {
    success: boolean,
    message: string,
    data?: WebhookClient[]
}
