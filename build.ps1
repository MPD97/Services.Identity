docker build -t service.identity . ;
docker tag service.identity mateusz9090/identity:local ;
docker push mateusz9090/identity:local