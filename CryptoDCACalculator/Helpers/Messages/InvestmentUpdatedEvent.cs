using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CryptoDCACalculator.Helpers.Messages;

public class InvestmentUpdatedEvent(InvestmentChanges value) : ValueChangedMessage<InvestmentChanges>(value);

public record InvestmentChanges(int Id, bool IsNew)
{
}
