# HRCoreCountriesServices
.
Projects

Project nameHR Core Services
Apr 2019 – Present

Project description.net Core access to countries data and boundaries

The Rest service is fully documented via Swagger at the following link : 
For Continuous Integration : https://fullcoreservices-ci.azurewebsites.net/swagger/index.html
For Candiate Release : https://fullcoreservices-release.azurewebsites.net/swagger/index.html


This project is integrated in Azure Dev ops for Continuous integration and Release and for continuous delivery.

All projects are in .net Standard 2.0 but the WebApiProject depends on Windows Platform in this candidate Release.

Cloud Databases use for Azure are the following

http://db.qgiscloud.com for PostGis (PostGres + Geospatial option) necessary to store HRBorders Geometries.
https://cloud.mongodb.com for mongoDB necessary to store HRCountry objects.
Credentials for ConnectionString are stored :
- secrets.json locally
- Azure Settings for CI and Release.


Tips to create your own MongoDB repository for HRCountry model :

To create mongodb cloud Database with geojson documents

1- Create a cluster on https://cloud.mongodb.com
sample : mine is HRDBMongoCluster available at : https://cloud.mongodb.com/v2/5bb86b5179358e9c19a400c1#clusters

2- Create a new collections for coutries via the previous web interface. Let's name this collection Countries

3- On your local server install mongodb server (to get the mongoimport). Watch out : Do not use any version lower than 4.0 as mongoimport does not support geojson.

4- Go in the directory of mongo where there is mongoimport. 
Sample : On ly server it's C:\Program Files\MongoDB\Server\4.0\bin

5- To import lauch in powershell on your server in the previous directory: 
./mongoimport.exe --host HRMongoDBCluster-shard-0/hrmongodbcluster-shard-00-00-wtmqq.gcp.mongodb.net:27017,hrmongodbcluster-shard-00-01-wtmqq.gcp.mongodb.net:27017,hrmongodbcluster-shard-00-02-wtmqq.gcp.mongodb.net:27017 --ssl --username YOURUSERNAME --password READABLEPASSWORD --authenticationDatabase admin --db HRMongoDBClust
er --collection Borders --type json --file C:\CODE\NET CORE\HRCoreCountriesServices\Assets\Boundaries.geojson

where : 
=> HRMongoDBCluster-shard-0/hrmongodbcluster-shard-00-00-wtmqq.gcp.mongodb.net:27017,hrmongodbcluster-shard-00-01-wtmqq.gcp.mongodb.net:27017,hrmongodbcluster-shard-00-02-wtmqq.gcp.mongodb.net:27017 --ssl is given in :
https://cloud.mongodb.com/v2/5bb86b5179358e9c19a400c1#clusters/commandLineTools/HRMongoDBCluster

=> --username YOURUSERNAME is your admin account

=> --password READABLEPASSWORD is the password of the previous account

=> --db HRMongoDBClust : is your database

=> --collection Borders : is the target collection

=> --type json : isthe type for geoJSon

=> --file C:\CODE\NET CORE\HRCoreCountriesServices\Assets\Boundaries.geojson : is your geojson








