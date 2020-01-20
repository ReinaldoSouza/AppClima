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
    public partial class SemanaPage : ContentPage
    {
        ObservableCollection<TEMPERATURASEMANA> list = new ObservableCollection<TEMPERATURASEMANA>();
        public CLIMA clima { get; private set; }

        string latitude;
        string longitude;
        public SemanaPage()
        {
            InitializeComponent();
            _list.BindingContext = list;
            trazerTemperaturaSemana();
        }


        public async void trazerTemperaturaSemana()
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

                lblSumario.Text = climaHora.daily.summary.ToString();

                var listAsync = climaHora.daily.data;

                if (listAsync != null)
                {
                    for (i = 0; i <= 6; i++)
                    {
                        var dataTemperatura = DateTime.Now.AddDays(i + 1);

                        TEMPERATURASEMANA dia = new TEMPERATURASEMANA();
                        dia.temperatureHigh = "Máxima de: " + listAsync[i].temperatureHigh + "°C" ;
                        dia.temperatureLow = "Mínima de: " + listAsync[i].temperatureLow + "°C";
                        dia.summary = listAsync[i].summary;
                        dia.icon = listAsync[i].icon.ToString().Replace("-", "_");
                        dia.precipProbability = listAsync[i].precipProbability + "%";
                        dia.diaSemana = dataTemperatura.DayOfWeek.ToString();
                        list.Add(dia);
                    }
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Recurso não suportado no device
                await DisplayAlert("ErroSemana ", "Recurso não suportado", "Ok");
            }
            catch (PermissionException pEx)
            {
                // Tratando erro de permissão
                await DisplayAlert("ErroSemana: ", "Permissão recusada. ", "Ok");
            }
            catch (Exception ex)
            {
                // Não foi possivel obter localização
                await DisplayAlert("ErroSemana : ", "Não foi possivel obter a localização", "Ok");
            }


        }
    }
}