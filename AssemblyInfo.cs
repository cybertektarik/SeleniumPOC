using NUnit.Framework;

#if RUN_PARALLEL
[assembly: Parallelizable(ParallelScope.Scenarios)]
[assembly: LevelOfParallelism(5)]
#else
[assembly: Parallelizable(ParallelScope.None)]
#endif
