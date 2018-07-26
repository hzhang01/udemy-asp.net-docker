# udemy-asp.net-docker

## Udemy Dockerized Microserivces App ASP .NET Core 2.0

### Author: Han Zhang

### Project: Selling Sport Shoes

Editor: VSCode
    Extensions: C#, VSCode Solution Explorer, Docker Explorer, Dotnet core commands
    SQL Manager: SQLPro for MSSQL

# Notes

## 1.1 to 3.10 (Theory + Database Setup)

Security: taken care by centralized token server in form of asp.net core mvc

Definition:
Microservices 
- Before: Monolithic architecture - one database - performance issues
- Now: Each module/layer will is a separate component using different technology
- Features:
    - each small service runs in its own process
    - Inter-services communication happens over HTTPS, WebSockets or AMQP
    - Each service functions within a context and is responsible for one thing only - inline with SRP of SOLID?
    - Each service must be developed autonomously and be deployable independently 
- Reasons to choose:
    - Provides long-term agility
    - Maintainability in complex, large, and highly-scalable systems
    - Each has granular and autonomous lifecycles
    - Scalability - can only scale one function based on demand, less cost
    - Inter-microservice communication: messaging

Each function will have its own architecture with API, functionalities and database

Docker - containerization services, running environment is the same for all machines
- Easy to use
- Speed (sandbox environment)
- Docker Hub 
- Modularity and Scalability
- Well suited for .NET programmers

Docker Flow
- Docker Registry (programs)
- Docker Client can communicate to Deamon in the Docker Host using REST
- Dockerfile - configuration with ports, urls and commands
- Container - things needed to run the image - OS sand more

Main Elements of MicroSs
- Web API container, Swagger UI, Controllers, Domain classes, entity framework core

Microsoft packages exist as reference, linked up on run time

## 3.11 DbContext

Access to model collection - exposes functionalities 

DbContext Properties 
- Database - access to database and execution of SQL statements
- Model - metadata about the shape of entities, relationships and how they map to the database 
- ChageTracker - access to info and operations for entity instance (DbContext included class) this context is tracking - DbSet<T>
- Methods 
    - AddAdd<TEntity>, AddRange, Find<T>, RemoveRemove<T>, RemoveChange, UpdateUpdate<T>, SaveChangesSaveChangesAsync, Entry(T entity)[Access to change], OnConfiguring [create or modify this context], onModelCreating [During initialization, before finalization, Fluent API placed here]

Tracked in Context, Database setup

## 3.12 Dependency Injection and Docker

Dependency Injection with ASP.NET Core
- A mechanism to support loose coupling between objects 
    - Instead of specifying in class creation, we implement interfaces can be passed to classes and methods 
    - Service container can be injected into other containers
    - Inside Startup Class
Lifetime options
- transient - created each time they are needed
- Scoped - once for each request, recommended for EntFram DbContext objects
- Singleton - once on first request and reused for the lifetime
- Instance - similar to singleton, created when called 

Docker
1. Download Docker Community Version -> Install 
2. Login if necessary 
3. Run: `docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=ProductApi(!)' -e 'MSSQL_PID=Express' -p 1445:1433 --name=catalogdb microsoft/mssql-server-linux:latest`
    1. Password has to be more that 8 character and at least 1 special character
    2. Lines “Starting up database “tempdb”” indicates a successful run
4. Run: `docker ps in another widow to see newly created container`

