namespace BackgroundBank131Service.Models;

public class Constants
{
    public readonly static int SendingPayoutSessionJobWaitingTimeInMs = 8 * 1000;
    public readonly static string LogPrefixBackground = "Background Job Bank 131";
    public readonly static int BatchSize = 50;
}