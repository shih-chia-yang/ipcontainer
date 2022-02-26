# jwt

## what is jwt

1.token base authentication
2.stateless authentication (no http session)

JSON Web Token，一種在server與client用來安的地共享資訊的開放標準，每一個JWT包含被加密的json object與一組claims。JWT利用加密演算法來保證在token產生之後claims無法被替換。
普遍使用方式是使用JWTs做為持有憑證，該方法在request of a client產生JWT並且簽名，使json object無法被其他人替換，client會將JWT連同request傳送給Rest API，而rest api會使用憑證驗證JWT的payload與header，當API驗證JWT無誤後，會取得並透過claims中的資訊允許或禁止這個client request

json format
{key:val}

jwt format:
Header    : JWT Specific information
Payload   : Claims (ClientId,ClientName,ProviderName,date,expDate..)
Signature : Base64Encode(Heaer)+Base64Encoder(Payload) <--SecretKey

Ex Token Format
{Header}.{Payload}.{Signature}

Generate Token using Jwt
read and Validate Token

## jwt flow

do not track cookie
do not track session
do not track any authentication

- 403 forbidden
- 401 UnAuthorized

1.(client)user login (username,password) => identity.api
    > 使用者傳送帳號密碼至identity.api

2.identity.api verify user info 
    > api 驗證帳號密碼成功後，產生token並回傳，驗證錯誤回傳401
    - if true return jwt token => (client)user
    - if failure return http status code :401

3.user call authorize action with jwt => resource.api
    > 呼叫resource.api中需要驗證的action，並傳送jwt

4.resource.api verify token 
    > resource.api 驗證token與user role是否有該action的權限，正確則回應結果，失敗回應401/403
    - if verify success then return action result => (client) user
    - if verify failure or the action which user don't have the right then return 401/403


- happy case
  - login => JWT => Send Request(JWT) => Info

- unhappy case1
  - login => 401

- unhappy case2
  - login =>JWT => Send Request(JWT) => 401/403

## refresh token

