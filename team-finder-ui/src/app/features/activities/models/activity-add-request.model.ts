export interface ActivityAddRequest {
    title: string;
    shortDescription: string;
    longDescription: string;
    startDate: Date;
    endDate: Date;
    openRegistration: boolean;
    maxTeams: number;
    minParticipant: number;
    maxParticipant: number;
    createdBy: string;
    categories: string[];
}