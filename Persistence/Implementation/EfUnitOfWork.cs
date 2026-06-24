using Microsoft.EntityFrameworkCore.Storage;
using TestAPI.Data;
using TestAPI.Persistence.Interfaces;

namespace TestAPI.Persistence.Implementation;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public EfUnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            return;
        }

        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);

        if (_transaction != null)
        {
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }

        _context.ChangeTracker.Clear();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
