resource "azurerm_resource_group" "terraform" {
  name     = "${var.resource_group_name}"
  location = "eastus"
}

resource "azurerm_storage_account" "terraform" {
  name                     = "${var.storage_account_name}"
  resource_group_name      = azurerm_resource_group.terraform.name
  location                 = azurerm_resource_group.terraform.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "homelink-dev" {
  name                  = "homelink-dev"
  storage_account_name  = azurerm_storage_account.terraform.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "homelink-stg" {
  name                  = "homelink-stg"
  storage_account_name  = azurerm_storage_account.terraform.name
  container_access_type = "private"
}

resource "azurerm_storage_container" "homelink-prd" {
  name                  = "homelink-prd"
  storage_account_name  = azurerm_storage_account.terraform.name
  container_access_type = "private"
}