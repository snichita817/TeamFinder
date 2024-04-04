import { Category } from "../../categories/models/category.model";

export interface UserProfile {
    id: string;
    email: string;
    userName: string;
    roles: string[];
    firstName: string;
    lastName: string;
    university: string;
    categories: Category[];
    graduationYear: number;
    bio: string;
    linkedinUrl: string;
    githubUrl: string;
    skills: string;
    portfolioUrl: string;
}