output "primary_blob_endpoint" {
  value = azurerm_storage_account.solution_storage.primary_blob_endpoint
}

output "account_name" {
  value = azurerm_storage_account.solution_storage.name
}