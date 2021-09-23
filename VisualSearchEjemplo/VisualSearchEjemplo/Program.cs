using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Bing.VisualSearch;

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

            if (resultadosCV.Captions.Count > 0)
            {
                foreach(var cap in resultadosCV.Captions)
                {
                    Console.WriteLine(cap.Text);
                }
            }

            Console.WriteLine("Llamada a computer vision terminada.");
        }
    }
}
