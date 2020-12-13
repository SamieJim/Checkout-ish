# Microservice architecture with an API gateway, using Envoy proxy - Payment gateway.
## Description of the system

I have two portals runnning on different ports, one for the merchant to query the transaction DB and the other for the payee to pay. These portals both speak to the API gateway that I have created for ingress only using Envoy. They independently ask the API gateway for the relevant microservice and one is spun up for them, e.g. the payment portal contacts the API gateway to make a transaction, the API gateway uses SSL to retrieve the relevant container over HTTPS, the container is spun up to service transaction requests from that payee.   
An SQL server is running on azure serving requests.  
Traffic is encrypted with HTTPS.  
Using Envoy is an excellent way of collecting logs and system information from the administration console (running on localhost:9901) and these can be integrated with prometheus - a concept I would enoy fleshing out in future.

## Considerations & Concessions

1. Payment models for transaction etc. have been repeated in the Bank mock. In reality, the bank would have no shared resources with the payment gateway and would have its own data models for transactions etc.

2. To simulate the bank I am assuming infinite funds, that the merchant exists within the bank and that the payment is successful. I am also not taking into account the merchants use of the gateway as this is left as a use case.

3. Originally the bank was simulated via a seperate API - in the interest of time I had to change the bank to a mock rather than configure an envoy egress port.

4. In order to enable SSL between the client portal and the __for use in this demo environment only__, I have programmed around some https cert verification in my code. __I have only done this to make it easier to demonstrate to the reviewer__ and am aware that this would never be used beyond this excecise.

5. As this was a backend task I didnt put large amounts of time into front end design but I have provided a client to call my gateway behind the API gateway - therefore my frontend is basic.

6. For the "bonus points" I have implemented a custom API gateway with Envoy and created a microservice architecture by pairing this with docker-compose.

7. My frontend testing is basic as this is a backend excercise.

8. I had a bug with my current culture always being InvariantCulture and so in the end was forced to hard code the symbol, with more time I would have found the error, but decided instead to move on to more important features.

## Set-up 
### Platform
- UNIX based.
- Open SSL.
- Docker & docker-compose.
- Root permissions for adding certs.

### App (Updated - Sorry Windows guys!)

1. In the root folder, there is a powershell script called setup.ps1 containing all of the commands required to set up the SSL capabilities of the application on localhost and then spin up the docker containers. You can run these manually if you'd like, or launch Windows PowerShell, navigate to the project root (where the script lives), type __PS> .\setup.ps1__ and hit enter. Make sure that the project folder is called __Checkout-ish__ rather than __Checkout-ish-master__ as git can sometimes name it :).

2. You may be prompted to type some passwords for root permissions, do so.

3. After a few moments, the payment portal can be accessed on __http://localhost:5000__ and the merchant portal on port 5001 of the same address (__http://localhost:5001__).

4. Please contact me if there are any issues.
