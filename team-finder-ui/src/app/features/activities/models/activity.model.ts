import { Update } from "../../updates/models/update.model";
import { Category } from "../../categories/models/category.model";
import { User } from "../../users/models/user.model";
export interface Activity {
    id: string;
    title: string;
    shortDescription: string;
    longDescription: string;
    startDate: Date;
    endDate: Date;
    openRegistration: boolean;
    maxParticipant: number;
    urlHandle: string;
    createdBy: User;
    createdDate: Date;
    updates: Update[];
    categories: Category[];
}