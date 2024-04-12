using System.Security.Cryptography.X509Certificates;
using MQTTnet;
using MQTTnet.Client;

namespace DemoAppConsole;

public class MyMqttClient
{
    private readonly IMqttClient _mqttClient = new MqttFactory().CreateMqttClient();
    public async Task ConnectAsync(string hostname, string clientId, string username, string x509Pem, string x509Key)
    {
        var certificate = new X509Certificate2(X509Certificate2.CreateFromPemFile(x509Pem, x509Key).Export(X509ContentType.Pkcs12));
        
        var connect = await _mqttClient.ConnectAsync(new MqttClientOptionsBuilder()
            .WithTcpServer(hostname, 8883)
            .WithClientId(clientId)
            .WithCredentials(username, "")  //use client authentication name in the username
            .WithTlsOptions(
                o =>
                {
                    o.WithClientCertificates(new List<X509Certificate2> { certificate });
                })
            .Build());

        Console.WriteLine($"Client Connected: {_mqttClient.IsConnected} with CONNACK: {connect.ResultCode}");
        
        await _mqttClient.SubscribeAsync("test/+/hello");
        
        _mqttClient.ApplicationMessageReceivedAsync += async m => 
            await Console.Out.WriteAsync($"Received message on topic: '{m.ApplicationMessage.Topic}' with content: '{m.ApplicationMessage.ConvertPayloadToString()}'\n\n");
    }
    
    public Task PublishAsync(string topic, string payload)
    {
        return _mqttClient.PublishStringAsync("test/client1/hello", "hello world!");
    }
}