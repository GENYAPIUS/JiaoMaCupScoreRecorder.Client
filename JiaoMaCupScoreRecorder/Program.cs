using JiaoMaCupScoreRecorder;
using JiaoMaCupScoreRecorder.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddGoogleSheetsApiAuth(builder.Configuration);
builder.Services.AddSpreadSheetData(builder.Configuration);
builder.Services.AddHttpClient();
builder.Services.AddServices();
await builder.Build().RunAsync();