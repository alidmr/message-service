## message-service

## How to use:
- You will need .NET SDK 6.0
- The latest SDK and tools can be downloaded from https://dot.net/core.

Also you can run the in Visual Studio (Windows) and Visual Studio Code (Windows, Linux or MacOS).

To know more about how to setup your enviroment visit the [Microsoft .NET Download Guide](https://www.microsoft.com/net/download)


## How to setup via local docker compose elastic-kibana:
```sh
$ docker-compose -f docker-compose-mongodb.yml up -d
$ docker-compose -f docker-compose-rabbitmq.yml up -d
$ docker-compose -f docker-compose-elastic.yml up -d
$ open http://localhost:27017
$ open http://localhost:15672
$ open http://localhost:9200
$ open http://localhost:5601
```

## How to setup via docker compose:
```sh
$ docker-compose build
$ docker-compose up
$ open http://localhost:27017
$ open http://localhost:15672
$ open http://localhost:9200
$ open http://localhost:5601
$ open http://localhost:1200
```

## For stop all containers
```sh
$ docker-compose down
```

## For stop containers
```sh
$ docker-compose stop [container_name]
```

## For remove container:
```sh
$ docker-compose rm [container_id] -f
$ docker rm [container_id]
```

## Technologies implemented:

- [x] .net6
- [x] dockerize
- [x] docker compose
- [x] mongodb
- [x] rabbitmq
- [x] logging + elasticsearch + kibana
- [x] global exception handling
- [x] mediatR
- [x] fluentvalidation