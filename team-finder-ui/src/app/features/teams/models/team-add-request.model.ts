export interface TeamAddRequest {
    name: string;
    description: string;
    createdDate: Date;
    acceptedToActivity: boolean;
    isPrivate: boolean;
    teamCaptainId: string;
    activityRegistered: string;
    members: string[];
}