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