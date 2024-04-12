data "azurerm_client_config" "current" {}

// Service bus used by the solution and shared by all 
// microserivces defined within the solution.
resource "azurerm_servicebus_namespace" "sb" {
  name                = "${var.name}-${var.unique_postfix}"
  location            = var.location
  resource_group_name = var.resource_group
  sku                 = "Standard"
}

// The queue to which device telemtry data will be sent by IoT hub.
resource "azurerm_servicebus_queue" "telemetry-queue" {
  name         = "device-telemetry"
  namespace_id = azurerm_servicebus_namespace.sb.id

  enable_partitioning = true
}

resource "azurerm_servicebus_queue_authorization_rule" "telemetry-queue-auth-role" {
  name     = "telemetry-queue-auth-role"
  queue_id = azurerm_servicebus_queue.telemetry-queue.id

  listen = false
  send   = true
  manage = false
}

resource "azurerm_role_assignment" "telemetry-queue" {
  scope                = azurerm_servicebus_queue.telemetry-queue.id
  role_definition_name = "Azure Service Bus Data Receiver"
  principal_id         = var.identity.principal_id
}

resource "azurerm_role_assignment" "telemetry-queue-current" {
  scope                = azurerm_servicebus_queue.telemetry-queue.id
  role_definition_name = "Azure Service Bus Data Receiver"
  principal_id         = data.azurerm_client_config.current.object_id
}