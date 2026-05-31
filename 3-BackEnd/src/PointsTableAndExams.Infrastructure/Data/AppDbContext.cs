using MediatR;
using Microsoft.EntityFrameworkCore;
using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.Interfaces.Repositories;

namespace PointsTableAndExams.Infrastructure.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator)
    : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; init; }
    public DbSet<FoodCategory> FoodCategories { get; init; }
    public DbSet<FoodItem> FoodItems { get; init; }
    public DbSet<DailyLog> DailyLogs { get; init; }
    public DbSet<DailyLogItem> DailyLogItems { get; init; }
    public DbSet<ExamCategory> ExamCategories { get; init; }
    public DbSet<Exam> Exams { get; init; }
    public DbSet<ExamRequest> ExamRequests { get; init; }
    public DbSet<ExamRequestItem> ExamRequestItems { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var aggregates = ChangeTracker.Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count != 0)
            .ToList();

        var events = aggregates.SelectMany(e => e.DomainEvents).ToList();
        aggregates.ForEach(e => e.ClearDomainEvents());

        foreach (var evt in events)
            await mediator.Publish(evt, cancellationToken);
    }
}
