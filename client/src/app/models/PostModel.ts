import { LikeModel } from "./LikeModel";
export interface PostModel {
    id: string;
    authorId: string;
    content: string;
    createdAt: Date;
    username?: string;
    likes?: LikeModel[];
    likedByUser?: boolean;
}
