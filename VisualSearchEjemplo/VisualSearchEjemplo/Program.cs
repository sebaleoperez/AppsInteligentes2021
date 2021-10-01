using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Bing.VisualSearch;
using Newtonsoft.Json;

namespace VisualSearchEjemplo
{
    class Program
    {
        async static Task Main(string[] args)
        {

            FileStream stream = new FileStream("../../../imagen.jpeg" , FileMode.Open);

            var cliente = new VisualSearchClient(new Microsoft.Bing.VisualSearch.ApiKeyServiceClientCredentials(AppSettings.ApiKey));

            Console.WriteLine("Iniciando llamada al visual search...");

            var resultados = await cliente.Images.VisualSearchMethodAsync(image: stream);

            if (resultados.Tags.Count > 0)
            {
                foreach(var tag in resultados.Tags)
                {
                    Console.WriteLine(tag.Actions[0].DisplayName);
                }
            }

            Console.WriteLine("Llamada a Visual Search terminada.");

            Stream streamCV = File.OpenRead("../../../imagen.jpeg");

            Console.WriteLine("Iniciando llamada al computer vision...");

            var clienteCV = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(AppSettings.ApiKeyCV));
            clienteCV.Endpoint = AppSettings.EndpointCV;

            var resultadosCV = await clienteCV.DescribeImageInStreamAsync(streamCV);

            // Informacion del cliente http de traduccion
            string route = "/translate?api-version=3.0&from=en&to=es";

            if (resultadosCV.Captions.Count > 0)
            {
                foreach(var cap in resultadosCV.Captions)
                {
                    object[] body = new object[] { new { Text = cap.Text } };
                    var requestBody = JsonConvert.SerializeObject(body);

                    using (var client = new HttpClient())
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri("https://api.cognitive.microsofttranslator.com/" + route);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                        request.Headers.Add("Ocp-Apim-Subscription-Key", AppSettings.TranslateKey);
                        request.Headers.Add("Ocp-Apim-Subscription-Region", "eastus");

                        HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                        string result = await response.Content.ReadAsStringAsync();

                        Console.WriteLine(result);
                    }
                }
            }

            Console.WriteLine("Llamada a computer vision terminada.");
        }
    }
}
