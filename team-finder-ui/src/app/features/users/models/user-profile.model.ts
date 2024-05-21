import { Category } from "../../categories/models/category.model";

export interface UserProfile {
    id: string;
    email: string;
    userName: string;
    roles: string[];
    firstName: string;
    lastName: string;
    university: string;
    courseOfStudy: number;
    graduationYear: number;
    profilePictureUrl?: string;
    bio: string;
    linkedinUrl: string;
    githubUrl: string;
    skills: string;
    categories: Category[];
    portfolioUrl: string;
}