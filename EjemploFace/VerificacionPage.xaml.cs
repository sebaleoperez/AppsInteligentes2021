using System;
using System.Collections.Generic;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Xamarin.Forms;

namespace EjemploFace
{
    public partial class VerificacionPage : ContentPage
    {
        public VerificacionPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            imagen1.Source = entryImagen1.Text;
            imagen2.Source = entryImagen2.Text;

            // Creo el cliente
            FaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(AppSettings.FaceAPIKey));
            faceClient.Endpoint = AppSettings.FaceAPIEndPoint;

            IList<FaceAttributeType> atributos = new List<FaceAttributeType>();
            atributos.Add(FaceAttributeType.Age);
            atributos.Add(FaceAttributeType.Emotion);
            atributos.Add(FaceAttributeType.Hair);
            atributos.Add(FaceAttributeType.Glasses);

            // Detecto las caras
            var resultadoImagen1 = await faceClient.Face.DetectWithUrlAsync(entryImagen1.Text, true, true, atributos);

            var resultadoImagen2 = await faceClient.Face.DetectWithUrlAsync(entryImagen2.Text, true, true, atributos);

            // Verifico las caras
            var verificationResult = await faceClient.Face.VerifyFaceToFaceAsync(
                resultadoImagen1[0].FaceId.Value, resultadoImagen2[0].FaceId.Value);

            labelResultado.Text = verificationResult.Confidence.ToString();
        }
    }
}
