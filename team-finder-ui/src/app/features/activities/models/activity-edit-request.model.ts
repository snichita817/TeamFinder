import { User } from "../../auth/models/user.model";

export interface ActivityEditRequest {
    title: string;
    shortDescription: string;
    longDescription: string;
    startDate: Date;
    endDate: Date;
    openRegistration: boolean;
    maxTeams: number;
    minParticipant: number;
    maxParticipant: number;
    createdBy: string | undefined;
    categories: string[];
}