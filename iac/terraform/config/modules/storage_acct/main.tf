resource "azurerm_storage_account" "solution_storage" {
  name                     = "${var.name}${var.unique_postfix}"
  resource_group_name      = var.resource_group
  location                 = var.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}