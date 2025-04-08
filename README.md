# Dokumentation - Cloud Deployment
<h2>Steg 1: Skapa Repository</h2>
1. git clone av https://github.com/Degendeg/cua24s_gamestore<br/>
2. Ta bort git filer och skapa ett nytt repository med namn "AzureGameShop" via Visual Studio<br/>
<h2>Steg 2: Skapa App Service</h2>
1. Välj "Web App" som typ<br/>
2. Lägg in i ny resource group "GameShop_rg"<br/>
3. Specs:<br/>
<i><b>
  Runtime stack: .NET 8.0<br/>
  OS: Windows<br/>
  Pricing: Free F1<br/>
</b></i>
4. Aktivera Continuous Deployment, koppla GitHub konto och välj repo "AzureGameShop"<br/>
5. Aktivera Application Insights<br/>
6. Lämna resterande inställningar som default och skapa App Service<br/>
<b>Du har nu en fungerande .NET MVC hemsida med Continuous Deployment via GitHub Actions och telemetri med App Insights</b>
<h2>Steg 3: Begränsa åtkomsten till App Service via IAM</h2>
1. App Service -> Access Control (IAM) -> Add -> Add Role Assignment<br/>
2. Välj "Reader" som roll (read-only access)<br/>
3. Välj en person i din organisation<br/>
4. Klicka på Review+Assign<br/>
5. Klicka på "Add -> Add Role Assignment" igen<br/>
6. Välj "Data Purger" som roll (Kan endast ta bort analytics data)<br/>
7. Välj en person i din organisation<br/>
8. Klicka på Review+Assign<br/><br/>
<b>Det finns nu två nya personer som har olika nivåer av åtkomst till Game Shop hemsidan<br>
Du kan även få en överblick av personer och roller kopplade till App Servicen och ta bort åtkomst på IAM-sidan</b>
<h2>Steg 4: Övervaka Application Insights</h2>
1: Investigate -> Transaction Search<br>
<b>Här kan du se den senaste telemetri data, såsom server requests tsm med response time<br><br>
OPTIONAL: Lägg till "App Insights" som en connected service i Visual Studio (via solution explorer) på repot och pusha.<br>
(Lägger till ett SDK som erbjuder mer tillgång till kategorisering och filtrering av analytics data i App Insights)</b>
<h2>Steg 5: Automatisk skalning</h2>
1: App Service -> Settings -> Scale out<br>
<b>Den lägsta pricing plan för automatiserad scalability var Basic med en månadskostnad på 54,75 USD/månad, så jag skippade detta steg</b>
<h2>Steg 6: CD Pipeline via Azure DevOps</h2>
<i>Continuous Deployment finns redan via GitHub Actions, detta steg är för att implementera CD med Azure DevOps</i><br>
1: Gå till https://aex.dev.azure.com/<br>
2: Välj "Create new organisation"<br>
3: Välj ett organisationsnamn och valfri region (Europe för mig)<br>
4: Skapa nytt projekt med namn "GameShopAssignment" och "Private" visibility (Public är avstängd för min organisation)<br>
5: GameShopAssignment -> Pipelines -> Pipelines -> New pipeline<br>
6: Välj GitHub -> Välj "AzureGameShop" repo -> Välj ASP.NET Core (pipeline template) -> Klicka "Save and run"<br>
7: Här blev det ett fel. Eftersom att mitt projekt är privat måste jag få "parallelism" manuellt godkänd för mitt projekt för att använda Azure DevOps för CD.<br>
8: Jag ändrade därför policyn av organisationen jag skapade för att tillåta public projects. Därefter gjorde jag steg 4-6 igen och nu funkade CD.<br>

<h2>Steg 7: Storage Account</h2>
