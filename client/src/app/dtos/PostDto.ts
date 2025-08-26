import { LikeDto } from "./LikeDto";
import { CommentDto } from "./CommentDto";
export interface PostDto {
    id: string;
    authorId: string;
    content: string;
    createdAt: Date;
    username?: string;
    likes?: LikeDto[];
    likedByCurrentUser?: boolean;
}
