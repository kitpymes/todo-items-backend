using MediatR;

namespace TodoItems.Infrastructure.IntegrationTests.Fakes;

public class FakeNotificationEventHandle<TEvent>(List<TEvent> received) : INotificationHandler<TEvent>
    where TEvent : INotification
{
    public Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        received.Add(notification);
        return Task.CompletedTask;
    }
}
