using System;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories;

public interface IJobRepository
{
    void ScheduleJob<T>(string jobId, Expression<Action<T>> methodCall, TimeSpan interval);
    Task<string> EnqueueAsync<T>(Expression<Func<T, Task>> methodCall);
}