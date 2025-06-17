# NEXTUNI
## What to do when adding a new module

1. Create all the projects and reference all the appropriate projects:
- Domain project references Common.Domain projects
- Application project references Common.Application and Domain project
- Api projects reference Common.Api and Application project
- Infrastructure project references Common.Infrastructure and Api project
- Add AssemblyReference class in both Api and Application projects
2. In Infrastructure project
- Create the {moduleName}Module class
- Create the Database directory that will hold the {moduleName}DbContext
- Create the respective Inbox and Outbox directories that can be copied from other modules, then change to the correct DbContext in both those directories
- Change the module name in ProcessInboxJob and ProcessOutboxJob too, there are 5 places that needed change
- Rename the Add{moduleName}Module in {ModuleName}Module class
- Change the section name in services.Configure<OutboxOptions>(configuration.GetSection(...)) and the InboxOptions counterpart
- Rename the schema name in Database/Schemas.cs
3. In NextUni.Api project
- Reference the new Infrastructure project
- Create the module's specific settings file: modules.{moduleName}.json and modules.{moduleName}.Development.json and modules.{moduleName}.Production.json
- Remember to change the module's name in each module json files
- Add the module configuration registration in Program.cs in the AddModuleConfiguration call
- Register the module using the Add{moduleName}Module method in Program.cs
- Add mediatR to the new Application project by adding the assembly to the moduleApplicationAssemblies array
- If the new module has any Consumers, then add its ConfigureConsumers method in the AddInfrastructure call
- Apply the new module migration in Extensions/MigrationExtensions.cs
- Run at least one migration for that module if there are any (inbox, outbox)

Finally, copy new project files to container in Dockerfile
