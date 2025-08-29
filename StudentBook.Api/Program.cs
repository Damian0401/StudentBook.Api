using StudentBook.Api;
using StudentBook.Api.Common;
using StudentBook.Api.Core;
using StudentBook.Api.Data;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddCommon()
    .AddData()
    .AddCore()
    .AddApi();

var app = builder.Build();
app
    .UseApi()
    .MapApi();

app.Run();