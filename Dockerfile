FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY bin/Release/net5.0/publish/ app/
WORKDIR /app
EXPOSE 5000
EXPOSE 3306

# copy csproj and restore as distinct layers

#COPY *.csproj ./
#RUN dotnet restore
#
## copy everything else and build app
#COPY /. ./
#WORKDIR /app/
#RUN dotnet publish -c Release -o out

#FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
#WORKDIR /app
#COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "NotesAPI.dll"]