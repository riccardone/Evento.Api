# Evento.Api
  
[![Deploy to PROD](https://github.com/riccardone/Evento.Api/actions/workflows/build-push-deploy-prod.yml/badge.svg)](https://github.com/riccardone/Evento.Api/actions/workflows/build-push-deploy-prod.yml)

Multitenant Distributed architecture for processing transactions with resiliency to failures, scalability, fast onboarding of new projects.  
[![Ingestion Api](./messaging-architecture-01.png)](http://www.dinuzzo.co.uk/2019/02/16/distributed-architecture-01-the-ingestion/)  

Scaled over tenants and projects using the same single Api entry point. Each CloudEvent message can be validated over different schemas
[![Ingestion Api](./messaging-architecture-02.png)](http://www.dinuzzo.co.uk/2019/02/16/distributed-architecture-01-the-ingestion/)  

More details here http://www.dinuzzo.co.uk/2019/02/16/distributed-architecture-01-the-ingestion/ 