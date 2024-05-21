export interface UserEditRequest {
    email: string;
    userName: string;
    roles: string[];
    firstName: string;
    lastName: string;
    university: string;
    graduationYear: number;
    profilePictureUrl?: string;
    bio: string;
    linkedinUrl: string;
    githubUrl: string;
    skills: string;
    portfolioUrl: string;
    categories: string[];
}