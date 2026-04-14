FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# ادخل إلى مجلد المشروع الحقيقي
COPY SoftHub/*.csproj ./SoftHub/
WORKDIR /app/SoftHub
RUN dotnet restore

# انسخ باقي الملفات
COPY SoftHub/. ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/SoftHub/out .
ENTRYPOINT ["dotnet", "SoftHub.dll"]