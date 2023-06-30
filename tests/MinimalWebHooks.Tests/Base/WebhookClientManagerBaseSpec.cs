﻿using MinimalWebHooks.Core.Processors;

namespace MinimalWebHooks.Tests.Base;

public class WebhookClientManagerBaseSpec
{
    protected Mock<IWebhookDataStore> DataStore { get; set; }
    protected Mock<IMinimalWebhookOptionsProcessor> OptionsProcessor { get; set; }
    protected WebhookClientManager Manager { get; set; }
    protected WebhookDataResult Result { get; set; }

    public WebhookClientManagerBaseSpec(MockWebhookDataStoreBuilder dataStoreBuilder, MockMinimalWebhookOptionsProcessorBuilder optionsBuilder)
    {
        DataStore = dataStoreBuilder.Build();
        OptionsProcessor = optionsBuilder.Build();
        Manager = new WebhookClientManager(DataStore.Object, OptionsProcessor.Object);
    }
}