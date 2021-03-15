import { Friend } from "./friend";
import { Notification } from "./notification";

export interface User{
    userName: string,
    token: string,
    email: string,
    avatar: string,
    friends: Friend[],
    notification: Notification
}