# City Weather API

- [City Weather API](#city-weather-api)
  - [Create City](#create-city)
    - [Create City Request](#create-city-request)
    - [Create City Response](#create-city-response)
  - [Get City](#get-city)
    - [Get City Request](#get-city-request)
    - [Get City Response](#get-city-response)
  - [Update City](#update-city)
    - [Update City Request](#update-city-request)
    - [Update City Response](#update-city-response)
  - [Delete City](#delete-city)
    - [Delete City Request](#delete-city-request)
    - [Delete City Response](#delete-city-response)

## Create City

### Create City Request

```js
POST /City
```

```json
{
    "name": "Taipei",
    "Latitude" : 25.105497,
    "Longitude" : 121.597366,
}
```

### Create City Response

```js
201 Created
```

```yml
Location: {{host}}/City/{{id}}
```

```json
{
    "id" : "0000",
    "name" : "Taipei",
    "Latitude" : 25.105497,
    "Longitude" : 121.597366,
}
```

## Get City

### Get City Request

```js
GET /City/{{id}}
```

### Get City Response

```js
200 Ok
```

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "name": "Vegan Sunshine",
    "description": "Vegan everything! Join us for a healthy City..",
    "startDateTime": "2022-04-08T08:00:00",
    "endDateTime": "2022-04-08T11:00:00",
    "lastModifiedDateTime": "2022-04-06T12:00:00",
    "savory": [
        "Oatmeal",
        "Avocado Toast",
        "Omelette",
        "Salad"
    ],
    "Sweet": [
        "Cookie"
    ]
}
```

## Update City

### Update City Request

```js
PUT /City/{{id}}
```

```json
{
    "name": "Vegan Sunshine",
    "description": "Vegan everything! Join us for a healthy City..",
    "startDateTime": "2022-04-08T08:00:00",
    "endDateTime": "2022-04-08T11:00:00",
    "savory": [
        "Oatmeal",
        "Avocado Toast",
        "Omelette",
        "Salad"
    ],
    "Sweet": [
        "Cookie"
    ]
}
```

### Update City Response

```js
204 No Content
```

or

```js
201 Created
```

```yml
Location: {{host}}/City/{{id}}
```

## Delete City

### Delete City Request

```js
DELETE /City/{{id}}
```

### Delete City Response

```js
204 No Content
```