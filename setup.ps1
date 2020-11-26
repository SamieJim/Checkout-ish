(docker network create my-network)
(dotnet dev-certs https --clean)
(cd Gateway) 
(dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Gateway.pfx -p Password) 
(dotnet dev-certs https --trust)
(dotnet user-secrets set "Kestrel:Certificates:Development:Password" "Password") 
(cd ../Payment) 
(dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Payment.pfx -p Password) 
(dotnet dev-certs https --trust)
(dotnet user-secrets set "Kestrel:Certificates:Development:Password" "Password")
(cd ../PaymentRetrieve) 
(dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\PaymentRetrieve.pfx -p Password) 
(dotnet https dev-certs --trust)
(dotnet user-secrets set "Kestrel:Certificates:Development:Password" "Password")
(cd ../Merchant) 
(dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Merchant.pfx -p Password) 
(dotnet https dev-certs --trust)
(dotnet user-secrets set "Kestrel:Certificates:Development:Password" "Password")
(cd ../Envoy) 
(dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\Envoy.pfx -p Password) 
(dotnet https dev-certs --trust)
(dotnet user-secrets set "Kestrel:Certificates:Development:Password" "Password")
(cd ..)
(openssl req -config https.config -new -out csr.pem)  
(openssl x509 -req -days 365 -extfile https.config -extensions v3_req -in csr.pem -signkey key.pem -out https.crt)  
(Move-Item https.crt Envoy/)  
(Move-Item key.pem Envoy/)
(docker-compose up --build)

