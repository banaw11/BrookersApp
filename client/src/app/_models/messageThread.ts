import { Friend } from "./friend";
import { Message } from "./message";

export interface MessageThread{
    chatMember: Friend,
    messages: Message[],
}