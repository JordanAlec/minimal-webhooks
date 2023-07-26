import {
  WebhookClientActivityLog,
} from '@/types/webhooks/webhook-client-activity-log';
import { WebhookClientHeader } from '@/types/webhooks/webhook-client-header';

export type WebhookClient = {
    id: number,
    name: string,
    webhookUrl: string,
    actionType: number,
    entityTypeName: string,
    disabled: boolean,
    clientHeaders?: WebhookClientHeader[],
    activityLogs?: WebhookClientActivityLog[]
}
