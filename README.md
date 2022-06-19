## N01522297-PassionProject-ExpiryDateTracker
Web application and API for storing and keeping track of expiry dates.

### Target Version
* Microsoft Visual Studio 2022
---

### Installation
```
git clone https://github.com/Cicada1105/N01522297-PassionProject-ExpiryDateTracker.git
```
---

### Setup
1. Open project in VisualStudio 2022
2. Create `App_Data` folder under N01522297-PassionProject-ExpiryDateTracker sub-folder
3. Open Tools > NuGet Packet Manager > Package Manager Console
4. Run in terminal: `Update Database`
---

### Building
- Open project in Visual Studio Code
- Debug >  Start Debugging
*The required bits and bobs of the project (e.g. CMS, Tailwind, etc.)*
---

### References
N/A

---

### Dependencies
* LINQ
* Entity Framework

 ---

### API Endpoints/Examples

**Note:** 
protocol - http/https
port_num - number localhost opens when ran through Visual Studio
host - localhost:[port_num]
base_path - api/ItemData

Prefix each endpoint with: api/ItemData/
|           Endpoint           |                    Example                    |
|------------------------------|-----------------------------------------------|
|          ListItems           |     `curl [protocol]://[host]/[Endpoint]`     |
|     ListItemsForPantry/2     |     `curl [protocol]://[host]/[Endpoint]`     |
|          FindItem/3          |     `curl [protocol]://[host]/[Endpoint]`     |
|           AddItem            | `curl -X POST -H "Content-Type:application/json" -d "{'ItemName':'Oatmeal', 'ItemExpiry':'2022-7-7', 'PantryID':8 }" [protocol]://[host]/[Endpoint]` |
|       UpdateItem/[id]        | `curl -X POST -H "Content-Type:application/json" -d "{'ItemID':[id], 'ItemName':'Pancakes', 'ItemExpiry':'2022-7-4', 'PantryID':8}" [protocol]://[host]/[Endpoint]` |
|       DeleteItem/[id]        | `curl -X POST [protocol]://[host]/[Endpoint]` |

#### Additional Endpoints

*PantryData API Endpoints*

Prefix each endpoint with: api/PantryData/
|         Endpoint         | Method |    Include Header    | Form Data Names |
|--------------------------|--------|----------------------|-----------------|
|       ListPantries       |  GET   | :white_large_square: |       N/A       |
| ListPantriesForUser/[id] |  GET   | :white_large_square: |       N/A       |
|     FindPantry/[id]      |  GET   | :white_large_square: |       N/A       |
|        AddPantry         |  POST  |  :white_check_mark:  | <ul><li>PantryName</li><li>UserID</li></ul> |
|    UpdatePantry/[id]     |  POST  |  :white_check_mark:  | <ul><li>PantryID</li><li>PantryName</li><li>UserID</li></ul> |
|    DeletePantry/[id]     |  POST  | :white_large_square: |       N/A       |
<br />
 
*UserData API Endpionts*

Prefix each endpoint with: api/UserData
|      Endpoint   | Method |    Include Header    | Form Data Names |
|-----------------|--------|----------------------|-----------------|
|    ListUsers    |  GET   | :white_large_square: |       N/A       |
|  FindUser/[id]  |  GET   | :white_large_square: |       N/A       |
|     AddUser     |  POST  |  :white_check_mark:  | <ul><li>UserFName</li><li>UserLName</li></ul> |
| UpdateUser/[id] |  POST  |  :white_check_mark:  | <ul><li>UserID</li><li>UserFName</li><li>UserLName</li></ul> |
| DeleteUser/[id] |  POST  | :white_large_square: |       N/A       |
---

### Project Notes
jsondata folder contains example JSON data for testing API endpoints

___
