data "azurerm_client_config" "current" {}

resource "azurerm_iothub" "hub" {
  name                         = var.iot_hub_name
  resource_group_name          = var.resource_group
  location                     = var.location
  local_authentication_enabled = true

  sku {
    name     = "S1"
    capacity = "1"
  }
}

resource "azurerm_role_assignment" "contributor" {
  scope                = azurerm_iothub.hub.id
  role_definition_name = "IoT Hub Data Contributor"
  principal_id         = data.azurerm_client_config.current.object_id
}

// Service Bus Endpoint and route:
resource "azurerm_iothub_endpoint_servicebus_queue" "telemetry-queue" {
  resource_group_name = var.resource_group
  iothub_id           = azurerm_iothub.hub.id
  name                = "telemetry-queue"
  connection_string   = var.telemetry_queue_conn_string
}

resource "azurerm_iothub_route" "telemetry-queue" {
  resource_group_name = var.resource_group
  iothub_name         = azurerm_iothub.hub.name
  name                = "telemetry-queue-route"

  source         = "DeviceMessages"
  condition      = "true"
  endpoint_names = [azurerm_iothub_endpoint_servicebus_queue.telemetry-queue.name]
  enabled        = true
}

// Event Hub Endpoint and route:
resource "azurerm_iothub_endpoint_servicebus_queue" "telemetry-hub" {
  resource_group_name = var.resource_group
  iothub_id           = azurerm_iothub.hub.id
  name                = "telemetry-event-hub"
  connection_string   = var.telemetry_hub_conn_string
}

resource "azurerm_iothub_route" "telemetry-hub" {
  resource_group_name = var.resource_group
  iothub_name         = azurerm_iothub.hub.name
  name                = "telemetry-eventhub-route"

  source         = "DeviceMessages"
  condition      = "true"
  endpoint_names = [azurerm_iothub_endpoint_servicebus_queue.telemetry-hub.name]
  enabled        = true
}

// Shared access connection string used to access the registry and service:
resource "azurerm_iothub_shared_access_policy" "service_access" {
  name                = "service_access"
  resource_group_name = var.resource_group
  iothub_name         = azurerm_iothub.hub.name

  registry_read   = true
  service_connect = true
}

resource "azurerm_key_vault_secret" "hub_connection_string" {
  name         = "hub-connection-string"
  value        = azurerm_iothub_shared_access_policy.service_access.primary_connection_string
  key_vault_id = var.key_vault_key_id
}

