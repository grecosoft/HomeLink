data "azurerm_client_config" "current" {}

resource "azurerm_digital_twins_instance" "dt" {
  name                = var.name
  resource_group_name = var.resource_group
  location            = var.location
}

resource "azurerm_role_assignment" "workload_data_owner" {
  scope                = azurerm_digital_twins_instance.dt.id
  role_definition_name = "Azure Digital Twins Data Owner"
  principal_id         = var.identity.principal_id
}

resource "azurerm_role_assignment" "data_owner" {
  scope                = azurerm_digital_twins_instance.dt.id
  role_definition_name = "Azure Digital Twins Data Owner"
  principal_id         = data.azurerm_client_config.current.object_id
}
