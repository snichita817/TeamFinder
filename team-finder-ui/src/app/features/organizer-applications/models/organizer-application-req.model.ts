import { User } from "../../users/models/user.model";

export interface OrganizerApplication {
    id: string;
    user: User;
    reason: string;
    applicationDate: Date;
    status: string;
}