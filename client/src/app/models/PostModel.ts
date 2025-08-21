export interface PostModel {
    id: string;
    authorId: string;
    content: string;
    createdAt: Date;
    username?: string;
}
