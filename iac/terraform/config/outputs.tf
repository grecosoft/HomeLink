output "resource_group_name" {
  value = azurerm_resource_group.solution_rg.name
}

output "workload_identity" {
  value = module.workload_identity.identity
}

output "key_vault_id" {
  value = module.key_vault.key_vault_id
}

output "app_config_id" {
  value = module.app_config.app_config_id
}

output "GITHUB_SECRETS" {
  value = {
    Description           = "Add the following as Github Repository Secrets for Environment: ${var.solution.environment}"
    AZURE_TENANT_ID       = azuread_service_principal.github_workflow_sp.application_tenant_id
    AZURE_CLIENT_ID       = azuread_application.github_workflow.client_id
    AZURE_SUBSCRIPTION_ID = data.azurerm_subscription.current.subscription_id,
    APP_CONFIG_GROUP      = azurerm_resource_group.solution_rg.name,
    APP_CONFIG_NAME       = module.app_config.app_config_name
  }
}


output "primary_blob_endpoint" {
  value = module.storage_account.primary_blob_endpoint
}

output "storage_account_name" {
  value = module.storage_account.account_name
}

output "digital_twins_host_name" {
  value = module.digital_twins.digital_twins_host_name
}

output "servicebus_hostname" {
  value = module.service_bus.servicebus_hostname
}

// TODO:  Encapsulate these related fields into an object and
// other related properties:
output "event_hub_namespace_name" {
  value = module.event_hub.event_hub_namespace_name
}

output "event_hub_hostname" {
  value = module.event_hub.event_hub_hostname
}

output "device_data_hub_name" {
  value = module.event_hub.device_data_hub_name
}

output "device_data_enriched_hub_name" {
  value = module.event_hub.device_data_enriched_hub_name
}