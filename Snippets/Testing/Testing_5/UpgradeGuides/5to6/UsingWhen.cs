﻿namespace Testing_5.UpgradeGuides._5to6
{
    using System;
    using NServiceBus.Testing;
    using NUnit.Framework;
    using Testing_5.Saga;

    [Explicit]
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Run()
        {
            Test.Initialize();
            Test.Saga<MySaga>()
                    .ExpectReplyToOriginator<MyResponse>() // In version 4 the typo in Originator was fixed.
                    .ExpectTimeoutToBeSetIn<StartsSaga>((state, span) => span == TimeSpan.FromDays(7))
                    .ExpectPublish<MyEvent>()
                    .ExpectSend<MyCommand>()
            #region 5to6-usingWhen
                .When(s => s.Handle(new StartsSaga()))
            #endregion
                .ExpectPublish<MyEvent>()
                .WhenSagaTimesOut()
                    .AssertSagaCompletionIs(true);
        }
    }
}
