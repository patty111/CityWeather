## CityWeather API
  - [Add City](#addcontroller)
  - [Delete](#deletecontroller)
  - [Search](#searchcontroller)
  - [Edit](#upsertcontroller)   
  
> Temperatures are stored in Kelvin unit 

### AddController
#### Request
```js
POST /citydata?cityname={cityname}&latitude={latitude}&longitude={longitude}
```
#### Response
##### Success
```json
200
{
    "id": "f878054a-d8c5-46bf-a970-5121e387f409",
    "cityname": "Seoul",
    "latitude": "37.532600",
    "longitude": "127.024612",
    "temperature": "290.15",
    "last_modify": "2023/5/3 下午 11:47:49"
}
```
##### Already Exist
```json
409
City already exists in database.
```
##### Latitude or Longitude Out of Range
```json
500
Arithmetic overflow error converting nvarchar to data type numeric.
The statement has been terminated.
```
---
### DeleteController
#### Request
```js
DELETE /citydata?cityname={cityname}
```
#### Response
##### Success
```json
200
{
    "msg": "City successfully deleted"
}
```
##### Not Found
```json
404
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "traceId": "00-1c9edb0489952a6d5957100271c63846-25e25d317bd5bfda-00"
}
```
---
### SearchController
#### Request
```js
GET /citydata?cityname={cityname}
```
#### Response
##### Success
```json
200
{
    "id": "f878054a-d8c5-46bf-a970-5121e387f409",
    "cityname": "Seoul",
    "latitude": "37.532600",
    "longitude": "127.024612",
    "temperature": "290.15",
    "last_modify": "2023/5/3 下午 11:47:49"
}
```
##### Not Found
```json
404
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
    "title": "Not Found",
    "status": 404,
    "traceId": "00-5f30ac53b3d97d0f8b637385431b7b0b-69279ccad277db22-00"
}
```
---
### UpsertController
#### Request
```js
PUT /citydata?cityname={cityname}&latitude={latitude}&longitude={longitude}&temp={temperature}
```
#### Response
##### Success
```json
200
{
    "id": "f878054a-d8c5-46bf-a970-5121e387f409",
    "cityname": "Seoul",
    "latitude": 37.532600,
    "longitude": 127.024612,
    "temperature": 291.15,
    "last_modify": "2023/5/4 上午 12:43:59"
}
```

##### Latitude or Longitude Out of Range
```json
500
Arithmetic overflow error converting nvarchar to data type numeric.
The statement has been terminated.
```
---