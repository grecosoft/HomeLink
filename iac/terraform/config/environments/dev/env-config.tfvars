solution = {
  cluster_resource_group = "kube-cluster"
  cluster_name = "aksmscluster"
  registry_name = "aksmsclusteracr"
  github_account = "grecosoft"
  environment = "dev"
  name     = "HomeLink"
  location = "eastus"
  keyvault = {
    sku                        = "standard"
    soft_delete_retention_days = 7
    purge_protection_enabled   = true
  }
  appconfig = {
    namespace = "app-config-system"
  }
}