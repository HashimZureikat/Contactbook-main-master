using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PhoneBook
{
    internal class Contact : INotifyPropertyChanged
    {
        private string lastname;
        private string firstname;
        private string email;
        private string phoneNumber;
        private DateTime dateOfBirth;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Lastname
        {
            get { return lastname; }
            set
            {
                lastname = value;
                OnPropertyChanged();
            }
        }
        public string Firstname
        {
            get { return firstname; }
            set
            {
                firstname = value;
                OnPropertyChanged();
            }
        }
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged();
            }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged();
            }
        }
        public DateTime DateOfBirth
        {
            get { return dateOfBirth; }
            set
            {
                dateOfBirth = value;
                OnPropertyChanged();
            }
        }

        public Contact(string lastname, string firstname, string email, string phoneNumber, DateTime dateOfBirth)
        {
            Lastname = lastname;
            Firstname = firstname;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
