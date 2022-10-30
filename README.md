# Introduction

this is the exercise done by Xanthus Wong for SleekFlow post application.

# Contact Information

please feel free to contact me for any question/uncertainty

- mobile: 67933067 [phone or whatsapp]
- email: xanthusmnwong@gmail.com

# Prerequisite

- docker installed
- docker-compose installed
- vs2022 installed
- .NET6 SDK intalled
- MS SQL Server Management Studio installed

# Test Procedure

## start api services

1. open SleekFlow.Web.sln with vs2022
2. in Developer PowerShell/Package Manager Console, exec => docker-compose -f "docker-compose.yml" -f "docker-compose.override.yml" -f "docker-compose.vs.debug.yml" up -d --build
3. the api services is now up

## run test projects

1. open Test Explorer window in visual studio
2. right click on the top most item of the tests [SleekFlow.Web.WebAPI.Test(5)] and click "Run"
3. all test cases should be run and pass

## try api by yourself

1. open browser and go to => https://localhost:11443/swagger/index.html
2. obtain the token of desired user from "WebAuth/Login", user can be "user" or "admin", password is "P@ssw0rd". p.s. admin can perform all actions while user can only read and search todo
3. click authorize on top right corner of swagger page and paste the token you obtained from "WebAuth/Login" and save
4. feel free to try the todo api

## db investigation

1. open ssms
2. connect to localhost,11433 with "sa" and "P@ssw0rd"
3. go to SleekFlow.Web -> Tables
4. check record behavior
p.s. each todo has basic audit and dbo.AuditEntries store all Todo history trail

# Implemented Features

- [x] Requirements
	- [x] Required
		- [x] TODOs CRUD
			- [x] Each TODO has
				- [x] Name
				- [x] Description
				- [x] Due Date
				- [x] Status
		- [x] Filtering
		- [x] Sorting
	- [x] Nice to Have
		- [x] Additional Attributes in each TODO
		- [x] Authentication
		- [x] Team Features
			- [x] Authorization
			- [ ] Real-time Collaboation
		- [ ] DevOps, _not enough time to do, could explain more if interested in_
		- [x] Architecture Diagram, _please refer to doc folder_
		- [x] Any thing you want to improve
			- [x] Audit Trail
- [x] Design Requirements
	- [x] Technical Design
		- [x] SOLID
		- [x] TDD
		- [ ] Consistency
	- [x] API Design
		- [x] Naming
		- [x] Model Mapping
	- [x] Documentation, _README.MD_
- [x] Deliverables
	- [x] A GitHub repsitory
	- [x] SwaggerDocument / Postman Collection