FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 55432
EXPOSE 44302

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["src/demo/Alaska.Demo.WebApp/Alaska.Demo.WebApp.csproj", "src/demo/Alaska.Demo.WebApp/"]
RUN dotnet restore "src/demo/Alaska.Demo.WebApp/Alaska.Demo.WebApp.csproj"
COPY . .
WORKDIR "/src/src/demo/Alaska.Demo.WebApp"
RUN dotnet build "Alaska.Demo.WebApp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Alaska.Demo.WebApp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Alaska.Demo.WebApp.dll"]