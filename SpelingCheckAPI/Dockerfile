FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 1: Build .NET application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SpelingCheckAPI/SpelingCheckAPI.csproj", "SpelingCheckAPI/"]
COPY ["CommonCode/CommonCode.csproj", "CommonCode/"]
RUN dotnet restore "SpelingCheckAPI/SpelingCheckAPI.csproj"
COPY . .
WORKDIR "/src/SpelingCheckAPI"
RUN dotnet build "SpelingCheckAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 2: Publish .NET application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SpelingCheckAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish -p:UseAppHost=false

# Stage 3: Setup Java
FROM openjdk:11-jdk AS java-build
COPY ["SpelingCheckAPI/LanguageTool", "LanguageTool"]
WORKDIR /LanguageTool

# Stage 4: Final stage, .NET runtime and Java
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=java-build /usr/local/openjdk-11 /usr/local/openjdk-11
COPY --from=java-build /LanguageTool /app/LanguageTool
COPY ["SpelingCheckAPI/MLPrepositionChecker", "/app/MLPrepositionChecker"]
ENV PATH="${PATH}:/usr/local/openjdk-11/bin"
ENTRYPOINT ["dotnet", "SpelingCheckAPI.dll"]