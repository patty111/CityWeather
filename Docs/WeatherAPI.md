# Weather Search API
  - [Weather Search](#weathersearchcontroller)

## WeatherSearchController
Pass the latitude and longitude to retrieve current weather data of the corresponding location
### Request
```js
GET /weathersearch?lat={latitude}&lon={longitude}
```
### Response
#### Success
```js
Example Request:
GET BaseURL/weathersearch?lat=34.052235&lon=-118.243683
Status: 200 OK
Body:
{
    "country": "US",
    "cityname": "Los Angeles",
    "latitude": "34.0522",
    "longitude": "-118.244",
    "temperature": "285.51",
    "description": "moderate rain"
}
```

---