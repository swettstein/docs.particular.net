﻿using NServiceBus;
using NServiceBus.Logging;

public class OrderCompletedHandler : IHandleMessages<OrderCompleted>
{
    static ILog logger = LogManager.GetLogger<OrderCompletedHandler>();

    public void Handle(OrderCompleted message)
    {
        logger.Info($"Received OrderCompleted for OrderId {message.OrderId}");
    }
}