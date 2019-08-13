FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY KubernetesPoc/KubernetesPoc.csproj KubernetesPoc/
RUN dotnet restore KubernetesPoc/KubernetesPoc.csproj
COPY . .
WORKDIR /src/KubernetesPoc
RUN dotnet build KubernetesPoc.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish KubernetesPoc.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KubernetesPoc.dll"]
