using System.Threading.Tasks;
using Azure.DigitalTwins.Core;
using HomeLink.Management.Domain.Entities.Schema;

namespace HomeLink.Management.Domain.Services;

public interface IDeviceDataService
{
    Task<TwinSchemaModel> CreateDeviceDataModel(string deviceModelId);
    Task<BasicDigitalTwin> CreateDeviceDataTwin(string deviceModelId, string deviceId, string? sourcePath);
}