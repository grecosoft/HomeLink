data "terraform_remote_state" "solution" {
  backend = "azurerm"

  config = {
    resource_group_name  = var.resource_group_name
    storage_account_name = var.storage_account_name
    container_name       = var.container_name
    key                  = "solution.tfstate"
  }
}

locals {
  solution_resource_group_name     = data.terraform_remote_state.solution.outputs.resource_group_name
  solution_workload_identity       = data.terraform_remote_state.solution.outputs.workload_identity
  solution_key_vault_id            = data.terraform_remote_state.solution.outputs.key_vault_id
  solution_app_config_id           = data.terraform_remote_state.solution.outputs.app_config_id

  solution_event_hub_namespace_name         = data.terraform_remote_state.solution.outputs.event_hub_namespace_name
  solution_eventhub_hostname                = data.terraform_remote_state.solution.outputs.event_hub_hostname
  solution_device_data_hub_name             = data.terraform_remote_state.solution.outputs.device_data_hub_name
  solution_device_data_enriched_hub_name    = data.terraform_remote_state.solution.outputs.device_data_enriched_hub_name
  solution_storage_endpoint                 = data.terraform_remote_state.solution.outputs.primary_blob_endpoint
  solution_storage_account_name             = data.terraform_remote_state.solution.outputs.storage_account_name
} 