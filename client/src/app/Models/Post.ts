import { Like } from "./like";
import { Comment } from "./comment";
export interface Post {
    id: string;
    authorId: string;
    content: string;
    imageUrl?: string | null;
    imageKey?: string | null;
    createdAt: Date;
    username?: string;
    likes?: Like[];
    likedByCurrentUser?: boolean;
}
