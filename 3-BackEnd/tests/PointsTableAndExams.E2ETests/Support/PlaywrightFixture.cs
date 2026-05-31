using Microsoft.Playwright;

namespace PointsTableAndExams.E2ETests.Support;

public sealed class PlaywrightFixture : IAsyncLifetime
{
    public IPlaywright Playwright { get; private set; } = null!;
    public IBrowser Browser { get; private set; } = null!;
    public IAPIRequestContext ApiContext { get; private set; } = null!;

    private const string BaseUrl = "https://localhost:7001";

    public async Task InitializeAsync()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        ApiContext = await Playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
        {
            BaseURL = BaseUrl,
            IgnoreHTTPSErrors = true
        });
    }

    public async Task DisposeAsync()
    {
        await ApiContext.DisposeAsync();
        await Browser.DisposeAsync();
        Playwright.Dispose();
    }
}
