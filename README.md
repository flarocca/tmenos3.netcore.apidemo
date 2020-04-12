# TMenos.NetCore.ApiDemo

This repo shows an implementation of Asp.Net Core 3.1 API in conjuntion with Docker, RabbitMQ and MS SQL Server

## Requirements

- Visual Studio 2019 with Net Core 3.1
- Docker

## Technologies included

- Net Core 3.1
- Entity Framework Core 3.1
- Docker
- RabbitMQ
- MS SQL Server
- Polly
- FluentAssertions
- AutoMapper
- EnsureThat
- FakeItEasy
- Serilog with enrichers and sikns
- ELK stack for logging
- Swagger
- SQLite
- Redis
- IDistributedCache

## Deployment

There is a kubernetes deployment manifest contains everything but ELK components. This is a deployable manifest.
However, there must be a valid kubernetes cluster with access to the docker registry of your choice and a storage class.

## Tests

There are only two tests, one Integration Test and one Unit Test. Both selected because of their complexity, and they are enough to serve as examples. 
For a real development this is not acceptable and all tests must be n place