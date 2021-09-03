using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.AI.TextAnalytics;

namespace AnalisisSentimientos
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new TextAnalyticsClient(new Uri(AppSettings.Endpoint),
                new AzureKeyCredential(AppSettings.ApiKey));
            
            DetectedLanguage response = await client.DetectLanguageAsync("Mira, reconoce español con entidades en ingles: Estas claves de Cognitive Services se usan para obtener acceso a la API.");

            Console.WriteLine(response.Name);
            Console.WriteLine(response.ConfidenceScore);

            var responseRE = await client.RecognizeEntitiesAsync("Mira, reconoce español con entidades en ingles: Estas claves de Cognitive Services se usan para obtener acceso a la API.");

            foreach (CategorizedEntity entity in responseRE.Value)
            {
                Console.WriteLine(entity.Text);
            }
            
            List<string> lista = new List<string>();
            for (int i = 0; i <= 10; i++)
            {
                lista.Add("Vale la pena. Lo unico malo es el pitido que hace cuando lo inflas, pero es totalmente entendible. La verdad, si tenes dinero disponible y visitas seguidas, vale la pena comprar. Todo este texto lo escribo porque me piden que sea extenso, la verdad que con un 'esta bueno' alcanzaba,pero bueno");
                lista.Add("Los recientes y exitosos acuerdos de reestructuración de la deuda soberana en divisas con los acreedores privados han proporcionado un importante alivio del flujo de caja a corto plazo, pero se necesita un plan macroeconómico y estructural creíble y sólido que pueda ser apoyado por la comunidad internacional para mejorar la posición exterior de Argentina a medio plazo.");
            }

            var responseAS = await client.AnalyzeSentimentBatchAsync(lista);
            var sentimientos = responseAS.Value;

            foreach (AnalyzeSentimentResult sentimiento in sentimientos)
            {
                Console.WriteLine("El sentimiento encontrado fue: " + sentimiento.DocumentSentiment.Sentiment);
                Console.WriteLine("Puntaje positivo: " + sentimiento.DocumentSentiment.ConfidenceScores.Positive);
                Console.WriteLine("Puntaje negativo: " + sentimiento.DocumentSentiment.ConfidenceScores.Negative);
                Console.WriteLine("Puntaje neutral: " + sentimiento.DocumentSentiment.ConfidenceScores.Neutral);
            }
            
            var responseKP = await client.ExtractKeyPhrasesAsync("Vale la pena. Lo unico malo es el pitido que hace cuando lo inflas, pero es totalmente entendible. La verdad, si tenes dinero disponible y visitas seguidas, vale la pena comprar. Todo este texto lo escribo porque me piden que sea extenso, la verdad que con un 'esta bueno' alcanzaba,pero bueno");

            foreach (string frase in responseKP.Value)
            {
                Console.WriteLine(frase);
            }
            
        }
    }
}
