using DemoAppConsole;

const string hostname = "test-frank.canadacentral-1.ts.eventgrid.azure.net";
const string clientId = "client1-session1";
const string username = "client1-authnID";
const string x509Pem = "/home/kefran/Documents/certificats/client1-authnID.pem";
const string x509Key = "/home/kefran/Documents/certificats/client1-authnID.key";

var client = new MyMqttClient();
await client.ConnectAsync(hostname, clientId, username, x509Pem, x509Key);

while (true)
{
    await client.PublishAsync("test/client1/hello", "Hello world");
    await Task.Delay(1000);
}