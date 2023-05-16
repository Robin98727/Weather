# Weather
Get Weather API

## 程式說明

### 使用API

使用[中央氣象局開放資料平臺之資料擷取API](https://opendata.cwb.gov.tw/dist/opendata-swagger.html)中的以下這支API

```
GET
/v1/rest/datastore/F-C0032-001
一般天氣預報-今明 36 小時天氣預報
```

### 預期結果

執行後前端畫面會顯示此API的json資料，並且會將資料寫進資料庫中的`Location` table

`Location 存放中央氣象局資料`

| 欄位 |	資料屬性  | 長度 | 可為空  | 說明 | 
| --- | ---  | --- | ---  | --- |
| LID | int  |  | NO  | 縣市流水號 |
| locationName | nvarchar  | 100 | NO | 縣市名稱 |
| weatherElement | nvarchar  | max | NO | 該縣市36小時內氣象資料 |

當重複執行API時，會去檢查該資料庫是否已經有資料，有的話會更新資料

