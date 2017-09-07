# csod-edge-integrations-custom-provider-service
This repo is a starter pack for integrating against custom type integrations in Cornerstone Edge. Custom type integrations are integrations which collectively fall under one category such as: background check, payroll, time and attendance, etc. Instead of integrating against every vendor, it is more pragamtic to define a common theme to integration types and implement at the type level. Therefore, background checks will have one well defined integration contract that various background check providers can build against; and the middleware to integrate between the well defined background check contract as defined by Cornerstone Edge and a background check contract as defined by a Vendor is what this repo serves to accomplish, thus the Custom Connector name.

For those who are savvy and know what they're doing you can simply clone this repo 'master' branch and run it on your local dev machine (currently the configuration is set to work on windows environments, but you can configure it to work in mac and/or linux environments) or an instance in the cloud (aws, azure, google app engine, digital ocean, etc). This repo is built using ASPNET Core. Please read up on aspnet core and get familiar with it if you're new to C#/donetcore.

This repo is a baseline for getting started. It includes a rudimentary user page, which allows you to associate a username/password combo (basic auth request) to a class of settings (which is unique per client against a vendor/provider). This repo serves to help speed up your development process and get a jumpstart by doing most of the leg work and setting up the boiler plate code. It also serves to help highlight the concept of Cornerstone Edge Custom Connectors, which is essentially the service that you're building; the middleware that interfaces with a vendor and integrates against a custom type integration as defined by Cornerstone Edge.

# Quick Start
Select the repo that most fits your needs. As of 9/5/2017 we have the following branches that we recommend:

**master** - the master branch and is super lightweight, no custom type contracts are loaded in here, just a users page

**assessment-base** - this is the base branch for custom type of assessments, it includes all of master branch's stuff plus the assessment custom type contracts. Use this repo if you're building against a Vendor of type assessments as this will get you up and running quicker

**assessment-aon** - this is the aon branch for custom type assessment. This branch helps you understand what a custom connector would look like and how it would work; in this case we have a working example of aon against custom type assessments

**bgcheck-base** - this is the base branch for custom type of background check, it includes all of master branch's stuff plus the custom background check contracts. Use this repo if you're building against a Vendor of type background check as this will get you up and running quicker

**bgcheck-fdav** - this is the fadv branch for custom type background check. This branch helps you understrand what a custom connector would look like and how it would work; in this case we have a working example of fadv against custom type background check

Clone repo, and run. Either command line or you can use IISExpress. You will land on the Manage Users page, or navigate to it http://localhost:31515/user . This should be a good starting point and help you familiarize yourself with a User which has a username and password and the associated Settings that a user has. By default I added VendorUrl and VendorUserIdForUser as just some dummy settings to help you understand what would/should go into Settings.

# Basics covering 'master' branch
ASPNET Core MVC + WebAPI + LiteDB (this is an embedded nosql datastore, much like sqlite for sql)

UI is built using Vuejs 2 and Semantic UI CSS (just the css)

