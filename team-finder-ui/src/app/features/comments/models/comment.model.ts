import { Update } from "../../updates/models/update.model";
import { User } from "../../users/models/user.model";

export interface Comment {
    id: string;
    text: string;
    date: Date;
    update: Update;
    user: User
  }
  