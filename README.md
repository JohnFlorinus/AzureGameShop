# Swedish Documentation - Azure Deployment
Dokumentation för en inlämningsuppgift i min Azure kurs<br>
App Service(Web App) med GitHub Actions, Storage Account, Application Insights, IAM Access-Control & Azure Key Vault 
<h2>Steg 1: Skapa Repository</h2>
1. git clone av https://github.com/Degendeg/cua24s_gamestore<br/>
2. Ta bort git filer och skapa ett nytt repository med namn "AzureGameShop" via Visual Studio<br/>
<h2>Steg 2: Skapa App Service</h2>
1. Gå till App Services -> Create -> Web App<br/>
2. Lägg in i ny resource group "GameShop_rg"<br/>
3. Specs:<br/>
<i><b>
  Runtime stack: .NET 8.0<br/>
  OS: Windows<br/>
  Pricing: Free F1<br/>
</b></i>
4. Aktivera Continuous Deployment, koppla GitHub konto och välj repo "AzureGameShop"<br/>
5. Aktivera Application Insights<br/>
6. Lämna resterande inställningar som default och skapa App Service<br/><br>
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
Du kan även få en överblick och hantera personer och roller kopplade till App Servicen på IAM-sidan</b>
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
7: Här blev det fel. Jag måste få "parallelism" manuellt godkänd för mitt projekt för att använda Azure DevOps för CD. Jag testade att ändra min organisations policy till att tillåta public projects och skapa det återigen, men jag behövde fortfarande manuellt godkännande. Därför skickade jag formuläret men valde att fortsatt använda GitHub Actions.<br>
<h2>Steg 7: Storage Account</h2>
1: Storage Account -> Create<br>
2: Namnge "gameshopstorage" och välj resource group "GameShop_rg"<br>
3: Specs:<br>
<i>
  Primary Service: Azure Blob Storage<br>
  Redundancy: LRS<br>
  Advanced -> Aktivera "Allow enabling anonymous access on individual containers"<br>
</i>
4: Lämna resterande inställningar som default och skapa Storage Account<br>
5: gameshopstorage -> Data Storage -> Containers -> Skapa ny container<br>
6: Ange containernamn som "Images" och Anonymous Access Level till "Blob"<br>
7: Gå till "AzureGameShop" repo och öppna games.json filen, öppna och ladda ner bilderna kopplade till varje spel objekt och ladda upp i "Images" container<br>
8: Klicka på varje blobfil så ser man en URL attached, ersätt den gamla bildlänken i JSON filen med respektive ny URL.<br>
9: Pusha ändringarna av games.json till master och med GitHub Actions så kommer det automatiskt laddas upp till hemsidan<br><br>
<b>Du hämtar nu bilderna för varje spel från din egna fillagring på Azure istället för en extern källa. För att dubbelkolla kan du använda Inspect Element på hemsidan och kolla img src</b>
<h2>Steg 8: Azure Key Vault</h2>
1: Azure Key Vault -> Create<br>
2: Ange namn "gameshopvault" och resource group "GameShop_rg" med region "North Europe"<br>
3: Klicka på Create<br>
4: gameshopvault -> Objects -> Secrets -> Generate/Import<br>
5: Namnge "APIKEY" och sätt Secret Value till "verysecretstuff", sedan klicka på Create knappen<br>
6: Det gick inte att skapa APIKEY, och den säger att jag har inte tillstånd att se några hemligheter.<br>
7: Gå till Access Control (IAM) i gameshopvault och klicka på Add new role assignment<br>
8: Välj roll "Key Vault Administrator" och koppla rollen till mitt egna azurekonto, sedan Review+Create<br>
9: Skapa APIKEY secret igen, och nu funkar det<br>
10: Öppna repo i Visual Studio och klicka på Connected Services -> Add -> Azure Key Vault<br>
11: Välj gameshopvault och klicka Finish<br>
12: Gå till program.cs och lägg till:<br>
<pre>
builder.Configuration.AddAzureKeyVault(new Uri("https://gameshopvault.vault.azure.net/"), new DefaultAzureCredential());
</pre>
13: Gå till Controllers mappen och skapa APIController.cs med kod:<br>
<pre>
using Microsoft.AspNetCore.Mvc;
namespace GameStore.Controllers
{
    [Route("veryhiddenapikey")]
    public class APIController : Controller
    {
        private readonly IConfiguration _configuration;
        public APIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult GetSecretValue()
        {
            var apiKey = _configuration["APIKEY"];

            ViewBag.ApiKey = apiKey;

            return Content(apiKey, "text/html");
        }
    }
}
</pre>
14: Pusha till master, och nu så kan du se den super-duper hemliga api nyckeln som absolut inte ska visas till användare när du går till azuregameshop.azurewebsites.net/veryhiddenapikey
