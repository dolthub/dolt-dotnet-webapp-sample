# dolt-dotnet-webapp-sample 
Sample web application using .NET Core Razor Pages and using Entity Framework Core to interact with a Dolt database. See the [DoltHub blog for a walkthrough of building this app](https://www.dolthub.com/blog/2024-02-28-works-with-dolt-dotnet-webapp/).

## Project Overview
* `Data` dir – This directory holds the DbContext, DoltContext, which serves as the central point for working with the Dolt database.
* `Migrations` dir – This directory holds the EF Core migration scripts, which can be used to bootstrap the application's schema in a new database.
* `Models` dir – This directory holds the model objects that our web app works with (e.g. Movie, Commit, MovieDiff).
* `Pages` dir – This directory holds the .NET Core Razor pages that form the view of our application.
* `wwwroot` dir – This directory contains the web resources for this application (e.g. CSS files, Javascript scripts).
* `Program.cs` file – This file is responsible for starting up the web application framework and starting the sample app.

## Running This Sample 

### Install .NET tools
Make sure you have [Microsoft's .NET Core SDK installed](https://dotnet.microsoft.com/download). After installing the .NET Core SDK, open a new terminal window, and you should have `~/.dotnet/tools` on your `PATH`; if not, you can add it manually. Once the `dontet` CLI is on your path, install the Entity Framework Core tools by running:
- `dotnet tool install --global dotnet-ef`

### Install Dolt
You can find [Dolt's installation instructions online](https://github.com/dolthub/dolt#installation). I use `brew install dolt` on my Mac, but the Dolt install instructions 
provide options for using a Windows MSI, Chocolatey, or even building from source. 

### Start up a Dolt `sql-server`
- create a directory named "dolt" and run `dolt init` in that directory to create the database named "dolt" that the application looks for.
- start up the sql-server by running `dolt sql-server -uroot --port 11229`.
- apply the migrations from the sample project by running `dotnet ef database update` from the sample project's root directory.
- create an initial Dolt commit from the applied database changes by running `mysql -uroot --protocol TCP --port 11229 -e "call dolt_commit('-Am', 'Applying initial schema');"`

### Run the .NET project
Run `dotnet restore` to ensure all the project's dependencies have been restored, then run `dotnet run` to execute the code in `Program.cs` and start up the web application.
The output of `dotnet run` will tell you what port the application is running on. 


## Problems? Questions?
If you've found an issue with this sample project, feel free to [create an issue in this project's GitHub repo](https://github.com/dolthub/dolt-dotnet-webapp-sample/issues/new). 

If you've found a problem with Dolt working correctly with .NET web applications, feel free to [create an issue in the Dolt GitHub repo](https://github.com/dolt
hub/dolt/issues/new).

If you just wanna ask some questions or discuss how to use Dolt, then please [swing by our Discord server](https://discord.gg/gqr7K4VNKe) and come find us! 
Our dev team spends our days on Discord and we love it when customers come by to chat about databases, versioning, or programming frameworks! 🤓
