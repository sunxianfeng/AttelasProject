

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AttelasWebApi/AttelasWebApi.csproj", "AttelasWebApi/"]

RUN dotnet restore "AttelasWebApi/AttelasWebApi.csproj"

RUN dotnet build "AttelasWebApi/AttelasWebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 以同样的方式为flask和mysql 创建dockerfile
