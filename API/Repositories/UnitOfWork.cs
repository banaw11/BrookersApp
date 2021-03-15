using System.Threading.Tasks;
using API.Data;
using API.Interfaces;
using API.SignalR._interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public IUserRepository UserRepository => new UserRepository(_context, _mapper);

        public IMessageRepository MessageRepository => new MessageRepository(_context);
        public IConnectionRepository ConnectionRepository => new ConnectionRepository(_context);

        public INotificationRepository NotificationRepository => new NotificationRepository(_context, _mapper);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool hasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public void Update(object obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        }
    }
}