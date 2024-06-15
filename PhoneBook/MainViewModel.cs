using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PhoneBook
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private const string ContactsFilePath = "contacts.json";

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Contact> contacts = new ObservableCollection<Contact>();
        public ObservableCollection<Contact> Contacts
        {
            get => contacts;
            set
            {
                contacts = value;
                OnPropertyChanged();
                UpdateFilteredContacts();
            }
        }

        private ObservableCollection<Contact> filteredContacts = new ObservableCollection<Contact>();
        public ObservableCollection<Contact> FilteredContacts
        {
            get => filteredContacts;
            private set
            {
                filteredContacts = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Contact> birthdaysThisWeek = new ObservableCollection<Contact>();
        public ObservableCollection<Contact> BirthdaysThisWeek
        {
            get => birthdaysThisWeek;
            private set
            {
                birthdaysThisWeek = value;
                OnPropertyChanged();
            }
        }

        private Contact selectedContact;
        public Contact SelectedContact
        {
            get => selectedContact;
            set
            {
                selectedContact = value;
                if (selectedContact != null)
                    FillForm(selectedContact);
                else
                    ResetProperties();
                CheckDeleteButtonAvailablity();
                OnPropertyChanged();
            }
        }

        private string searchQuery;
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                searchQuery = value;
                OnPropertyChanged();
                UpdateFilteredContacts();
            }
        }

        private string lastname;
        private string firstname;
        private string email;
        private string phoneNumber;
        private DateTime dateOfBirth;

        public string Lastname
        {
            get { return lastname; }
            set
            {
                lastname = value;
                CheckSaveButtonAvailablity();
                OnPropertyChanged();
            }
        }
        public string Firstname
        {
            get { return firstname; }
            set
            {
                firstname = value;
                CheckSaveButtonAvailablity();
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                CheckSaveButtonAvailablity();
                OnPropertyChanged();
            }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                CheckSaveButtonAvailablity();
                OnPropertyChanged();
            }
        }
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                CheckSaveButtonAvailablity();
                OnPropertyChanged();
            }
        }

        private bool saveButtonEnabled;
        private bool deleteButtonEnabled;
        public bool SaveButtonEnabled
        {
            get { return saveButtonEnabled; }
            private set
            {
                saveButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool DeleteButtonEnabled
        {
            get { return deleteButtonEnabled; }
            private set
            {
                deleteButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand ShowBirthdaysCommand { get; set; }

        public MainViewModel()
        {
            SaveCommand = new ActionCommand(SaveCommandAction);
            DeleteCommand = new ActionCommand(DeleteCommandAction);
            SearchCommand = new ActionCommand(SearchCommandAction);
            ResetCommand = new ActionCommand(ResetCommandAction);
            ShowBirthdaysCommand = new ActionCommand(ShowBirthdaysCommandAction);

            LoadContacts();
            ResetProperties();  // Initialize properties
        }

        private void SaveCommandAction()
        {
            if (SelectedContact == null)
            {
                Contacts.Add(new Contact(Lastname, Firstname, Email, PhoneNumber, DateOfBirth));
                ResetProperties();
            }
            else
            {
                SelectedContact.Lastname = Lastname;
                SelectedContact.Firstname = Firstname;
                SelectedContact.Email = Email;
                SelectedContact.PhoneNumber = PhoneNumber;
                SelectedContact.DateOfBirth = DateOfBirth;

                SelectedContact = null;
            }
            UpdateFilteredContacts();
            SaveContacts();
        }

        private void DeleteCommandAction()
        {
            Contacts.Remove(SelectedContact);
            UpdateFilteredContacts();
            SaveContacts();
        }

        private void SearchCommandAction()
        {
            UpdateFilteredContacts();
        }

        private void ResetCommandAction()
        {
            SearchQuery = string.Empty;
            UpdateFilteredContacts();
        }

        private void ShowBirthdaysCommandAction()
        {
            BirthdaysThisWeek = new ObservableCollection<Contact>(
                Contacts.Where(c =>
                {
                    var today = DateTime.Today;
                    var startOfWeek = today.AddDays(-(int)today.DayOfWeek);
                    var endOfWeek = startOfWeek.AddDays(7);

                    var thisYearBirthday = new DateTime(today.Year, c.DateOfBirth.Month, c.DateOfBirth.Day);
                    return thisYearBirthday >= startOfWeek && thisYearBirthday < endOfWeek;
                }));
        }

        private void CheckSaveButtonAvailablity()
        {
            SaveButtonEnabled = !string.IsNullOrEmpty(Lastname) && !string.IsNullOrEmpty(Firstname) && (!string.IsNullOrEmpty(Email) || !string.IsNullOrEmpty(PhoneNumber));
        }
        private void CheckDeleteButtonAvailablity()
        {
            DeleteButtonEnabled = SelectedContact != null;
        }

        private void FillForm(Contact selectedContact)
        {
            Lastname = selectedContact.Lastname;
            Firstname = selectedContact.Firstname;
            Email = selectedContact.Email;
            PhoneNumber = selectedContact.PhoneNumber;
            DateOfBirth = selectedContact.DateOfBirth;
        }

        private void ResetProperties()
        {
            Lastname = null;
            Firstname = null;
            Email = null;
            PhoneNumber = null;
            DateOfBirth = DateTime.Now;
        }

        private void UpdateFilteredContacts()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                FilteredContacts = new ObservableCollection<Contact>(Contacts);
            }
            else
            {
                FilteredContacts = new ObservableCollection<Contact>(
                    Contacts.Where(c =>
                        (c.Lastname != null && c.Lastname.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (c.Firstname != null && c.Firstname.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (c.Email != null && c.Email.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (c.PhoneNumber != null && c.PhoneNumber.IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (c.DateOfBirth.ToString("MM/dd/yyyy").IndexOf(SearchQuery, StringComparison.OrdinalIgnoreCase) >= 0)));
            }
        }

        private void SaveContacts()
        {
            var json = JsonConvert.SerializeObject(Contacts);
            File.WriteAllText(ContactsFilePath, json);
        }

        private void LoadContacts()
        {
            if (File.Exists(ContactsFilePath))
            {
                var json = File.ReadAllText(ContactsFilePath);
                var contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(json);
                Contacts = contacts ?? new ObservableCollection<Contact>();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ActionCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public ActionCommand(Action execute) : this(execute, null)
        {
        }

        public ActionCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
