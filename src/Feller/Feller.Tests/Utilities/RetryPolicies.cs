using System;
using Polly;
using Polly.Retry;

namespace Feller.Tests.Utilities
{
    public static class RetryPolicies
    {
        public static RetryPolicy<bool> DefaultRetryPolicy = Policy
                .HandleResult(false)
                .WaitAndRetry(50, i => TimeSpan.FromMilliseconds(2));
    }
}
