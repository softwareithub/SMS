#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["SERP.UI/SERP.UI.csproj", "SERP.UI/"]
COPY ["SERP.Utilities/SERP.Utilities.csproj", "SERP.Utilities/"]
COPY ["SERP.Core.Entities/SERP.Core.Entities.csproj", "SERP.Core.Entities/"]
COPY ["SERP.Core.Model/SERP.Core.Model.csproj", "SERP.Core.Model/"]
COPY ["SERP.Infrastructure.Implementation/SERP.Infrastructure.Implementation.csproj", "SERP.Infrastructure.Implementation/"]
COPY ["SERP.Infrastructure.Repository/SERP.Infrastructure.Repository.csproj", "SERP.Infrastructure.Repository/"]
COPY ["BarCodeGenerator/BarCodeGenerator.csproj", "BarCodeGenerator/"]
RUN dotnet restore "SERP.UI/SERP.UI.csproj"
COPY . .
WORKDIR "/src/SERP.UI"
RUN dotnet build "SERP.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SERP.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SERP.UI.dll"]