## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# _AAN Hub API_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2Fdas-aan-hub-api?repoName=SkillsFundingAgency%2Fdas-aan-hub-api&branchName=refs%2Fpull%2F169%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2974&repoName=SkillsFundingAgency%2Fdas-aan-hub-api&branchName=refs%2Fpull%2F169%2Fmerge)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-aan-hub-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-aan-hub-api)
[![Confluence Page](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3848175632/AAN+Hub+Solution+Architecture)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This API solution is part of Apprentice Ambassador Network (AAN) project. This API encapsulates the backend SQL store for AAN. 

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* A storage emulator like Azurite
* The AAN Hub database - Published from the'SFA.DAS.AANHub.Database' project
  
### Config

You can find the latest config file in [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-aan-hub-api/SFA.DAS.AanHub.Api.json) repository.

In the API project, if not exist already, add appSettings.Development.json file with following content:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true",
  "ConfigNames": "SFA.DAS.AanHub.Api",
  "EnvironmentName": "LOCAL",
  "Version": "1.0"
} 
```
### Running

* Start storage emulator e.g. using a command `C:\Program Files (x86)\Microsoft SDKs\Azure\Storage Emulator>AzureStorageEmulator.exe start`
* Run the solution under Kestrel
* Open a browser window and navigate to https://localhost:7299 to see the Swagger API page where you can test endpoints. 

## Technologies
* .Net 8.0
* SQL Server
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions
