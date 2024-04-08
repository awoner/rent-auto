using NCrontab;

namespace RentAutoPoc.Api.Infrastructure.Crons;

public sealed record CronRegistryEntry(Type Type, CrontabSchedule CrontabSchedule);