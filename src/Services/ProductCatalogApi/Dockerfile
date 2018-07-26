FROM microsoft/dotnet:latest AS build

WORKDIR /code

COPY . .

RUN dotnet restore

RUN dotnet publish --output /out/ --configuration Release 

FROM microsoft/dotnet

COPY --from=build /out /app/

WORKDIR /app 

ENTRYPOINT ["dotnet","ProductCatalogApi.dll"]