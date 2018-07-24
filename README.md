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



