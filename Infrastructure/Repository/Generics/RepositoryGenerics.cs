using Domain.Interfaces.Generics;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Infrastructure.Repository.Generics
{
    public class RepositoryGenerics<T> : IGeneric<T>, IDisposable where T : class
    {

        private readonly DbContextOptions<ContextBase> _context;

        public RepositoryGenerics()
        {
            _context = new DbContextOptions<ContextBase>();
        }
        

        public async Task Add(T obj)
        {
            using (var data = new ContextBase(_context))
            {
                await data.Set<T>().AddAsync(obj);
                await data.SaveChangesAsync();
            }
        }

        public async Task Delete(T obj)
        {
            using (var data = new ContextBase(_context))
            {
                data.Set<T>().Remove(obj);
                await data.SaveChangesAsync();
            }
        }

        public async Task<T> GetEntityById(int id)
        {
            using (var data = new ContextBase(_context))
            {
                return await data.Set<T>().FindAsync(id);
            }
        }

        public async Task<List<T>> List()
        {
            using (var data = new ContextBase(_context))
            {
                return await data.Set<T>().ToListAsync();
            }
        }

        public async Task Update(T obj)
        {
            using (var data = new ContextBase(_context))
            {
                data.Set<T>().Update(obj);
                await data.SaveChangesAsync();
            }
        }

        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);


        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion
    }
}
