using weatherForm.DTO;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace weatherForm
{
    public partial class Form1 : Form
    {
        HttpClient client;
        private bool isFirstRun = true;
        public Form1()
        {
            InitializeComponent();
            client = new HttpClient();
        }
        private void Inilize_comboBox()
        {
            string url = $"http://localhost:31002/api/countery";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                List<Country> data = response.Content.ReadAsAsync<List<Country>>().Result;
                cb_countries.DataSource = data;
                cb_countries.DisplayMember = "Name";
                cb_countries.ValueMember = "Id";
            }
        }

        private void displayData(Weateher data)
        {
            lb_capital.Text = data.Location.name;
            lb_Countery.Text = data.Location.country;
            lb_region.Text = data.Location.region;
            lb_lat.Text = data.Location.lat.ToString();
            lb_log.Text = data.Location.lon.ToString();
            lb_localtime.Text = data.Location.localtime;
            lb_Temp_c.Text = data.current.temp_c.ToString();
            lb_temp_f.Text = data.current.temp_f.ToString();
            lb_contion_text.Text = data.current.condition.text;
            lb_icon.Text = data.current.condition.icon;
        }
        private void emptyAllLabel()
        {
            lb_capital.Text = string.Empty;
            lb_Countery.Text = string.Empty;
            lb_region.Text = string.Empty;
            lb_lat.Text = string.Empty;
            lb_log.Text = string.Empty;
            lb_localtime.Text = string.Empty;
            lb_Temp_c.Text = string.Empty;
            lb_temp_f.Text = string.Empty;
            lb_contion_text.Text = string.Empty;
            lb_icon.Text = string.Empty;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCounteryName.Text))
            {
                MessageBox.Show($"string is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string url = $"http://localhost:31002/api/Weather?name={txtCounteryName.Text.Trim()}";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Weateher data = response.Content.ReadAsAsync<Weateher>().Result;
                displayData(data);
            }
            else
            {
                MessageBox.Show($"{response.StatusCode.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                emptyAllLabel();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Inilize_comboBox();
        }

        private void btn_addcountery_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_country.Text))
            {
                MessageBox.Show($"string is empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Country country = new Country()
            {
                Name = txt_country.Text.Trim(),
            };

            string url = $"http://localhost:31002/api/countery";
            HttpResponseMessage response = client.PostAsJsonAsync<Country>(url, country).Result;
            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"countery  {country.Name} Added Successfuly ");
                Inilize_comboBox();
            }
            else
            {
                MessageBox.Show($"error {response.StatusCode.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cb_countries_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_countries_SelectedValueChanged(object sender, EventArgs e)
        {
            

            if (cb_countries.SelectedItem != null)
            {
                Country selectedCountry = (Country)cb_countries.SelectedItem;
                string selectedCountryName = selectedCountry.Name;
                string url = $"http://localhost:31002/api/Weather?name={selectedCountryName}";
                HttpResponseMessage response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Weateher data = response.Content.ReadAsAsync<Weateher>().Result;
                    displayData(data);
                }
                else
                {
                    MessageBox.Show($"{response.StatusCode.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    emptyAllLabel();
                }

            }

            

        }

       
    }
}
