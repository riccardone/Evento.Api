# Evento.Api
  
[![Deploy to PROD](https://github.com/riccardone/Evento.Api/actions/workflows/build-push-deploy-prod.yml/badge.svg)](https://github.com/riccardone/Evento.Api/actions/workflows/build-push-deploy-prod.yml)

This Web Api is one of the building blocks part of my Multitenant Distributed architecture for processing transactions with multiple schemas, fault tolerance, scalability, fast onboarding of new projects/tenants.  
[![Ingestion Api](./messaging-architecture-01.png)](http://www.dinuzzo.co.uk/2019/02/16/distributed-architecture-01-the-ingestion/)  

Scaled over tenants and projects using the same single Api entry point. Each CloudEvent message is validated over different schemas
[![Ingestion Api](./messaging-architecture-02.png)](http://www.dinuzzo.co.uk/2019/02/16/distributed-architecture-01-the-ingestion/)  

More details here http://www.dinuzzo.co.uk/2019/02/16/distributed-architecture-01-the-ingestion/ 
