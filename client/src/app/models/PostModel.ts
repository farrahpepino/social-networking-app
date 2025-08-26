import { LikeModel } from "./LikeModel";
import { CommentModel } from "./CommentModel";
export interface PostModel {
    id: string;
    authorId: string;
    content: string;
    createdAt: Date;
    username?: string;
    likes: LikeModel[];
    liked: boolean;
}
