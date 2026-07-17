

namespace MiraDistribution.Domain.Enums
{
    public enum BookStatus
    {
        NotAssigned = 1,           // لم يتم اشارته لأي موزع
        AssignedToDistributor = 2, // تم اشارته للموزع
        WaitingForCash = 3,        // انتظار النقدية
        CashReceived = 4,          // استلام النقدية
        FullyCollected = 5         // تم تحصيل الدفتر بالكامل
    }
}
