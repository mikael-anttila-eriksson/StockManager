# Projekt StockManager
Course Databases and web-based systems
2023, October.
### App
A website for a user to manage their stocks, register, create accounts, buy/sell, see stock charts.

### ER-Diagram
A User can create one-to-many Accounts. Between Account and Stocks there is a many-to-many relationship, linked togheter by three tables: Purchases, Sold, StockAccount (stocks owned by a certain account). Each Stock uses data from StockChart (with daily quotes) to build a stock chart, using candlesticks. 

![image](https://github.com/mikael-anttila-eriksson/StockManager/assets/105818456/de283094-5572-4443-864f-2422aa4008a2)

### Stock charts
Only three stocks available. Their daily quotes are downloaded .csv-files that the app reads and saves to the database separetly

### Other features/techniques used:
* Meta tags
* Cookie authentication for sign in/out
* Using CanvasJS Stockchart
* Using SQL: Procedures & Transaction in C# to save Daily quotes data
* Custom validation

### Limitations
* No buy/sell function
* Only daily quotes, not per week, hour or minutes
* Most Account data-logic is not implemented
