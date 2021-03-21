namespace API.SignalR._interfaces
{
    public interface IUnitOfHub
    {
        IMessageService MessageService {get;}
        INotificationsService NotificationsService {get;}
        IInviteService InviteService { get; }
    }
}