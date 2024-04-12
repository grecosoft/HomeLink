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

resource "azurerm_app_configuration_key" "twins_connection_string" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Services:DigitalTwins:host"
  value                  = local.solution_digital_twins_host_name
}

resource "azurerm_app_configuration_key" "servicebus_hostname" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:ServiceBus:Namespaces:Home-IoT:Host"
  value                  = local.solution_servicebus_hostname
}

// Microserivce labeled secrets:
data "azurerm_key_vault_secret" "hub_connection_string" {
  name         = "hub-connection-string"
  key_vault_id = local.solution_key_vault_id
}

resource "azurerm_app_configuration_key" "hub_connection_string" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure__Services__DigitalTwins__IotHubConn"
  type                   = "vault"
  vault_key_reference    = data.azurerm_key_vault_secret.hub_connection_string.versionless_id
}




resource "azurerm_app_configuration_key" "eventhub_hostname" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Settings:EventHubHost"
  value                  = local.solution_eventhub_hostname
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
resource "azurerm_eventhub_consumer_group" "device_data_management" {
  name                = "device-data-management"
  namespace_name      = local.solution_event_hub_namespace_name
  eventhub_name       = local.solution_device_data_enriched_hub_name
  resource_group_name = local.solution_resource_group_name
}

# Create container collection to store current offsets (move to enrichment service)
resource "azurerm_storage_container" "event_hub_data" {
  name                  = "event-hub-enrichment"
  storage_account_name  = local.solution_storage_account_name
  container_access_type = "private"
}

resource "azurerm_app_configuration_key" "device_data_consumer_group_name" {
  configuration_store_id = local.solution_app_config_id
  key                    = "${var.service_name}/Azure:Settings:DeviceDataConsumerGroupName"
  value                  = azurerm_eventhub_consumer_group.device_data_management.name
}

resource "azurerm_role_assignment" "event_hub_data" {
  scope                = azurerm_storage_container.event_hub_data.resource_manager_id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = local.solution_workload_identity.principal_id
}

resource "azurerm_role_assignment" "event_hub_data_current" {
  scope                = azurerm_storage_container.event_hub_data.resource_manager_id
  role_definition_name = "Storage Blob Data Contributor"
  principal_id         = data.azurerm_client_config.current.object_id
}