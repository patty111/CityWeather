# History API
- [History API](#history-api)
  - [DeleteController](#deletecontroller)
    - [Request](#request)
    - [Response](#response)
      - [Success](#success)
  - [RetrieveController](#retrievecontroller)
    - [Request](#request-1)
    - [Response](#response-1)
      - [Success](#success-1)

## DeleteController
Delete search history.
  
Two delete methods:  
1. Delete last $n$ search history
2. Delete all search history in $m$ minutes  
   If both fields contain value, then first method will be executed.
### Request
```js
DELETE /historydata?delete_nums=[n]&minutes=[m]
```
### Response
#### Success
```js
Example Request:
DELETE BaseURL/historydata?delete_nums=1
Status: 200 OK
Body:
{
    "msg": "1 deleted"
}
```

---
## RetrieveController
Retrieve last $n$ search histories.
### Request
```js
GET /historydata?retrieve_nums={n}
```
### Response
#### Success
```js
Example Request:
GET BaseURL/historydata?retrieve_nums=2
Status: 200 OK
Body:
[
    "cityname = Taichung, latitude = 24.147736, longitude = 120.673645, temperature = 0.00, last_modify = 2023/5/4 下午 10:11:03, search_time = 2023/5/4 下午 10:11:28 ",
    "cityname = asdasd, latitude = 87.000000, longitude = -99.000000, temperature = 300.00, last_modify = 2023/5/4 上午 04:52:00, search_time = 2023/5/4 上午 05:58:45 "
]
```
