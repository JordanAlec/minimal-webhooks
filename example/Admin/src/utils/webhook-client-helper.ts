import { WebhookClient } from '@/types/webhooks/webhook-client';
import { WebhookClientHeader } from '@/types/webhooks/webhook-client-header';

export const actionTypeConversion = (actionType: number) => {
    switch (actionType) {
        case 0:
            return 'Create';
        case 1:
            return 'Update';
        case 2:
            return 'Delete';
        default:
            return 'Unknown';
    }
}

export const HeaderExistsOnClient = (webhookClient: WebhookClient, headerToCheck: WebhookClientHeader) => {
    const isFound = webhookClient.clientHeaders?.some(header => {
        if (header.key === headerToCheck.key) return true;
        return false;
    })
    return isFound;
}

export const addHeaderToClient = (webhookClient: WebhookClient, headerToAdd: WebhookClientHeader): WebhookClient => {
    webhookClient.clientHeaders = webhookClient.clientHeaders ? webhookClient.clientHeaders : [];
    const headerExists = HeaderExistsOnClient(webhookClient, headerToAdd);
    headerExists ? updateHeaderOnClient(webhookClient, headerToAdd) : webhookClient.clientHeaders?.push(headerToAdd);
    return webhookClient
}

export const updateHeaderOnClient = (webhookClient: WebhookClient, headerToUpdate: WebhookClientHeader): WebhookClient => {
    const elementIndex = webhookClient.clientHeaders?.findIndex((header => header.key == headerToUpdate.key ));
    webhookClient.clientHeaders![elementIndex as number].value = headerToUpdate.value;
    return webhookClient;
}

export const removeHeaderFromClient = (webhookClient: WebhookClient, headerToRemove: WebhookClientHeader): WebhookClient => {
    webhookClient.clientHeaders = webhookClient.clientHeaders?.filter(h => h.key != headerToRemove.key);
    return webhookClient
}