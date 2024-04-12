output "telemetry_queue_conn_string" {
  value = azurerm_servicebus_queue_authorization_rule.telemetry-queue-auth-role.primary_connection_string
}

output "servicebus_hostname" {
  value = "${azurerm_servicebus_namespace.sb.name}.servicebus.windows.net"
}