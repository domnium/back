using System;
using System.Linq.Expressions;
using Domain.Interfaces.Repositories;
using Hangfire;

namespace Infrastructure.Repositories;
public class JobRepository : IJobRepository
{
    public void ScheduleJob<T>(string jobId, Expression<Action<T>> methodCall, TimeSpan interval)
    => RecurringJob.AddOrUpdate(jobId, methodCall, Cron.Minutely);
    public Task<string> EnqueueAsync<T>(Expression<Func<T, Task>> methodCall)
    => Task.FromResult(BackgroundJob.Enqueue(methodCall));
}