Microsoft SQL Management Studio (Windows Only - can use replacements)
- [StackOverflow](https://stackoverflow.com/questions/3452/sql-client-for-mac-os-x-that-works-with-ms-sql-server)
- Create a new connection with localhost, port:1445, username: sa, pass: ProductApi(!)

Run: docker exec -it catalogdb /opt/mssql-tools/bin/sqlcmd -s localhost -U sa
	- “-it” means interactive; location of MSSQL utility location; “-s” server name; -u user
	- Will prompt to login 
	- You can now run SQL commands
    - > CREATE DATABASE TrialDb - seeing in the list of databases
    - > SELECT Name FROM Sys.Databases
    - > GO

Create a Trial Database
```
1> USE TrialDb
2> CREATE TABLE Colleagues(Id INT, name NVARCHAR(50))
3> INSERT INTO Colleagues VALUES(1,'Ben Hayat')
4> INSERT INTO Colleagues VALUES(2,'Delight')
5> GO
```

```
1> SELECT * FROM Colleagues
2> GO
```

Check in MSSQL Management 
- Refresh or re-connect
- Open the Colleague table to see the result

## 3.13 DbContext Migration

Add a new extension to VSCode called dotnet-core-command for package managing.

Add Microsoft.EntityFrameworkCore.Tools and Microsoft.EntityFrameworkCore.Tools.DotNet to the ProductCatalogApi inside Solution Explorer window.

Now ready for migration preparation. The packages will allow us create C# classes that will generate databases and map coresponding elements and references.

Go into ProductCatalogApi folder and run:
`dotnet ef migrations add InitialMigration -o Data/Migrations -c CatalogContext`
- -o Migration folder
- -c Context class

In my terminal, a message saying the 'Price' will be truncked if the value is not within default decimal place range.

Inside [Initial Migration file](./src/Services/ProductCatalogApi/Data/Migrations/20180724194837_InitialMigration.cs) there are two methods: Up() and Down(). The up method describes the change made to the databases while the down desribes the rollbacks happend during this migration. Inside the up function three different hilo sequences are created based on our specification inside the CatalogContext.

Now we want to update the database rules to the MSSQL/Docker server.
First check whether your docker is running. Run `docker ps` to see that. If it has no values that mean you need to run through `docker start catalogdb` or right-click start on catalogdb located in VSCode Docker Container extension.

Now it's time for migration.
Go inside ProductCatalogApi directory inside your command line and run the following command:
`dotnet ef database update InitialMigration -c CatalogContext` 
instead of the one given by the lecturer. His command doesn't work on Mac currently.
- Here we are not mentioning any migration name so all dependent migrations will applyed.
- If you specify a migration name all up to and including the migration name will be applyed.

If you just run this command the command line might throw you an exception saying that "Keyword not supported: "userid"." This has to do the way you wrote your connection string inside [appsettings.json](./src/Services/ProductCatalogApi/appsettings.json). Similar problems might occur with the other keywords if they are spelled wrong or has the wrong format.

If everything run smoothly you should see a "Done" message in your command prompt. Now you can go to your SQL databse visualizer software, mine is SQL Pro for MSSQL, and see the changes. Inside you would be able to see 3 tables: Catalog, CatalogBrand, and CatalogType with all the column names and properties. My SQL database visualizer doesn't show the sequences, but yours might do it if you are using Windows.

Conclusion: 
We have applied the CatalogContext mapping onto a database using EntityFrameworkCore migration operation.

## 3.14 Data Seeding/Insert Data

Goal: need to test web API endpoints.

First, we create a [CatalogSeed.cs](./src/Services/ProductCatalogApi/Data/CatalogSeed.cs) file in Data folder. The lecturer doesn't write the file from scratch, but I will do it from sractch just to familiarize with C# syntax and add comments. Feel free to copy the file from the course package, especially when you try to seed CatalogItems.

CatalogTypeId and CatalogBrandId are specified here manually based on previous declarations.
The PictureURL will be generated by another function based its id, this will be swapped during the run-time.
Picture will be stored in the wwwroot folder.

Now we create a client callable method called SeedAsync.In this class add a range of values pre-configured in our static methods. We also do a duplicate check when this method executes. This method save asynchronously to the database.

In the [Startup.cs](./src/Services/ProductCatalogApi/Startup.cs) we want to SeedAsync method but we first need an instance of the CatalogContext. Since I am using a newer version of EntityFrameworkCore, my default product uses IWebHostBuilder Interface instead of WebHost Interface used in the course. To overcome that, I added the lecturer's code in the ConfigureSerivices method. After than just try to build your project.

Since my versioning issue, I didn't have a Controller folder and ValuesController.cs file so I copied them over from the lecture package. Furthermore, I had to add launch url into [launchSettings.json](./Properties/launchSettings.json) inside ProductCatalogApi object: `"launchUrl": "api/values"`.

If your are using EFC 2.1 and IWebHostBuilder in your Program.cs file, then you should seed your data inside the [CatalogContext.cs](./src/Services/ProductCatalogApi/Data/CatalogContext.cs).
From this point onwards, CatalogSeed.cs is absolete as all the data seeding is now embedded in the CatalogContext.cs. If you are using EFC 2.1 or earlier with IWebHost in your Program.cs file then you can use this 
https://stackoverflow.com/questions/46222692/asp-net-core-2-seed-database instead in Program.cs.

For EFC 2.1 and IWebHostBuilder, you should create a new migration `dotnet ef migrations add TestMigration -o Data/Migrations -c CatalogContext` then run `dotnet ef database update` to see the change in your database visualization app.
Like myself you might encounter issue with id missing from you seed data. I had to manually add those ids to the data but there must be a better way.

## 3.15 PicController

Create two controllers: CatalogController and PicController. Controllers will give us the option to modify through API endpoints.

First, we will create the [PicController](./src/Services/Controller/PicController.cs). Copy over lecturer's pictures into wwwroot/Pics directory and remove everything that was there during project creation. You should also delete your previous Controller files, like the ValuesController, inside the Controller folder.

Inside PicController, we create a private readonly variable using AspNetCore.Hosting called IHostingEnvironment. By initializing PicController class's _env to equal to this host environment, we can now access root web folder, json, docker files within our class.

Inside the GetImage function, we first define the web root of the project using the IHostingEnvironment functionality. Then we define the full path of an image using string concatination and Path.Combine method. In order to return an actual image, we should first read all the bytes of the image file into a buffer and return the file by specifying he image type.

## 3.16 Ammendment to 3.15

With 3.15 method we can't load the image onto the website so we had to add more code into the PicController.cs file.

If you don't have Postman, the API testing tool, please download it.
After you open a new tab in Postman, write `http://localhost:5001/api/Pic/1` if your localhost port is 5001, if not just change it to your port number. If Postman return a handshake problem, change the `http` to `https`. 

## 3.17 CatalogController

Create a file called [CatalogController](./src/Services/Controller/CatalogController.cs) inside the Controller folder and configure the route to `api/Catalog`. We need an instance of the CatalogContext in this class so we create a private read-only variable. If your `CatalogContext` keyword has an reference issues you could add `using ProductCatalogApi.Data` in your package import section.

We also need our ProductCatalogApi URL in this class so we need update the url inside appsettings.json. Inside the appsettings.json add `"ExternalCatalogBaseUrl": "http://localhost:5000"`. 
Furthermore, we sould also need to make some changes inside the launchSettings.json. Inside launch Setting change the 'applicationUrl' user 'iisSetting' to `"http://localhost:5000"`. If you have a key/value pair under 'profiles' called 'launchUrl' chang the value to `api/Catalog/items`, otherwise just add this key/pair value. Change the 'launchUrl' for 'ProductCatalogApi' object to `api/Catalog/items` as well. 
Your final settings should look something like this: [appsettings.json](./src/Services/ProductCatalogApi/appsettings.json) and [launchSettings.json](./src/Services/ProductCatalogApi/Properties/launchSettings.json).

Now we want to use those urls in our CatalogController class. The recommended way of doing this is to convert the settings inside appsettings.json into strongly-typed variables inside a class we would call CatalogSettings.cs in the root of the project. In order to make this work, we should also configure our newly created class inside Startup.cs.

Now inside our CatalogController class we can create a private ready-only variable of CatalogSettings. Any properties you have defined inside the CatalogSettings class you can now use them in this class.

We also need to turn off CatalogContext tracking to increase speed of processing.
Now can create 2 action methods for getting catalog types and brands. We should also set action type of `[HttpGet]` and route to `[Route("[action]")]`. The action is a replacement token, it will be replaced by the request method. 

## 3.18 CatalogController Part 2

We want create a GetItemById method inside the CatalogController class.
We also want to create a PaginatedItemViewModel class with page size, page index, count and data available for external get access.

## 3.19 CatalogController Part 3

We create 3 more methods all called Items but with different signatures: one only requesting for page size and page index, one specifying item name, one specifying brand and/or type id. I the last one we create a variable to convert catalog items into a queryable list and filtered accordingly.