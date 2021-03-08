import { BehaviorSubject, Observable } from "rxjs";
import { Friend } from "./friend";
import { Message } from "./message";

export interface MessageThread{
    chatMember: Observable<Friend>,
    messages: Observable<Message[]>,
}