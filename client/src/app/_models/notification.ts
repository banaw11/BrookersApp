import { Friend } from "./friend";
import { UnreadMessage } from "./unreadMessage";

export interface Notification{
    unreadMessages: UnreadMessage[];
    invitations: Friend[];
}