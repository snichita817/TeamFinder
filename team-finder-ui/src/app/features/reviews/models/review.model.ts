import { User } from "../../users/models/user.model";

export interface Review {
    id: string;
    content: string;
    rating: number;
    date: Date;
    organizer: User;
}