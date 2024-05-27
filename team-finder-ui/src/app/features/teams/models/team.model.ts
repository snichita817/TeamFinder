import { Activity } from "../../activities/models/activity.model";
import { User } from "../../users/models/user.model";

export interface Team{
    id: string;
    name: string;
    description: string;
    createdDate: Date;
    acceptedToActivity: string;
    isPrivate: boolean;
    teamCaptainId: string;
    submissionUrl: string;
    minParticipant: number;
    maxParticipant: number;
    activityRegistered: Activity;
    members: User[];
}