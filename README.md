# ExcelWebAPI

Backend API for Excel.

## Description

The API has 2 endpoints for receiving data and 1 endpoint for writing.

Supports basic data types: string, integer, float.
Support basic math operations like +, -, /, * and ().
The program checks the correct placement and number of parentheses. 
The string type is immutable, no operations can be performed on it. To set a value of type float, you need to write numbers with a comma separator. Otherwise, the number will be interpreted as a string.

Names cellId and sheetId are case insensitive.
cellId and sheetId cannot include prohibited characters, such as !,?, #, $, etc.
Extra spaces before or after the id are ignored.

Formulas must begin with '='.

If the user specifies an invalid cellId or sheetId, they receive a response with a status code of 400 Bad Request.
If there is an error in the formula, the user receives a response with the status code 422 Unprocessable Entity.
The user sees the 404 Not Found status when the get request did not find data by cellId or sheetId.

Data is stored in the SQLite database.

## Executing program

### Docker

You can run the application in the docker
* From your project directory, start up your application by running 'docker compose up'
* Enter http://localhost:8080/ in a browser to see the application running.
You can also enter http://localhost:8080/swagger/index.html in your browser to see the graphical interface.

## Test Data

The test data has already been uploaded to the database and can be accessed by requesting http://localhost:8080/api/v1/devchallenge-xx.

## License

This project is licensed under the MIT License - see the LICENSE.md file for details
