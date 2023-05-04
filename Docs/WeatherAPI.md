# Weather Search API
- [Weather Search API](#weather-search-api)
  - [WeatherSearchController](#weathersearchcontroller)
    - [Request](#request)
    - [Response](#response)
      - [Success](#success)
  - [ShowWeatherController](#showweathercontroller)
    - [Request](#request-1)

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
## ShowWeatherController
Show all weather search history if `true` is passed to "show"
### Request
```js
GET /weathersearch?[show]
Example Request:
GET BaseURL/weathersearch?show=[true]
Status: 200 OK
Body:
[
    {
        "search_time": "2023-05-05T05:09:32.33",
        "latitude": 34.052200,
        "longitude": -118.243700,
        "temperature": 289.79,
        "descript": "few clouds"
    },
    {
        "search_time": "2023-05-05T05:10:16.633",
        "latitude": 34.052200,
        "longitude": -118.243700,
        "temperature": 289.79,
        "descript": "few clouds"
    },
    {
        "search_time": "2023-05-05T05:10:27.5",
        "latitude": 11.010401,
        "longitude": -118.243700,
        "temperature": 299.71,
        "descript": "overcast clouds"
    }
]
```