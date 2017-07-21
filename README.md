# csod-edge-integrations-custom-provider-service
this repo is a starter pack for integrating against custom type integrations in Cornerstone Edge

To get started clone this repo and run it on your local dev machine or an instance in the cloud (aws, azure, google app engine, digital ocean, etc). This repo is built using ASPNET Core and therefore will be able to be run in any linux, windows, or mac environment. Please read up on aspnet core and get familiar with it.

This repo is a baseline for getting started. It includes a rudimentary user page, which allows you to tie a username/password combo (basic auth request) to a class of settings. This repo contains the concept of being an adapter which is essentially what this service serves to accomplish: being the middleware that talks to a vendor and integrates against a custom type integration as defined by Cornerstone Edge.

# Quick Start
Clone repo, and run. Either command line or you can use IISExpress. You will land on the Manage Users page, or navigate to it http://localhost:31515/user . This should be a good starting point and help you familiarize yourself with a User which has a username and password and the associated Settings that a user has. By default I added VendorUrl and VendorUserIdForUser as just some dummy settings to help you understand what would/should go into Settings.

# Basics
ASPNET Core MVC + WebAPI + Entity Framework Core + Sqlite

UI is built using Vuejs 2 and Semantic UI CSS (just the css)

wwwroot/ - contains all static files, css, js, this is where the frontend code lives

Controllers/
  UserController.cs - This is a WebAPI controller which contains all the necessary CRUD operations for a User

Migrations/ - This folder contains all migrations. I have added an initial migration that matches the User and Settings model. If you update your Settings or User model you would need to run migrations. Look up EFCore and dotnet. Basically you're going to run, `dotnet ef migrations add MyMigrationName`, and then do, `dotnet ef database update`. You can also circumvent the process and manually add your own migration into the migrations folder and build/execute the project and migrations should kick in because of the Database.Migrations() call executed in UserContext.cs

Models/
  Settings.cs - This is the settings model that you will use to reflect a group of settings to the vendor. This can be anything from a unique ID that the vendor wants you to use every time you make a request or a URL that the vendor has specificed for you when making requests. The UI figures out the model of the settings, but currently does not support nested hierarchy, so I reccommend keeping everything flat. 99% of use cases is flat hierachry for settings.

  User.cs - This is the user model.

  UserContext.cs - This is the Entity Framework Core DbContext for user. Both users and settings db access are located here.

Views/
   User/
    Index.cshtml - This is an entry point for the UI to launch

Views/_Layout.cshtml - This is the shared layout used by Index

appsettings.json - This is your appconfig file in JSON format
 
  appsettings.Development.json - I would use this for development purposes only. If a Config setting you're trying to access isn't in here, dotnet will try to use the non-Development one. The order is if you're in DEV, Development settings superceed the non-Development one.

Program.cs - entry point to start everything.

Startup.cs - This is where you define what your app is using. This is also where you would do dependency injection as well as global configuration settings such as JSON Formatter Styling

# Dev Guide
## Sqlite 
is currently used as the datastore for the models. For PRODUCTION the sqlite instance of Database.db should be where your application directory is. For DEVELOPMENT purposes it is located in `\bin\Debug\netcoreapp1.1\`. Note that this service currently uses netcoreapp1.1, of course if you use a different version it would be in a different folder.
## Building your service
This section is dedicated to helping you understand the flow of how an integration works. While each integration type might have different contracts, the logical flow is all the same.

### simple flow
>CSOD EDGE makes a request for some information or action => This service receives it and then makes a request out to the vendor => which sends back a response => which is then piped as a response back to CSOD EDGE

### more detailed breakdown of the flow
I am going to use Background Check as an example to help you understand the entire process and the separation of concerns from each domain
>Sally is a recruiter. She is going to use Custom Background Check to run a background check on her potential employees. The first thing Sally does is look at a list of available background check packages.

This is where the flow starts. When the user, Sally, requests a list of backgrround checks, CSOD Edge makes a call out to the service endpoint of packages which is implemented by this service against the contract as defined here: https://app.swaggerhub.com/apis/mwangcsod/BackgroundCheck/1.0.0 . **Note that this is specifically for background checks, your integration type might differ** For all requests coming from CSOD Edge you should be validating the username/password that comes in as basic auth in the header of every request. Using this username/password combo you can then retrieve the associated settings. Using the settings you can then make the appropriate request out to the vendor. In this case the request hits the packages endpoint, so you would retrieve the settings, look up the packages endpoint url for the vendor as well as the unique identifier that the vendor has specified for this user/company. You would then use these settings and make a request out to the vendor for a list of background check packages. You can then format the vendor response in a way that CSOD Edge understands (as definied by the above contracts) and send it back to CSOD Edge for Sally to view the list of background check packages.

>After viewing the list of background check packages, Sally has decided to run one of the packages on a potential candidate. She selects the package and initiates the request for a background check.

Here, Sally has selected a package and made a request to start a background check. CSOD Edge then gathers all the relevant information as defined by the above contract and makes a request out to this service to initiate a background check. Background checks utilize callbacks as part of the contract and as a result CSOD Edge has sent you a callback url as part of the request. Here you would create a class model that represents some unique Guid mapping to a callback url. You would then store this association and when the vendor hits your callback with the Guid that you've supplied you can look up the correct associated CSOD Edge callback url. Again as the service, you are parsing the header for basic auth credentials and retrieving the assoicated settings. Once again you use the correct corresponding settings and make a request out to the vendor to start a background check. In this case the vendor will either acknolwedge or throw an error which is then up to you to send back the appripriate response that CSOD Edge understands.

>A few days later, Sally is alerted that the background check is complete and she can view the status. 

Prior to Sally getting the notification that the background check is complete the vendor has sent out a callback to your service endpoint. This callback contains relevant information about the status of the background check as well as the reference ID (Guid) that you sent as part of the background check request to the vendor. You would then take this Guid and look up the associated callback url. This is the callback url to use when sending the response to CSOD Edge. This is where you would then use the callback response from the vendor and formulate it to one that CSOD Edge understands. Then you would make the apprioate call to to that aforementioned CSOD Edge callback url with the background check payload. CSOD Edge receives this payload and updates the status of the background check. Note that for some integrations such as Background Check there could be multiple callbacks. This completes the entire flow of the integration.
