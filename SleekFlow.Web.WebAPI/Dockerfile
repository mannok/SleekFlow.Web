#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SleekFlow.Web.WebAPI/SleekFlow.Web.WebAPI.csproj", "SleekFlow.Web.WebAPI/"]
RUN dotnet restore "SleekFlow.Web.WebAPI/SleekFlow.Web.WebAPI.csproj"
COPY . .
WORKDIR "/src/SleekFlow.Web.WebAPI"
RUN dotnet build "SleekFlow.Web.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SleekFlow.Web.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SleekFlow.Web.WebAPI.dll"]