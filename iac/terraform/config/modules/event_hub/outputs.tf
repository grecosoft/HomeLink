output "telemetry_hub_conn_string" {
  value = azurerm_eventhub_authorization_rule.telemetry-hub-auth-role.primary_connection_string
}

output "event_hub_namespace_name" {
  value = azurerm_eventhub_namespace.event_hub_ns.name
}

output "event_hub_hostname" {
  value = "${azurerm_eventhub_namespace.event_hub_ns.name}.servicebus.windows.net"
}

output "device_data_hub_name" {
  value = azurerm_eventhub.data_hub.name
}

output "device_data_enriched_hub_name" {
  value = azurerm_eventhub.data_enriched_hub.name
}
