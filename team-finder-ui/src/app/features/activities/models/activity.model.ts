export interface Activity {
    id: string;
    title: string;
    shortDescription: string;
    longDescription: string;
    startDate: Date;
    endDate: Date;
    openRegistration: boolean;
    maxParticipant: number;
    createdBy: string;
    createdDate: Date;
}