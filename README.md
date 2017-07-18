# csod-edge-integrations-custom-provider-service
this repo is a starter pack for integrating against custom type integrations in Cornerstone Edge

To get started clone this repo and run it on your local dev machine or an instance in the cloud (aws, azure, google app engine, digital ocean, etc). This repo is built using ASPNET Core and therefore will be able to be run in any linux, windows, or mac environment. Please read up on aspnet core and get familiar with it.

This repo is a baseline for getting started. It includes a rudimentary user page, which allows you to tie a username/password combo (basic auth request) to a class of settings. This repo also includes a few sample endpoints which will help you understand the consumption of this service. Lastly, this repo contains the concept of being an adapter which is essentially what this service serves to accomplish: being the middleware that talks to a vendor and integrates against a custom type integration as defined by Cornerstone Edge.
