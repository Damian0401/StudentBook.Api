# StudentBook.Api

Simple API to manage students and classes.

## Setup

Create the SQLite database by running:

```
dotnet ef database update -s .\StudentBook.Api\ -p .\StudentBook.Api.Data\
```

## Run

Start the API with:

```
dotnet run --project .\StudentBook.Api\ --launch-profile https
```

## Docs

Json: https://localhost:7276/openapi/v1.json

UI: https://localhost:7276/docs