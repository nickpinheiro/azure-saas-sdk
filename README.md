# Azure SaaS Development Kit
[![Deploy to Azure](http://azuredeploy.net/deploybutton.png)](https://azuredeploy.net/)

The Azure SaaS Development Kit provides tools to help developers deliver their applications as a service.  The tool kit includes recommended patterns and practices around SaaS platform architecture, onboarding new tenants, automated deployments, operational architecture, security and everything else you need to know to begin building SaaS solutions on the Azure PaaS and Serverless platform.   Technologies include:  Azure App Service, Azure Web Apps, Azure API Apps, Azure Functions, ASP.NET, Azure REST API, Azure Resource Manager (ARM), Azure Role Based Access Control (RBAC), CI/CD with Azure DevOps, Azure SQL and Azure Storage. 

## Features
- SaaS Foundational Layer Deployment ('Deploy to Azure' button)
- SaaS Provider Web App
- Tenant Orchestration API App
- Tenant Automated Deployment

## Setup and Configuration
1. Run the 'Deploy to Azure' button above
2. Once the deployment is complete, create an 'App Registration' for the Orchestration API App in Azure Active Directory
3. Provide the App Registration created in Step 2 with 'Contributer' level Access control (IAM) permissions to your Azure Subscription 
4. Add the following 'Application Settings' values relative to your Subscription in the Orchestration API App app service ([SaaS Provider]-[Environment]-provider-api)
- ida:Subscription
- ida:Tenant
- ida:TenantId
- ida:ClientId
- ida:AppKey
5. Load the SaaS Provider web app, select a Plan, provide a Tenant 'Name' and submit.  Your first tenant will be created in the '[SaaS Provider]-[Environment]-Tenants' Resource Group.

## Downloads
Download the companion slide deck from the 2018 Microsoft Azure + AI Conference:
https://downloader.modernappz.com/nickpinheiro/modernize-your-app-to-be-delivered-as-a-saas-service

## Wingtip Tickets SaaS - Standalone Application (aka App-per-tenant)
The Azure SaaS Development Kit builds on the Wingtip Tickets SaaS demo solution (Standalone Application (aka App-per-tenant)).  Additional tools and features include a SaaS Provider Web Application with pricing plans and automated tenant deployments via API App.
https://github.com/Microsoft/WingtipTicketsSaaS-StandaloneApp

## Subscribe for Updates
Subscribe for notifications of updates and new features:
https://nick.modernappz.com   

## License
The Azure SaaS Development Kit is licensed under the MIT license. See the LICENSE file for more details.