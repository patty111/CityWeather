# City Weather
This API set provides three endpoints:
1. A CRUD endpoint for city data, allowing users to perform create, read, update, and delete operations to manage city data.  
2. A weather search endpoint based on latitude and longitude, allowing users to query current weather information for a specified  location.  
3. A search history endpoint, allowing users to view their previous search history.

Change databases connection string in `appsettings.json`


Api description see Docs folder  
[Demo Video Link](https://youtu.be/rQYHkT3IuLI)




#### Sql tables:
``` Sql
CREATE TABLE Cities(
	id UNIQUEIDENTIFIER PRIMARY KEY,
	cityname NVARCHAR(255) NOT NULL,
	latitude DECIMAL (9,6) NOT NULL,
	longitude DECIMAL (9,6) NOT NULL,
	temperature DECIMAL (10,2) NOT NULL,
	last_modify DATETIME NOT NULL DEFAULT GETDATE()
);

CREATE TABLE weather (
  search_time DATETIME NOT NULL DEFAULT GETDATE() PRIMARY KEY ,
  latitude DECIMAL(9, 6) NOT NULL,
  longitude DECIMAL(9, 6) NOT NULL,
  temperature DECIMAL (10, 2) NOT NULL,
  descript NVARCHAR(255)
);

CREATE TABLE history(
	cityname NVARCHAR(255),
	latitude NVARCHAR(20),
	longitude NVARCHAR(20),
	temperature DECIMAL(10, 2),
	last_modify DATETIME,
	search_time DATETIME NOT NULL DEFAULT GETDATE()
);
```
