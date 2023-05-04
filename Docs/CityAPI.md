## CityWeather API
- [CityWeather API](#cityweather-api)
  - [AddController](#addcontroller)
    - [Request](#request)
    - [Response](#response)
      - [Success](#success)
      - [Already Exist](#already-exist)
      - [Latitude or Longitude Out of Range](#latitude-or-longitude-out-of-range)
  - [DeleteController](#deletecontroller)
    - [Request](#request-1)
    - [Response](#response-1)
      - [Success](#success-1)
      - [Not Found](#not-found)
  - [SearchController](#searchcontroller)
    - [Request](#request-2)
    - [Response](#response-2)
      - [Success](#success-2)
      - [Not Found](#not-found-1)
  - [UpsertController](#upsertcontroller)
    - [Request](#request-3)
    - [Response](#response-3)
      - [Success](#success-3)
      - [Latitude or Longitude Out of Range](#latitude-or-longitude-out-of-range-1)

  
> Temperatures are stored in Kelvin unit 

### AddController
Add city to database by passing cityname, latitude and longitude
#### Request
```js
POST /citydata?cityname={cityname}&latitude={latitude}&longitude={longitude}
```
#### Response
##### Success
```js
Example Request:
POST BaseURLcitydata?cityname=New Orleans&latitude=29.9547&longitude=-90.0751
Status: 201 CREATED
Body:
{
    "city": {
        "id": "2c48cf41-dfc4-48b6-8332-14b2c906f2d7",
        "cityName": "New Orleans",
        "latitude": 29.9547,
        "longitude": -90.0751
    },
    "weather": {
        "temperature": 0,
        "lastModified": "2023-05-05T00:56:13.2685256+08:00"
    }
}
```
##### Already Exist
```js
Status: 409 Conflict
City already exists in database.
```
##### Latitude or Longitude Out of Range
```js
Status: 500 Internal Sever Error
Body:
Arithmetic overflow error converting nvarchar to data type numeric.
The statement has been terminated.
```
---
### DeleteController
Delete city from database by cityname
#### Request
```js
DELETE /citydata?cityname={cityname}
```
#### Response
##### Success
```js
Example Request:
DELETE BaseURL/citydata?cityname=Seoul
Status: 204 NO CONTENT
Body:

```
##### Not Found
```js
Status: 404 Not Found
Body:
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "traceId": "00-1c9edb0489952a6d5957100271c63846-25e25d317bd5bfda-00"
}
```
---
### SearchController
Search city info from database by cityname 
#### Request
```js
GET /citydata?cityname={cityname}
```
#### Response
##### Success
```js
Example Request:
GET BaseURL/citydata?cityname=New Orleans
Status: 200 OK
Body:
{
    "city": {
        "id": "2c48cf41-dfc4-48b6-8332-14b2c906f2d7",
        "cityName": "New Orleans",
        "latitude": 29.954700,
        "longitude": -90.075100
    },
    "weather": {
        "temperature": 0.00,
        "lastModified": "2023-05-05T00:56:13.27"
    }
}
```
##### Not Found
```js
Status: 404 Not Found
Body:
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "traceId": "00-5f30ac53b3d97d0f8b637385431b7b0b-69279ccad277db22-00"
}
```
---
### UpsertController
Update city info if the city is already in database
#### Request
```js
PUT /citydata?cityname={cityname}&latitude={latitude}&longitude={longitude}&temp={temperature}
```
#### Response
##### Success
```js
Example Request:
PUT BaseURL/citydata?cityname=Seoul&latitude=37.532600&longitude=127.024612&temp=291.15

Status: 201 CREATED
Body:
{
    "city": {
        "id": "2c48cf41-dfc4-48b6-8332-14b2c906f2d7",
        "cityName": "New Orleans",
        "latitude": 29.9547,
        "longitude": -90.0751
    },
    "weather": {
        "temperature": 300.36,
        "lastModified": "2023-05-05T00:59:54.5364055+08:00"
    }
}
```

##### Latitude or Longitude Out of Range
```js
Status: 500 Internal Server Error
Body:
Arithmetic overflow error converting nvarchar to data type numeric.
The statement has been terminated.
```
---