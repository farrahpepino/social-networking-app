import { Like } from "./Like";
import { Comment } from "./Comment";
export interface Post {
    id: string;
    authorId: string;
    content: string;
    imageUrl?: string | null;
    createdAt: Date;
    username?: string;
    likes?: Like[];
    likedByCurrentUser?: boolean;
}
