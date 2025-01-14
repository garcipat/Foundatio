﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Foundatio.Lock;

namespace Foundatio.Jobs {
    public class WorkItemContext {
        private readonly Func<int, string, Task> _progressCallback;

        public WorkItemContext(object data, string jobId, ILock workItemLock, CancellationToken cancellationToken, Func<int, string, Task> progressCallback) {
            Data = data;
            JobId = jobId;
            WorkItemLock = workItemLock;
            CancellationToken = cancellationToken;
            _progressCallback = progressCallback;
        }

        public object Data { get; private set; }
        public string JobId { get; private set; }
        public ILock WorkItemLock { get; private set; }
        public JobResult Result { get; set; } = JobResult.Success;
        public CancellationToken CancellationToken { get; private set; }

        public Task ReportProgressAsync(int progress, string message = null) {
            return _progressCallback(progress, message);
        }

        public Task RenewLockAsync() {
            if (WorkItemLock != null)
                return WorkItemLock.RenewAsync();

            return Task.CompletedTask;
        }

        public T GetData<T>() where T : class {
            return Data as T;
        }
    }
}