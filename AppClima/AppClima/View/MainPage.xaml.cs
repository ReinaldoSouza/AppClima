using AppClima.Global;
using AppClima.Model;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppClima.View
{
    public partial class MainPage : ContentPage
    {
        string latitude;
        string longitude;
        
        public MainPage()
        {
            InitializeComponent();
            TrazerClimaAtual();
        }

        //Método pega latitude e longitude da posição do celular e mostra a situação do clima da localização do usuário.
        public async void TrazerClimaAtual()
        {
            var loadingPage = new LoadingPage();
            await Navigation.PushPopupAsync(loadingPage);
            await Task.Delay(5000);

            CLIMA climaAtual;

            try
            {
                //verifica se o aplicativo tem a permissão de coletar as informações de localização do aparelho
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if (status != PermissionStatus.Granted)
                {
                 var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                   if (results.ContainsKey(Permission.Location))
                       status = results[Permission.Location];
                }

                var localizacao = await Geolocation.GetLastKnownLocationAsync();
                if (localizacao != null)
                {
                    latitude = localizacao.Latitude.ToString();
                    longitude = localizacao.Longitude.ToString();
                }

                HttpClient cliente = new HttpClient();

                string uri = string.Format(Constantes.URL_DARK_SKY, Constantes.SECRET_KEY, latitude, longitude);

                string resultado = await cliente.GetStringAsync(uri);

                var climaJson = JsonConvert.DeserializeObject<CLIMA>(resultado);

                climaAtual = climaJson;

                var atual = climaAtual.currently;

                string[] cidade = climaAtual.timezone.ToString().Split('/');

                lblCidade.Text = cidade[1].ToString().Replace("_", " ") + ", " + cidade[0].ToString().Replace("_", " ");
                lblEstadoTemperatura.Text = atual.summary.ToString();

                imgIconeClima.Source = atual.icon.ToString().Replace("-", "_");

                lblTemperatura.Text = climaAtual.currently.temperature + "°C";

                lblSensacao.Text = "Sensação Térmica de " + climaAtual.currently.apparentTemperature + "°C";

                await Navigation.RemovePopupPageAsync(loadingPage);

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Recurso não suportado no device
                await DisplayAlert("Erro ", "Recurso não suportado", "Ok");
            }
            catch (PermissionException pEx)
            {
                // Tratando erro de permissão
                await DisplayAlert("Erro: ", "Permissão recusada. ", "Ok");
            }
            catch (Exception ex)
            {
                // Não foi possivel obter localização
                await DisplayAlert("Erro : ", "Não foi possivel obter a localização", "Ok");
            }
        }

        
    }
}
