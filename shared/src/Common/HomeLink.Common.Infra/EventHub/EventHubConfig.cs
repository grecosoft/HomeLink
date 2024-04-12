using NetFusion.Core.Settings;

namespace HomeLink.Common.Infra.EventHub;

[ConfigurationSection("azure:settings")]
public class EventHubConfig : IAppSettings
{
    public required string EventHubHost { get; set; }
    public required string DeviceDataHubName { get; set; }
    public required string DeviceDataEnrichedHubName { get; set; }
    public required string DeviceDataConsumerGroupName { get; set; }
    public required string StorageAccountEndpoint { get; set; }
    public required string EventHubStorageAccountCollectionName { get; set; }
}