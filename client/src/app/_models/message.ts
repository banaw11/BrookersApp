export interface Message{
    senderId: number,
    receiverId: number,
    content: string,
    isRead: boolean,
    sendDate: Date
}