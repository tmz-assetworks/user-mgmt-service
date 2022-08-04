#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 6008
EXPOSE 443

#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["UsersService.Api.csproj", "."]
#RUN dotnet restore "UsersService.Api.csproj"
#COPY . .
#WORKDIR "."
#RUN dotnet build "UsersService.Api.csproj" -c Release -o /app/build

#FROM build AS publish
#RUN dotnet publish "UsersService.Api.csproj" -c Release -o /app/publish
#
FROM base AS final
WORKDIR /app
#ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT="production"
COPY /app/publish /app/publish .
ENV DECRYPTKEY="E534C8DF286CD5931069B522E695D4F1"
ENV AZUREAD_SCOPE="user.read"
ENV AZUREAD_CID="7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed"
ENV AZUREAD_INSTANCE="https://login.microsoftonline.com/"
ENV AZUREAD_TID="744aa8b0-bb99-4982-903f-52328216b4be"
ENV AZUREAD_CLIENT_SECRET="xg.8Q~mpJZWo5su4_WIKRaVaydfHP99xFa15uak_"
ENV JWT_KEY="ThisismySecretKey"
ENV JWT_ISSUER="Asset_AuthService"
ENV JWT_AUD="Asset_DashboardService"
#ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "UsersService.Api.dll"]