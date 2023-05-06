# City Weather
Change databases connectionstring in `appsettings.json`


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
