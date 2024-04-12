resource "azurerm_iotcentral_application" "iot_central" {
  name                = "${var.name}-${var.unique_postfix}"
  resource_group_name = var.resource_group
  location            = var.location
  sub_domain          = "${var.name}-${var.unique_postfix}"

  display_name = "${var.name}-${var.unique_postfix}"
  sku          = "ST1"
}