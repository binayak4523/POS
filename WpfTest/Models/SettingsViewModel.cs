using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;

namespace WpfTest.Models
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private Company _company;
        private ObservableCollection<string> _currencies;
        private ObservableCollection<string> _languages;

        public Company Company
        {
            get => _company;
            set
            {
                _company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        public ObservableCollection<string> Currencies
        {
            get => _currencies;
            set
            {
                _currencies = value;
                OnPropertyChanged(nameof(Currencies));
            }
        }

        public ObservableCollection<string> Languages
        {
            get => _languages;
            set
            {
                _languages = value;
                OnPropertyChanged(nameof(Languages));
            }
        }

        public SettingsViewModel()
        {
            LoadCurrencies();
            LoadLanguages();
            LoadCompanyData();
        }

        private void LoadCompanyData()
        {
            using (var connection = new SQLiteConnection("Data Source=pos.db;Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Company LIMIT 1", connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Company = new Company
                        {
                            StoreName = reader["StoreName"].ToString(),
                            Location = reader["Location"].ToString(),
                            Language = reader["Language"].ToString(),
                            Currency = reader["Currency"].ToString(),
                            CurrentPassword = reader["CurrentPassword"].ToString()
                        };
                    }
                }
            }
        }

        private void LoadCurrencies()
        {
            Currencies = new ObservableCollection<string>();
            using (var connection = new SQLiteConnection("Data Source=pos.db;Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT CurrencyCode FROM Currency", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Currencies.Add(reader["CurrencyCode"].ToString());
                    }
                }
            }
        }

        private void LoadLanguages()
        {
            Languages = new ObservableCollection<string>();
            using (var connection = new SQLiteConnection("Data Source=pos.db;Version=3;"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT LanguageName FROM Language", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Languages.Add(reader["LanguageName"].ToString());
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Company
    {
        public string StoreName { get; set; }
        public string Location { get; set; }
        public string Language { get; set; }  // Single selected language
        public string Currency { get; set; }
        public string CurrentPassword { get; set; }
    }
}