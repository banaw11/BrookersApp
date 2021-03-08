import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { NewMessage } from '../_models/newMessage';

@Injectable({
  providedIn: 'root'
})
export class ChatService {
baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getMessageThread(memberId : number){
    return this.http.get<Message[]>(this.baseUrl + "chat/"+ memberId);
  }

  sendMessage(message: NewMessage){
    return this.http.post(this.baseUrl + 'chat/new-message', message).pipe(
      map((msg : Message) => {
        return msg;
      })
    )
  }
}
