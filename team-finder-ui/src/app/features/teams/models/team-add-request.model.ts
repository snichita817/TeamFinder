import { Activity } from "../../activities/models/activity.model";
import { User } from "../../users/models/user.model";

export interface TeamAddRequest {
    name: string;
    description: string;
    createdDate: Date;
    acceptedToActivity: boolean;
    isPrivate: boolean;
    createdBy: string;
    activityRegistered: string;
    members: string[];
}