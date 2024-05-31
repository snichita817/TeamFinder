import { Team } from "../../teams/models/team.model";

export interface WinnerResult {
    id: string;
    teams: Team[]
}