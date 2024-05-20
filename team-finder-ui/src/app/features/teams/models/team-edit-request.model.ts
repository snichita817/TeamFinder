export interface TeamEditRequest {
    name: string;
    description: string;
    createdDate: Date;
    isPrivate: boolean;
    teamCaptainId: string;
    members: string[];
}