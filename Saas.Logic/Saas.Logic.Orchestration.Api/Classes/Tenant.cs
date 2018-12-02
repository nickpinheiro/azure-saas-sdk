using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Saas.Logic.Orchestration.Api.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Saas.Logic.Orchestration.Api.Classes
{
    public class Tenant
    {
        private static string subscription = ConfigurationManager.AppSettings["ida:Subscription"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenant = ConfigurationManager.AppSettings["ida:Tenant"];
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string appKey = ConfigurationManager.AppSettings["ida:AppKey"];
        private static string resource = "https://management.azure.com/";
        private static string azureManagementServiceBaseAddress = ConfigurationManager.AppSettings["azure:ManagementServiceBaseAddress"];
        private static string authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenantId);

        private static HttpClient httpClient = new HttpClient();
        private static AuthenticationContext authContext = null;
        private static ClientCredential clientCredential = null;

        public static async Task CreateTenantAsync(Models.Tenant tenant)
        {
            string tenantNameFormatted = tenant.Name.Replace(" ", string.Empty).ToLower();
            string environmentName = ConfigurationManager.AppSettings["SaasEnvironmentName"];
            string foundationResourceGroupName = ConfigurationManager.AppSettings["SaasFoundationResourceGroupName"];
            string tenantResourceGroupName = ConfigurationManager.AppSettings["SaasTenantResourceGroupName"];
            string tenantPrefixName = ConfigurationManager.AppSettings["SaasProviderName"] + "-" + environmentName + "-plan3-";
            string appServicePlanName = ConfigurationManager.AppSettings["SaasProviderName"] + "-" + environmentName + "-tenantsp";
            string appServiceName = tenantPrefixName + tenantNameFormatted + "-web";
            string databaseServer = ConfigurationManager.AppSettings["SaasProviderName"] + environmentName+ "saas";
            string databaseName = tenantPrefixName + tenantNameFormatted +"-sql";

            AppSettings appSettings = new AppSettings();
            appSettings.BlobPath = "https://wingtipsaas.blob.core.windows.net/images-sa/";
            appSettings.ConnectionTimeOut = "30";
            appSettings.DatabasePassword = "pass@word1";
            appSettings.DatabaseServerPort = "1433";
            appSettings.DatabaseUser = "saasadmin";
            appSettings.ResetEventDates = "true";
            appSettings.ServicePlan = "Standard";
            appSettings.SqlProtocol = "tcp";
            appSettings.TenantServer = databaseServer;
            appSettings.TenantDatabase = databaseName;
            appSettings.LearnHowFooterUrl = "https://aka.ms/sqldbsaastutorial";
            appSettings.ASPNETCORE_ENVIRONMENT = "Production";
            appSettings.WEBSITE_NODE_DEFAULT_VERSION = "4.2.3";
            appSettings.DefaultRequestCulture = "en-us";

            AppConfig properties = new AppConfig();
            properties.Properties = appSettings;

            authContext = new AuthenticationContext(authority);
            clientCredential = new ClientCredential(clientId, appKey);

            await CreateDatabaseAsync(foundationResourceGroupName, tenantNameFormatted, databaseServer, databaseName);
            await CreateResourceGroupAsync(tenantResourceGroupName);
            await CreateAppServicePlanAsync(tenantResourceGroupName, appServicePlanName);
            await CreateAppServiceAsync(tenantResourceGroupName, appServiceName, appServicePlanName, tenantNameFormatted);
            await UpdateAppSettingsAsync(tenantResourceGroupName, appServiceName, properties, tenantNameFormatted);
            AddNewTenant(tenant.Name, tenant.ProductId);
        }

        private static async Task CreateDatabaseAsync(string foundationResourceGroupName, string tenantName, string databaseServer, string databaseName)
        {
            //
            // Create a Database.
            //
            AuthenticationResult result = await GetAccessToken();

            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // JSON encode To Do item and PUT to the Azure Management Service API.

            StringContent stringContent = new StringContent("{properties: {\"templateLink\": {\"uri\": \"https://contosodevartifacts.blob.core.windows.net/artifacts/templates/database/azuredeploy.json\",\"contentVersion\": \"1.0.0.0\"},\"mode\": \"Incremental\",\"parameters\": {\"databaseName\": {\"value\": \"" + databaseName + "\"}, \"databaseServer\": {\"value\": \"" + databaseServer + "\"}}}}}", UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(azureManagementServiceBaseAddress + "subscriptions/" + ConfigurationManager.AppSettings["ida:Subscription"] + "/resourcegroups/"+ foundationResourceGroupName +"/providers/Microsoft.Resources/deployments/saas-deploy-database-" + tenantName + "?api-version=2015-01-01", stringContent);
            if (response.IsSuccessStatusCode == true)
            {

            }
            else
            {

            }
        }

        private static async Task CreateResourceGroupAsync(string tenantResourceGroupName)
        {
            //
            // Create a Resource Group if it doesn't already exist.
            //
            AuthenticationResult result = await GetAccessToken();

            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // JSON encode To Do item and PUT to the Azure Management Service API.
            StringContent stringContent = new StringContent("{location: \"eastus\"}", UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(azureManagementServiceBaseAddress + "subscriptions/" + ConfigurationManager.AppSettings["ida:Subscription"] + "/resourcegroups/" + tenantResourceGroupName + "?api-version=2015-01-01", stringContent);

            if (response.IsSuccessStatusCode == true)
            {

            }
            else
            {

            }
        }

        private static async Task CreateAppServicePlanAsync(string tenantResourceGroupName, string appServicePlanName)
        {
            //
            // Create a Resource Group if it doesn't already exist.
            //
            AuthenticationResult result = await GetAccessToken();

            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // JSON encode To Do item and PUT to the Azure Management Service API.
            StringContent stringContent = new StringContent("{location: \"eastus\"}", UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(azureManagementServiceBaseAddress + "subscriptions/" + ConfigurationManager.AppSettings["ida:Subscription"] + "/resourcegroups/" + tenantResourceGroupName + "/providers/Microsoft.Web/serverfarms/"+ appServicePlanName +"?api-version=2016-09-01", stringContent);

            if (response.IsSuccessStatusCode == true)
            {

            }
            else
            {

            }
        }

        private static async Task CreateAppServiceAsync(string tenantResourceGroupName, string appServiceName, string appServicePlanName, string tenantName)
        {
            //
            // Create a Web Application.
            //
            AuthenticationResult result = await GetAccessToken();

            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // JSON encode To Do item and PUT to the Azure Management Service API.
            //Console.WriteLine("Creating App Service at {0}", timeNow);

            StringContent stringContent = new StringContent("{properties: {\"templateLink\": {\"uri\": \"https://contosodevartifacts.blob.core.windows.net/artifacts/templates/web/azuredeploy.json\",\"contentVersion\": \"1.0.0.0\"},\"mode\": \"Incremental\",\"parameters\": {\"TenantWebAppName\": {\"value\": \"" + appServiceName + "\"}, \"AppServicePlanName\": {\"value\": \"" + appServicePlanName + "\"}}}}}", UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(azureManagementServiceBaseAddress + "subscriptions/" + ConfigurationManager.AppSettings["ida:Subscription"] + "/resourcegroups/" + tenantResourceGroupName + "/providers/Microsoft.Resources/deployments/saas-deploy-web-" + tenantName + "?api-version=2015-01-01", stringContent);

            if (response.IsSuccessStatusCode == true)
            {

            }
            else
            {

            }
        }

        private static async Task UpdateAppSettingsAsync(string tenantResourceGroupName, string appServiceName, AppConfig properties, string tenantNameFormatted)
        {
            // Check deployment status of app service
            await CheckDeploymentStatusAsync(tenantResourceGroupName, "saas-deploy-web-" + tenantNameFormatted);

            //
            // Set Application Settings.
            //
            AuthenticationResult result = await GetAccessToken();

            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // JSON encode To Do item and PUT to the Azure Management Service API.
            string json = JsonConvert.SerializeObject(properties);
            StringContent stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            Uri uri = new Uri(azureManagementServiceBaseAddress + "subscriptions/" + ConfigurationManager.AppSettings["ida:Subscription"] + "/resourcegroups/" + tenantResourceGroupName + "/providers/Microsoft.Web/sites/" + appServiceName + "/config/appsettings?api-version=2016-08-01");

            HttpResponseMessage response = await httpClient.PutAsync(uri, stringContent);

            if (response.IsSuccessStatusCode == true)
            {

            }
            else
            {

            }
        }

        private static async Task CheckDeploymentStatusAsync(string resourceGroupName, string deploymentName)
        {
            string provisioningState = await GetDeploymentAsync(resourceGroupName, deploymentName);

            while (!IsDeploymentSucceeded(provisioningState))
            {
                await Task.Delay(30000);  // Delay for 30 seconds and check deployment status again
                provisioningState = await GetDeploymentAsync(resourceGroupName, deploymentName);
            }
        }

        private static async Task<string> GetDeploymentAsync(string resourceGroupName, string deploymentName)
        {
            // https://docs.microsoft.com/en-us/rest/api/resources/deployments/get#deploymentpropertiesextended

            //
            // Get database deployment and check status.
            //
            AuthenticationResult result = await GetAccessToken();

            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            HttpResponseMessage response = await httpClient.GetAsync(azureManagementServiceBaseAddress + "subscriptions/" + ConfigurationManager.AppSettings["ida:Subscription"] + "/resourcegroups/" + resourceGroupName + "/deployments/" + deploymentName + "?api-version=2018-02-01");
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode == true)
            {
                Deployment deployment = JsonConvert.DeserializeObject<Deployment>(json);
                string provisioningState = deployment.properties.provisioningState;
                return provisioningState;
            }
            else
            {
                return null;
            }
        }

        private static void AddNewTenant(string tenantName, int productId)
        {
            // Add tenant
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("usp_AddTenant", connection))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@name", tenantName);
                    cmd.Parameters.AddWithValue("@productId", productId);

                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private static bool IsDeploymentSucceeded(string provisioningState)
        {
            if (provisioningState == "Succeeded")
            {
                return true; // Deployment is complete
            }
            else
            {
                return false; // Deployment is running
            }
        }

        private static async Task<AuthenticationResult> GetAccessToken()
        {
            //
            // Get an access token from Azure AD using client credentials.
            // If the attempt to get a token fails because the server is unavailable, retry twice after 3 seconds each.
            //
            AuthenticationResult result = null;
            int retryCount = 0;
            bool retry = false;

            do
            {
                retry = false;
                try
                {
                    // ADAL includes an in memory cache, so this call will only send a message to the server if the cached token is expired.
                    result = await authContext.AcquireTokenAsync(resource, clientCredential);
                    return result;
                }
                catch (AdalException ex)
                {
                    if (ex.ErrorCode == "temporarily_unavailable")
                    {
                        retry = true;
                        retryCount++;
                        Thread.Sleep(3000);
                    }

                    Console.WriteLine(
                        String.Format("An error occurred while acquiring a token\nTime: {0}\nError: {1}\nRetry: {2}\n",
                        DateTime.Now.ToString(),
                        ex.ToString(),
                        retry.ToString()));
                }
            }
            while ((retry == true) && (retryCount < 3));

            if (result == null)
            {
                Console.WriteLine("Canceling attempt to contact the Azure Management Service.\n");
                return null;
            }

            return null;
        }
    }
}