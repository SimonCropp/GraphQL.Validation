using ApprovalTests.Reporters;
using Xunit;

[assembly: UseReporter(typeof(DiffReporter), typeof(AllFailingTestsClipboardReporter))]
[assembly: CollectionBehavior(DisableTestParallelization = true,MaxParallelThreads = 1)]