export interface TeamAddRequest {
    name: string;
    description: string;
    createdDate: Date;
    isPrivate: boolean;
    teamCaptainId: string;
    activityRegistered: string;
    members: string[];
}