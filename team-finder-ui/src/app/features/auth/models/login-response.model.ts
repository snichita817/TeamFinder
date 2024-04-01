export interface LoginResponse {
    id: string;
    email: string;
    token: string;
    roles: string[];
}