import { Team } from "../../teams/models/team.model";
import { User } from "../../users/models/user.model";

export interface TeamMembershipRequest {
    id: string;
    user: User;
    team: Team;
    requestDate: Date;
    status: string;
}