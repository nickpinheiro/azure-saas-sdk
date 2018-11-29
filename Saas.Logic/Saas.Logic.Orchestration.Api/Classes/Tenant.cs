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

        //
        // To contact the Azure Management Service we need it's base URL.
        //

        private static HttpClient httpClient = new HttpClient();
        private static AuthenticationContext authContext = null;
        private static ClientCredential clientCredential = null;

        public static async Task CreateTenantAsync(Models.Tenant tenant)
        {
            string tenantNameFormatted = tenant.Name.Replace(" ", string.Empty).ToLower();
            string resourceGroupName = tenantNameFormatted;

            //string databaseDeployment = "deploy-database-" + tenantNameFormatted;

            await CreateDatabaseAsync(resourceGroupName, tenantNameFormatted);
            //await CheckDeploymentStatusAsync("bd-prod-core", databaseDeployment);
            AddNewTenant(tenant.Name, tenant.ProductId);

            //return null;
        }

        private static async Task CreateDatabaseAsync(string resourceGroupName, string tenantName)
        {
            authContext = new AuthenticationContext(authority);
            clientCredential = new ClientCredential(clientId, appKey);

            AuthenticationResult result = await GetAccessToken();

            // Add the access token to the authorization header of the request.
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

            // JSON encode To Do item and PUT to the Azure Management Service API.

            StringContent stringContent = new StringContent("{properties: {\"templateLink\": {\"uri\": \"https://contosodevstorageacct.blob.core.windows.net/templates/azuredeploy.json\",\"contentVersion\": \"1.0.0.0\"},\"mode\": \"Incremental\",\"parameters\": {\"databaseName\": {\"value\": \"contoso-dev-plan3-" + tenantName + "-sql\"}}}}}", UnicodeEncoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PutAsync(azureManagementServiceBaseAddress + "subscriptions/" + ConfigurationManager.AppSettings["ida:Subscription"] + "/resourcegroups/contoso-dev-foundation/providers/Microsoft.Resources/deployments/deploy-database-" + tenantName + "?api-version=2015-01-01", stringContent);
            if (response.IsSuccessStatusCode == true)
            {

            }
            else
            {

            }
        }

        //private static async Task CheckDeploymentStatusAsync(string resourceGroupName, string deploymentName)
        //{
        //    string provisioningState = await GetDeploymentAsync(resourceGroupName, deploymentName);

        //    while (!IsDeploymentSucceeded(provisioningState))
        //    {
        //        await Task.Delay(30000);  // Delay for 30 seconds and check deployment status again
        //        provisioningState = await GetDeploymentAsync(resourceGroupName, deploymentName);
        //    }
        //}

        //private static async Task<string> GetDeploymentAsync(string resourceGroupName, string deploymentName)
        //{
        //    // https://docs.microsoft.com/en-us/rest/api/resources/deployments/get#deploymentpropertiesextended

        //    //
        //    // Get database deployment and check status.
        //    //
        //    AuthenticationResult result = await GetAccessToken();

        //    // Add the access token to the authorization header of the request.
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

        //    HttpResponseMessage response = await httpClient.GetAsync(azureManagementServiceBaseAddress + "subscriptions/" + ConfigurationManager.AppSettings["ida:Subscription"] + "/resourcegroups/" + resourceGroupName + "/deployments/" + deploymentName + "?api-version=2018-02-01");
        //    var json = await response.Content.ReadAsStringAsync();

        //    if (response.IsSuccessStatusCode == true)
        //    {
        //        Deployment deployment = JsonConvert.DeserializeObject<Deployment>(json);
        //        string provisioningState = deployment.properties.provisioningState;
        //        return provisioningState;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

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
                    cmd.Parameters.AddWithValue("@productid", productId);

                    //SqlParameter returnValue = new SqlParameter();
                    //returnValue.Direction = System.Data.ParameterDirection.ReturnValue;
                    //cmd.Parameters.Add(returnValue);

                    cmd.ExecuteNonQuery();

                    //int customerId = (int)returnValue.Value;

                    //return customerId;
                }
            }

        }

        //private static bool IsDeploymentSucceeded(string provisioningState)
        //{
        //    if (provisioningState == "Succeeded")
        //    {
        //        return true; // Deployment is complete
        //    }
        //    else
        //    {
        //        return false; // Deployment is running
        //    }
        //}

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