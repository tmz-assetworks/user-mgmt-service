#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8081
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
ENV EncryptDecryptkey="E534C8DF286CD5931069B522E695D4F1"
ENV AzureAd__scopes="user.read"
ENV AzureAd__ClientId="7698cbed-7d9f-43b3-b9cd-a4f09b9b55ed"
ENV AzureAd__Instance="https://login.microsoftonline.com/"
ENV AzureAd__TenantId="744aa8b0-bb99-4982-903f-52328216b4be"
ENV AzureAd__clientSecret="xg.8Q~mpJZWo5su4_WIKRaVaydfHP99xFa15uak_"
#ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "UsersService.Api.dll"]