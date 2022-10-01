using Domain.Interfaces;
using Entities.Entities;
using Infrastructure.Context;
using Infrastructure.Repository.Generics;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Repositories
{
    public class MessageRepository : RepositoryGenerics<Message>, IMessage
    {
        private readonly DbContextOptions<ContextBase> _context;

        public MessageRepository()
        {
            _context = new DbContextOptions<ContextBase>();
        }
    }
}
