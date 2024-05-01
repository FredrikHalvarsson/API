# ASP.NET API
This is an assignment for the course Webbappplikationer i C#, ASP.NET at Edugrade

## Examples for endpoint calls:
### Get all people
GET/people (no parameters)

### Get a specific person by ID, including all their interests and links:
GET/people/{id} (no parameter other than ID)

### Add new person:
POST/people
{
  "personId": 0,
  "firstName": "string",
  "lastName": "string",
  "phoneNumber": "123-4567890",
  "email": "user@example.com"
}

### Add new interest:
POST/interests
{
  "interestId": 0,
  "interestName": "string",
  "description": "string"
}

### Connect a person to a new interest:
POST/personwithinterest
{
  "id": 0,
  "personId": {id},
  "interestId": {id}
}

### Add a link to a person - interest connection:
POST/links
{
  "linkId": 0,
  "url": "string",
  "personWithInterestsId": {id}
}
