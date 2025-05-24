# LifeBridge-MVC
A secure ASP.NET Core MVC app for managing blood and organ donations with authentication and authorization.



# For developers
1. Fork/clone the reposity
2. Go to the project folder : `LifeBridge`
3. Go the Terminal, Install the packages and initialize databse
```bash
$ dotnet restore
$ dotnet tool install --global dotnet-ef
$ dotnet ef database update
$ dotnet run
```
4. Add Migration if you do any changes in databse:
``` bash
$ dotnet ef migrations add your_upadte_details
$ dotnet ef database update
```