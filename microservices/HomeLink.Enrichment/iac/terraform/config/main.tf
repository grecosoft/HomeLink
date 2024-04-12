data "azurerm_resource_group" "solution_rg" {
  name = local.solution_resource_group_name
}

data "azurerm_client_config" "current" {}

// Microserivce prefixed configurations:
resource "azurerm_app_configuration_key" "seq" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/logging:seqUrl"
  value                  = "http://seq:5341"
}

resource "azurerm_app_configuration_key" "eventhub_hostname" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Settings:EventHubHost"
  value                  = local.solution_eventhub_hostname
}

resource "azurerm_app_configuration_key" "device_data_hub_name" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Settings:DeviceDataHubName"
  value                  = local.solution_device_data_hub_name
}

resource "azurerm_app_configuration_key" "device_data_enriched_hub_name" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Settings:DeviceDataEnrichedHubName"
  value                  = local.solution_device_data_enriched_hub_name
}

resource "azurerm_app_configuration_key" "storage_account_endpoint" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Settings:StorageAccountEndpoint"
  value                  = local.solution_storage_endpoint
}

resource "azurerm_app_configuration_key" "eventhub_storage_account_collection_name" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Settings:EventHubStorageAccountCollectionName"
  value                  = azurerm_storage_container.event_hub_data.name
}


# Consumer group for processing the received device-data that is transformed
# and enriched and sent to the secondary device-data-enriched hub consumed by
# other down stream microserivces.
resource "azurerm_eventhub_consumer_group" "device_data_enrichment" {
  name                = "device-data-enrichment"
  namespace_name      = local.solution_event_hub_namespace_name
  eventhub_name       = local.solution_device_data_hub_name
  resource_group_name = local.solution_resource_group_name
}

# Create container collection to store current offsets (move to enrichment service)
resource "azurerm_storage_container" "event_hub_data" {
  name                  = "event-hub-data"
  storage_account_name  = local.solution_storage_account_name
  container_access_type = "private"
}

resource "azurerm_app_configuration_key" "device_data_consumer_group_name" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Settings:DeviceDataConsumerGroupName"
  value                  = azurerm_eventhub_consumer_group.device_data_enrichment.name
}

resource "azurerm_role_assignment" "event_hub_data" {
  scope                = azurerm_storage_container.event_hub_data.resource_manager_id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = local.solution_workload_identity.principal_id
}