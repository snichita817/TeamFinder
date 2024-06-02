export interface TeamEditRequest {
    name: string;
    description: string;
    isPrivate: boolean;
    teamCaptainId: string;
    members: string[];
}