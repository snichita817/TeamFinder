import { Update } from "../../updates/models/update.model";
import { Category } from "../../categories/models/category.model";
import { User } from "../../users/models/user.model";
import { Team } from "../../teams/models/team.model";
import { WinnerResult } from "./winner-result.model";
export interface Activity {
    id: string;
    title: string;
    shortDescription: string;
    longDescription: string;
    startDate: Date;
    endDate: Date;
    openRegistration: boolean;
    maxTeams: number;
    minParticipant: number;
    maxParticipant: number;
    urlHandle: string;
    createdBy: User;
    createdDate: Date;
    updates: Update[];
    categories: Category[];
    teams: Team[];
    winnerResult: WinnerResult;
}