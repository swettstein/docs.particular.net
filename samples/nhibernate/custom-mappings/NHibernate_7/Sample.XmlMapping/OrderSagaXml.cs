﻿using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class OrderSagaXml : Saga<OrderSagaDataXml>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>
{
    static ILog logger = LogManager.GetLogger<OrderSagaXml>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaDataXml> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(message => message.OrderId)
                .ToSaga(sagaData => sagaData.OrderId);
    }

    public Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        Data.OrderId = message.OrderId;
        logger.Info($"Saga with OrderId {Data.OrderId} received StartOrder with OrderId {message.OrderId} (Saga version: {Data.Version})");

        if (Data.From == null)
        {
            Data.From = new OrderSagaDataXml.Location();
        }
        if (Data.To == null)
        {
            Data.To = new OrderSagaDataXml.Location();
        }

        Data.From.Lat = 51.9166667;
        Data.From.Long = 4.5;

        Data.To.Lat = 51.51558;
        Data.To.Long = -0.12085;

        var completeOrder = new CompleteOrder
        {
            OrderId = "123"
        };
        return context.SendLocal(completeOrder);
    }

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.Info($"Saga with OrderId {Data.OrderId} received CompleteOrder with OrderId {message.OrderId}");
        MarkAsComplete();
        return Task.FromResult(0);
    }

}
