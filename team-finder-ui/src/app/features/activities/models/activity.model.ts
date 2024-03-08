import { Update } from "../../updates/models/update.model";
import { Category } from "../../categories/models/category.model";

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
    updates: Update[];
    categories: Category[];
}