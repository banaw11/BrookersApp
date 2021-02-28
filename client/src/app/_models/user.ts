import { Friend } from "./friend";

export interface User{
    userName: string,
    token: string,
    email: string,
    avatar: string,
    friends: Friend[]
}