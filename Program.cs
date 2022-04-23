using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


var timer = new PeriodicTimer(TimeSpan.FromSeconds(20));
while (await timer.WaitForNextTickAsync())
{

    using (var driver = new ChromeDriver("."))
    {
        driver.Navigate().GoToUrl("https://www.nseindia.com/api/option-chain-indices?symbol=BANKNIFTY");

        var jsonBody = driver.FindElement(By.TagName("pre")).Text;

        dynamic doc1Definition = new
        {
            id = Guid.NewGuid(),
            body = jsonBody.Replace("\"", "'")
        };


        var endpoint = "https://{NAME}.documents.azure.com:443/";
        var masterKey = "{PRIMARY_KEY}";
        using (var client = new DocumentClient(new Uri(endpoint), masterKey))
        {
            var doc = await client.CreateDocumentAsync(
           UriFactory.CreateDocumentCollectionUri("{DB_NAME}", "{COLLECTION_NAME}"),
           doc1Definition);
        }
        driver.Close();

    }

}

    

