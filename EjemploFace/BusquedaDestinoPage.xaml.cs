using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bing.ImageSearch;
using Microsoft.Bing.NewsSearch;
using Xamarin.Forms;

namespace EjemploFace
{
    public partial class BusquedaDestinoPage : ContentPage
    {
        public BusquedaDestinoPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            aiCargaImagenes.IsVisible = true;
            aiCargaImagenes.IsRunning = true;
            pbProgreso.IsVisible = true;

            var clienteImagenes = new ImageSearchClient(new Microsoft.Bing.ImageSearch.ApiKeyServiceClientCredentials(AppSettings.SearchAPIKey));

            var resultadoImagenes = await clienteImagenes.Images.SearchAsync(entryDestino.Text, count: 5, minHeight: 100, minWidth: 100, maxHeight:500, maxWidth: 500, imageType: "Photo");

            if (resultadoImagenes.Value.Count > 0)
            {
                List<ResultadoImagen> resultados = new List<ResultadoImagen>();

                foreach(var item in resultadoImagenes.Value)
                {
                    resultados.Add(new ResultadoImagen() { URLImagen = item.ContentUrl });
                    pbProgreso.Progress += 0.1;
                    await Task.Delay(1000);
                }

                cvImagenes.ItemsSource = resultados;
            }

            var clienteNoticias = new NewsSearchClient(new Microsoft.Bing.NewsSearch.ApiKeyServiceClientCredentials(AppSettings.SearchAPIKey));

            var resultadoNoticias = await clienteNoticias.News.SearchAsync(entryDestino.Text, count: 5);

            if (resultadoNoticias.Value.Count > 0)
            {
                List<ResultadoNoticias> resultados = new List<ResultadoNoticias>();

                foreach(var item in resultadoNoticias.Value)
                {
                    resultados.Add(new ResultadoNoticias() { Titulo = item.Description, URLImagen = item.Image.Thumbnail.ContentUrl });
                    pbProgreso.Progress += 0.1;
                    await Task.Delay(1000);
                }

                cvNoticias.ItemsSource = resultados;
            }

            aiCargaImagenes.IsVisible = false;
            aiCargaImagenes.IsRunning = false;
            pbProgreso.IsVisible = false;
        }
    }

    public class ResultadoImagen
    {
        public string URLImagen { get; set; }
    }

    public class ResultadoNoticias
    {
        public string URLImagen { get; set; }
        public string Titulo { get; set; }
    }
}
