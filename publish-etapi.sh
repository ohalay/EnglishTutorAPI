cd /tmp/EnglishTutorAPI
git pull
cd EnglishTutor.Api
dotnet restore
dotnet publish -c "Release"
cp /tmp/EnglishTutorAPI/EnglishTutor.Api/bin/Release/netcoreapp1.1/publish /tmp/server
sudo systemctl restart et-api.service
systemctl status et-api.service