using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository {get;}
        IMessageRepository MessageRepository {get;}
        IConnectionRepository ConnectionRepository {get;}
        Task<bool> Complete();
        bool hasChanges();
    }
}