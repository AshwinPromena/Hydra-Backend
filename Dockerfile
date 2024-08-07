FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/out .
COPY Hydra/Templates/PasswordResetOtp.html /App/Templates/PasswordResetOtp.html
COPY Hydra/Templates/StaffLoginCredential.html /App/Templates/StaffLoginCredential.html
RUN ls
ENTRYPOINT ["dotnet", "Hydra.dll"]
EXPOSE 8080