# Bill Tracking - CSYE 6225

 A .NET core 3.0 backend web application for tracking bills for a user. A user can create, update, view and delete bills.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

What things you need to install the software and how to install them

* [.NET Core 3.0](https://dotnet.microsoft.com/) - The web framework used
* [Entity Framework](https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet) - Dependency Management
* [PostgreSQL](http://postgresguide.com/setup/install.html) - Relational Database

### Installing

A step by step series of commands that tell you how to get a development env installed.

1. Download [.NET Core SDK](https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1804) by selecting your choice of the OS kernel. For this document, we are using the **Ubuntu -18.04** image. Follow the instructions to download the [.NET Core SDK](https://docs.microsoft.com/en-us/dotnet/core/install/linux-package-manager-ubuntu-1804).

2. Check everything installed correctly by running the **dotnet** command in the terminal. You should see the help menu after the following command runs.
```
~$ dotnet
```
3. Restore the **Dotnet Entity Framework** by running the below command in the terminal. This commands restores the framework locally. Run the command at the location of .config folder where the manifest file is.
```
~$ dotnet tool restore
```
4. Install **PostgreSQL** by running the following command in the terminal. Set up a user and a database for the application configuration. We will modify the configuration file in the step below.
```
~$ sudo apt-get install postgresql
```
This completes the prerequisite software and tool installation.

###

### Building

A step by step series of commands that tell you how to get a development environement running.

1. Navigate to the project directory where the code base has been downloaded or cloned. 

2. Modify the database environment by editing the file **appsettings.Development.json** with the PostgreSQL database name and user which you created during database installation.

3. Run the build command to resotre the application packages and build the application.
```
~$/webapp/csye6225 dotnet build
```
4. Update the database with the models by running the following command in the terminal.
```
~$/webapp/csye6225 dotnet ef database update
```

## Running the tests

1. Navigate to the **UnitTests** project directory.
2. Run the automated tests for this system by running the following command in the terminal
```
~$/webapp/UnitTests dotnet test
```
3. Check the database, two new users should be created in the **Account** table.

## Authors

* **Sajal Sood** - *Initial work* - [Sajal Sood](https://github.com/SajalSood)

## License
[MIT](https://choosealicense.com/licenses/mit/)

