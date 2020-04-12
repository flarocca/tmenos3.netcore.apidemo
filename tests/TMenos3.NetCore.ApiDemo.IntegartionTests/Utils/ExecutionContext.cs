using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.IntegartionTests.Utils
{
    internal class ExecutionContext<TContext> :
        IDisposable
        where TContext : DbContext
    {
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<TContext> _options;
        private readonly Func<Task> _runMigrations;

        public ExecutionContext(bool runMigrations)
        {
            // We could have used an InMemory Database Provider instead as it is
            // suitable for testing since it does not require any dependency.
            // However InMemory Database Provider does not ensure consistency.
            // SQLite can run as an In Memory database as well as InMemory Database 
            // Provider, but offering all the consistency checks we look for from 
            // relational databases
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<TContext>()
                .UseSqlite(_connection)
                .Options;

            _runMigrations = runMigrations
                ? RunMigrationsAsync :
                new Func<Task>(() => Task.CompletedTask);
        }

        public async Task<TContext> CreateContextAsync()
        {
            await _runMigrations();
            return CreateContextInstance();
        }

        public void Dispose() =>
            _connection?.Dispose();

        private async Task RunMigrationsAsync()
        {
            using var context = CreateContextInstance();
            await context.Database.MigrateAsync();
        }

        private TContext CreateContextInstance() =>
            Activator.CreateInstance(typeof(TContext), new object[] { _options }) as TContext;
    }
}
