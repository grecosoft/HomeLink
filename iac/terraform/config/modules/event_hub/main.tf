data "azurerm_client_config" "current" {}

resource "azurerm_eventhub_namespace" "event_hub_ns" {
  name                = "${var.name}-${var.unique_postfix}"
  location            = var.location
  resource_group_name = var.resource_group
  sku                 = "Standard"
  capacity            = 1
}

resource "azurerm_eventhub" "data_hub" {
  name                = "device-data"
  namespace_name      = azurerm_eventhub_namespace.event_hub_ns.name
  resource_group_name = var.resource_group
  partition_count     = 1
  message_retention   = 1
}

resource "azurerm_eventhub" "data_enriched_hub" {
  name                = "device-data-enriched"
  namespace_name      = azurerm_eventhub_namespace.event_hub_ns.name
  resource_group_name = var.resource_group
  partition_count     = 1
  message_retention   = 1
}

resource "azurerm_eventhub_authorization_rule" "telemetry-hub-auth-role" {
  name                = "telemetry-hub-auth-role"
  namespace_name      = azurerm_eventhub_namespace.event_hub_ns.name
  eventhub_name       = azurerm_eventhub.data_hub.name
  resource_group_name = var.resource_group
  listen              = false
  send                = true
  manage              = false
}

resource "azurerm_role_assignment" "telemetry-hub" {
  scope                = azurerm_eventhub.data_hub.id
  role_definition_name = "Azure Event Hubs Data Receiver"
  principal_id         = var.identity.principal_id
}

resource "azurerm_role_assignment" "telemetry-hub-current" {
  scope                = azurerm_eventhub.data_hub.id
  role_definition_name = "Azure Event Hubs Data Receiver"
  principal_id         = data.azurerm_client_config.current.object_id
}

resource "azurerm_role_assignment" "telemetry-hub-enriched" {
  scope                = azurerm_eventhub.data_enriched_hub.id
  role_definition_name = "Azure Event Hubs Data Sender"
  principal_id         = var.identity.principal_id
}

resource "azurerm_role_assignment" "telemetry-hub-enriched-current" {
  scope                = azurerm_eventhub.data_enriched_hub.id
  role_definition_name = "Azure Event Hubs Data Sender"
  principal_id         = data.azurerm_client_config.current.object_id
}

resource "azurerm_role_assignment" "telemetry-hub-enriched-receiver" {
  scope                = azurerm_eventhub.data_enriched_hub.id
  role_definition_name = "Azure Event Hubs Data Receiver"
  principal_id         = var.identity.principal_id
}

resource "azurerm_role_assignment" "telemetry-hub-enriched-current-receiver" {
  scope                = azurerm_eventhub.data_enriched_hub.id
  role_definition_name = "Azure Event Hubs Data Receiver"
  principal_id         = data.azurerm_client_config.current.object_id
}