## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# _AAN Hub API_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2Fdas-aan-hub-api?repoName=SkillsFundingAgency%2Fdas-aan-hub-api&branchName=main)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2974&repoName=SkillsFundingAgency%2Fdas-aan-hub-api&branchName=main)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-aan-hub-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-aan-hub-api)
[![Confluence Page](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3848175632/AAN+Hub+Solution+Architecture)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## About
This API solution is part of Apprentice Ambassador Network (AAN) project. This API encapsulates the backend SQL store for AAN. 

## Developer Setup

### Pre-Requisites
You will need following on your local:
* A clone of this repository
* Visual studio or similar IDE 
* .Net 10.0 SDK
* Azurite or similar local storage emulator
* SQL Database
  
### Configuration

- Create a `Configuration `table in your (Development) local storage account.
- Obtain the [SFA.DAS.AanHub.Api.json](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-aan-hub-api/SFA.DAS.AanHub.Api.json) from the das-employer-config and adjust the SqlConnectionString property to match your local setup.
- Add a row to the Configuration table with fields: 
  - PartitionKey: LOCAL
  - RowKey: SFA.DAS.AanHub.Api_1.0
  - Data: {The contents of the `SFA.DAS.AanHub.Api.json` file}

In the `SFA.DAS.AanHub.Api` project, if not exist already, add `appSettings.Development.json` file with following content:
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

### Database publish
- Publish the database project `SFA.DAS.AanHub.Database` to your local SQL Server instance.

## Technologies
* .Net 10.0
* SQL Server
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions
