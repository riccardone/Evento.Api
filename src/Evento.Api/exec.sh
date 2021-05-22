#!/usr/bin/env bash
docker build -t evento-api .
docker run -it -p 3000:3000 evento-api
