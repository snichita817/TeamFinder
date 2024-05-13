import { Activity } from "../../activities/models/activity.model";
import { User } from "../../users/models/user.model";

export interface Team{
    id: string;
    name: string;
    description: string;
    createdDate: Date;
    acceptedToActivity: boolean;
    isPrivate: boolean;
    activityRegistered: Activity;
    members: User[];
}