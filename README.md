
# Regions With Api

### Application running on .Net 6 and Angular 14 using MediatR and CQRS pattern

## Steps to run project

#### 1.  npm install from terminal in ClientApp folder
#### 2.  Change connection string in appsettings file to use local db or your sql instance
#### 3.  Run project from Visual Studio 2022
#### 4.  npm start from terminal in ClientApp folder
#### 5.  Open localhost:4200 in browser and open console to see results
#### 6.  After first start of API and successfull data seed, SeedData.Initialize(services) on line 49 in program.cs needs to be commented out. 

## P.S. Didn't have time to show results in UI and interact with endpoint, had to comment out UseSpa method. We can use swagger endpoint /swagger for API to test
