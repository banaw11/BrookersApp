using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;}
        IMessageRepository MessageRepository {get;}
        IConnectionRepository ConnectionRepository {get;}
        INotificationRepository NotificationRepository { get; }
        Task<bool> Complete();
        bool hasChanges();
        void Update(object obj);
    }
}