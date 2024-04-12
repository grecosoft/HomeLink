FROM  mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /build/src

ARG NUGET_USERNAME
ARG NUGET_PASSWORD

# --------------------------------------------------------------------------------------
# RESTORE 
# --------------------------------------------------------------------------------------
# - This section copies only the solution and projects files used to restore the needed 
#   NuGet packages.  The restored packages are placed in:  /root/.nuget/packages.
#
# - Since the source of the microservice will change the most often, these layers will
#   be reused and will not have be recreated when the updated source is built. 
# --------------------------------------------------------------------------------------

# Copy microservice components:
COPY ./src/Components/HomeLink.Management.App/*.csproj ./Components/HomeLink.Management.App/
COPY ./src/Components/HomeLink.Management.Domain/*.csproj ./Components/HomeLink.Management.Domain/
COPY ./src/Components/HomeLink.Management.Infra/*.csproj ./Components/HomeLink.Management.Infra/

# Copy microservice unit tests and web host:
COPY ./src/HomeLink.Management.Tests/*.csproj ./HomeLink.Management.Tests/
COPY ./src/HomeLink.Management.WebApi/*.csproj ./HomeLink.Management.WebApi/

# Copy the solution file to restore all projects:
COPY ./src/HomeLink.Management.sln ./

RUN dotnet nuget add source https://nuget.pkg.github.com/grecosoft/index.json \
      --name="GitHub Package Repository" \
      --username ${NUGET_USERNAME} \
      --valid-authentication-types basic \
      --store-password-in-clear-text \
      --password ${NUGET_PASSWORD}

RUN dotnet restore 

# Copy all the source and build the microservice.
COPY ./src ./

# --------------------------------------------------------------------------------------
# PUBLISH
# --------------------------------------------------------------------------------------
FROM restore AS publish
RUN dotnet publish ./HomeLink.Management.WebApi/HomeLink.Management.WebApi.csproj \
      -c release \
      --output ../out \
      --no-restore \
      /p:DebugType=None

# --------------------------------------------------------------------------------------
# CREATE IMAGE FOR CONTAINER CREATION
# - This stage takes the published output and copies it to a layer
#   belonging to a new Docker image based on the runtime .net image. 
# --------------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /microservice

COPY --from=publish /build/out ./
ENTRYPOINT ["dotnet", "HomeLink.Management.WebApi.dll"]
EXPOSE 7200