# PhoneBook Application

This is a simple WPF-based PhoneBook application that allows users to manage contacts. It supports adding, editing, deleting, and searching contacts, as well as displaying contacts whose birthdays fall within the current week.

## Features

- **Add Contacts:** Add new contacts with details like First Name, Last Name, Phone Number, Email, and Date of Birth.
- **Edit Contacts:** Update existing contact information.
- **Delete Contacts:** Remove contacts from the phonebook.
- **Search Contacts:** Search for contacts by any field (First Name, Last Name, Phone Number, Email, Date of Birth).
- **Birthdays This Week:** Display contacts whose birthdays fall within the current week.

## Prerequisites

- .NET Framework (version 4.6.1 or higher)
- Newtonsoft.Json (JSON.NET) library

## Installation

1. **Clone the repository:**

    ```sh
    git clone https://github.com/HashimZureikat/Contactbook-master.git
    cd Contactbook-master
    ```

2. **Open the project in Visual Studio:**

    Open `PhoneBook.sln` in Visual Studio.

3. **Restore NuGet packages:**

    Restore the required NuGet packages (e.g., Newtonsoft.Json).

4. **Build and Run:**

    Build the project and run the application.

## Usage

- **Add a Contact:** Fill in the contact details and click the "Save" button.
- **Edit a Contact:** Select a contact from the list, modify the details, and click the "Save" button.
- **Delete a Contact:** Select a contact from the list and click the "Delete" button.
- **Search Contacts:** Enter search criteria in the search box and click the "Search" button. Use the "Reset" button to clear the search.
- **Show Birthdays This Week:** Click the "Show Birthdays This Week" button to display contacts with birthdays in the current week.

## Screenshots

![image](https://github.com/HashimZureikat/Contactbook-main-master/assets/87613242/e0511a87-5b13-4ce1-b2b5-54fa551d1ca8)


## Project Structure

- `PhoneBook.sln`: Solution file.
- `MainWindow.xaml`: XAML file for the main window layout.
- `MainWindow.xaml.cs`: Code-behind file for the main window.
- `Contact.cs`: Model class representing a contact.
- `MainViewModel.cs`: ViewModel class handling the application logic.

## Contributing

Contributions are welcome! Please fork this repository, make your changes, and submit a pull request.


