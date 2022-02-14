# jwt

do not track cookie
do not track session
do not track any authentication

- 403 forbidden
- 401 UnAuthorized

1.(client)user login (username,password) => api

2.api verify user info 
    - if true return jwt token => (client)user
    - if failure return http status code :401

3.user call authorize action with jwt => api

4.api verify token 
    - if verify success then return action result => (client) user
    - if verify failure or the action which user don't have the right then return 401/403


- happy case
  - login => JWT => Send Request(JWT) => Info

- unhappy case1
  - login => 401

- unhappy case2
  - login =>JWT => Send Request(JWT) => 401/403

