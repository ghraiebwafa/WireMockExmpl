using NUnit.Framework;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

WireMockServer server;

[SetUp]
public void StartMockServer()
{
    server = WireMockServer.Start();
}

[Test]
public async Task Should_respond_to_request()
{
    // Arrange (start WireMock.Net server)
    server
        .Given(Request.Create().WithPath("/foo").UsingGet())
        .RespondWith(
            Response.Create()
                .WithStatusCode(200)
                .WithBody(@"{ ""msg"": ""Hello world!"" }")
        );

    // Act (use a HttpClient which connects to the URL where WireMock.Net is running)
    var response = await new HttpClient().GetAsync($"{server.Urls[0]}/foo");
    
    // Assert
    Check.That(response).IsEqualTo(EXPECTED_RESULT);
}

[TearDown]
public void ShutdownServer()
{
    server.Stop();
}