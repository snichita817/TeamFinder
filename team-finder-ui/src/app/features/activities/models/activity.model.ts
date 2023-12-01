export interface Activity {
    id: string;
    title: string;
    shortDescription: string;
    longDescription: string;
    startDate: Date;
    endDate: Date;
    openRegistration: boolean;
    maxParticipants: number;
    createdBy: string;
    createdDate: Date;
}