**wwwroot/** - contains all static files, css, js, this is where the frontend code lives

**Controllers/**
  
  **CallbackController.cs** - This is a WebAPI controller which contains an endpoint for callbacks from a vendor to this service. Callbacks seem to be universal for most vendors so I included it for reference. There is also a private method I created to help you generate a callback record, which is where the callback starts; by you generating a unique ID and then associating it with a record and handing the unique ID off to the vendor to reference on callbacks; this private method to generate callbacks can live anywhere
  
  **SettingsController.cs** - This is a WebAPI controller which contains operations for settings. You can choose to use this or not. During development I realized that because settings are so closely associated to a user, it made more sense to lump settings during a get user call from the UI
  
  **UserController.cs** - This is a WebAPI controller which contains all the necessary operations for validating user credentials and adding/updating a user

**Data/**
  
  **CallbackRepository.cs** - This is the repository pattern that sits on top of litedb that specifically operates on callbacks with CRUD operations. You can choose to add more functions that operate on the callback class as you see fit. I currently use dependency injection to spin up an instance of this callback repository, check out Startup.cs and any controller class to see usage.
  
  **SettingsRepository.cs** - This is the repository pattern that sits on top of litedb that specifically operates on settings with CRUD operations. You can choose to add more functions that operate on the settings class as you see fit. I currently use dependency injection to spin up an instance of this settings repository, check out Startup.cs and any controller class to see usage.
  
  **UserRepository.cs** - This is the repository pattern that sits on top of litedb that specifically operates on users with CRUD operations. You can choose to add more functions that operate on the users class as you see fit. I currently use dependency injection to spin up an instance of this user repository, check out Startup.cs and any controller class to see usage.

**Middleware/**
  
  **BasicAuthenticationHandler.cs** - This is the basic authentication override where we can take over how authentication is handled during basic auth. You can write your own custom logic, however we have already wired this up to work correctly with users, so this is more of a reference than any code modification. Usage is attribute decoration on any controllers like so: **[Authorize(ActiveAuthenticationSchemes = "Basic")]**. Refer to any controllers to get a good idea.
  
  **BasicAuthenticationIdentity.cs** - This is the class we're using to support our custom version of basic auth identity. As you can see it implements IIdentity. You can choose to expand this or leave it as is. A word of warning, DotNetCore uses ClaimsIdentity, please familiarize yourself with this before attempting any overrides or custom user property storage.
  
  **BasicAuthenticationMiddleware** - This is the middleware that intercepts basic auth request and routes it to the handler, BasicAuthenticationHandler
  
  **BasicAuthenticationOptions** - This just tells the server what authentication scheme we're looking for. In this case it is "basic"
  
  **MiddlewareExtensions** - The extension that fires up and tells IApplicationBuilder to use our BasicAuthenticationMiddleware for basic auth request


**Models/**

  **Callback.cs** - This is the callback model that is defined to help store callbacks. You can add more properties to this data contract and fill it with your needs. The basic properties I have added are: 
  `id - which is an autoincremented id used by litedb to keep track of records`, 
  
  `PublicId - which is a GUID used to provide to vendors to help provide unique keys for callbaks`, 
  
  `EdgeCallbackUrl - the url that Edge provided to make callbacks into edge, it make sense to include this as most of the time you have the PublicID and need the EdgeCallbackUrl to update some information`
  
  `Limit - this is a counter which helps you keep track of callbacks, callbacks exceeding this limit are ignored, of course you can modify this logic in the callback controller should you want/need to`
  
  **Settings.cs** - This is the settings model that you will use to reflect a group of settings to the vendor. This can be anything from a unique ID that the vendor wants you to use every time you make a request or a URL that the vendor has specificed for you when making requests. The UI figures out the model of the settings, but currently does not support nested hierarchy, so I reccommend keeping everything flat. 99% of use cases is flat hierachry for settings. If you wish to add more comlicated hiearchy you will have to maintain the UI and update ManageUser.js to reflect your custom settings

  **UpdateOrAddUserSettingsRequest.cs** - This is the data contract that helps the webapi endpoint strongly type a update user settings request from the UI.
  
  **UpdateUserPasswordRequest.cs** - This is the data contract that helps the webapi endpoint strongly type a user password change request from the UI.

  **User.cs** - This is the user model. Pretty self explanatory, you can choose to add more properties for your concept of 'user' here

  **UserLoginRequest.cs** - This is a data contract that helps the webapi endpoint strongly type login request from the UI

**Views/**
   **User/**
    **Index.cshtml** - This is an entry point for the UI to launch

**Views/_Layout.cshtml** - This is the shared layout used by Index

**appsettings.json** - This is your appconfig file in JSON format
 
  **appsettings.Development.json** - I would use this for development purposes only. If a Config setting you're trying to access isn't in here, dotnet will try to use the non-Development one. The order is if you're in DEV, Development settings superceed the non-Development one.

**Program.cs** - entry point to start everything.

**Startup.cs** - This is where you define what your app is using such as MVC, webapi,etc. This is also where you would do dependency injection as well as global configuration settings such as JSON Formatter Styling

# Dev Guide
## litedb
is currently used as the datastore for the models. We chose litedb because it was lightweight and an embedded nosql solution. For our example boilerplate code we wanted to provide a quick, get started and code solution;instead of building against Mongo, Couch, and the litany of nosql dbs, we decided for the express route of embedded. You can always move your database to mongo because the way we build the project, you just have to replace the adapter layer that talks to the DB. The location of this datastore can be defined in your appsettings.json. For deploying to AWS via Elastic BeanStalk we've noticed that you can only define datastores relative to the C drive or absolute to the C drive, so instead of relative to the application as definied in appsettings.json, the correct way for AWS would be C:\yourdatastorefile.db or anything along those lines would be fine

## Building your service
This section is dedicated to helping you understand the flow of how an integration works. While each integration type might have different contracts, the logical flow is all the same.

### simple flow
>CSOD EDGE makes a request to this service for some information or action => This service receives it and then makes a request out to the vendor => which sends back a response => which is then operated on and piped as a response back to CSOD EDGE

### more detailed breakdown of the flow
I am going to use Background Check as an example to help you understand the entire process and the separation of concerns from each domain
>Sally is a recruiter. She is going to use Custom Background Check to run a background check on her potential employees. The first thing Sally does is look at a list of available background check packages.

This is where the flow starts. When the user, Sally, requests a list of backgrround checks, CSOD Edge makes a call out to the service endpoint of packages which is implemented by this service against the contract as defined here: https://app.swaggerhub.com/apis/mwangcsod/BackgroundCheck/1.0.0 . **Note that this is specifically for background checks, your integration type might differ** For all requests coming from CSOD Edge you should be validating the username/password that comes in as basic auth in the header of every request. Using this username/password combo you can then retrieve the associated settings. Using the settings you can then make the appropriate request out to the vendor. In this case the request hits the packages endpoint, so you would retrieve the settings, look up the packages endpoint url for the vendor as well as the unique identifier that the vendor has specified for this user/company. You would then use these settings and make a request out to the vendor for a list of background check packages. You can then format the vendor response in a way that CSOD Edge understands (as definied by the above contracts) and send it back to CSOD Edge for Sally to view the list of background check packages.

>After viewing the list of background check packages, Sally has decided to run one of the packages on a potential candidate. She selects the package and initiates the request for a background check.

Here, Sally has selected a package and made a request to start a background check. CSOD Edge gathers all the relevant information as defined by the above contract and makes a request out to this service to initiate a background check. Background checks utilize callbacks as part of the contract and as a result CSOD Edge has sent a callback url as part of the request. Here you would create a class model that represents some unique Guid mapping to a callback url. You would then store this association and when the vendor hits your callback with the Guid that you've supplied you can look up the correct associated CSOD Edge callback url. Again as the service, you are parsing the header for basic auth credentials and retrieving the assoicated settings. Once again you use the correct corresponding settings and make a request out to the vendor to start a background check; using and formating the information passed by CSOD Edge to one that the Vendor understands. In this case the vendor will either acknolwedge or throw an error which is then up to you to send back the appripriate response that CSOD Edge understands.

>A few days later, Sally is alerted that the background check is complete and she can view the status. 

Prior to Sally getting the notification that the background check is complete the vendor has sent out a callback to your service endpoint. This callback contains relevant information about the status of the background check as well as the reference ID (Guid) that you sent as part of the background check request to the vendor. You would then take this Guid and look up the associated callback url. This is the callback url to use when sending the response to CSOD Edge. * *Note for some CSOD Edge integrations there needs to be an api secret key that should be sent, this is a case by case basis.* This is where you would then use the callback response from the vendor and formulate it to one that CSOD Edge understands. Then you would make the apprioate call to to that aforementioned CSOD Edge callback url with the background check payload. CSOD Edge receives this payload and updates the status of the background check. Note that for some integrations such as Background Check there could be multiple callbacks. This completes the entire flow of the integration.
