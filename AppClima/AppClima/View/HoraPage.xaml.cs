using AppClima.Global;
using AppClima.Model;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppClima.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HoraPage : ContentPage
    {
        ObservableCollection<TEMPERATURAHORA> list = new ObservableCollection<TEMPERATURAHORA>();
        public CLIMA clima { get; private set; }

        string latitude;
        string longitude;

        public HoraPage()
        {
            InitializeComponent();
            _list.BindingContext = list;
            trazerTemperaturaHora();
        }

        public async void trazerTemperaturaHora()
        {
            CLIMA climaHora;

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

                climaHora = climaJson;

                int i = 0;

                lblSumario.Text = climaHora.hourly.summary.ToString();

                var listAsync = climaHora.hourly.data;      

                if (listAsync != null)
                {
                    for (i = 0; i <= 23; i++)
                    {
                        var dataTemperatura = DateTime.Now.AddHours(i + 1);

                        TEMPERATURAHORA hora = new TEMPERATURAHORA();
                        hora.temperature = listAsync[i].temperature + "°C";
                        hora.summary = listAsync[i].summary;
                        hora.icon = listAsync[i].icon.ToString().Replace("-", "_");
                        hora.precipProbability = listAsync[i].precipProbability.ToString() + "%";
                        hora.hora = dataTemperatura.Hour.ToString() + ":00";
                        list.Add(hora);
                    }
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Recurso não suportado no device
                await DisplayAlert("ErroHora ", "Recurso não suportado", "Ok");
            }
            catch (PermissionException pEx)
            {
                // Tratando erro de permissão
                await DisplayAlert("ErroHora ", "Permissão recusada. ", "Ok");
            }
            catch (Exception ex)
            {
                // Não foi possivel obter localização
                await DisplayAlert("ErroHora ", "Não foi possivel obter a localização", "Ok");
            }


        }
    }
}