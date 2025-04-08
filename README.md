# AzureGameShop Dokumentation
<h2>Steg 1: Skapa Repository</h2>
1. git clone av https://github.com/Degendeg/cua24s_gamestore<br/>
2. Skapa nytt repository med namn "AzureGameShop"<br/>
<h2>Steg 2: Skapa App Service</h2>
1. Välj "Web App" som typ<br/>
2. Lägg in i ny resource group "GameShop_rg"<br/>
3. Väljs Specs:<br/>
<i><b>
  Runtime stack: .NET 8.0<br/>
  OS: Windows<br/>
  Pricing: Free F1<br/>
</b></i>
4. Aktivera Continuous Deployment och välj "AzureGameShop" som repo<br/>
5. Aktivera Application Insights<br/>
6. Lämna resterande inställningar som default och skapa App Service<br/>
<h2>Steg 3: Begränsa åtkomsten till App Service via IAM</h2>
1. App Service -> Access Control (IAM) -> Add -> Add Role Assignment"<br/>
2. Välj "Reader" som roll (read-only access)<br/>
3. Välj en person i din organisation<br/>
4. Klicka på Review+Assign<br/>
5. Klicka på "Add -> Add Role Assignment" igen<br/>
6. Välj "Data Purger" som roll (Kan endast ta bort analytics data)<br/>
7. Välj en person i din organisation<br/>
8. Klicka på Review+Assign<br/>
<b>Det finns nu två nya personer som har olika nivåer av åtkomst till Game Shop hemsidan</b>
