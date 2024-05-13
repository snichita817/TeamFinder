export interface TeamEditRequest {
    name: string;
    description: string;
    createdDate: Date;
    acceptedToActivity: boolean;
    isPrivate: boolean;
    teamCaptainId: string;
    members: string[];
}