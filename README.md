# BlogApi

# Endpoints
```PM
v1

POST   Account/register
POST   Account/login
GET  	 Account/profile
GET    Account{user_id}

GET     blogs
POST    blogs
GET  	blogs/{blogId}
DELETE  blogs/{blogId}
PUT     blogs/{blogId}
GET  	blogs/{blogId}/posts
POST 	blogs/{blogId}/posts
GET  	blogs/{blogId}/posts/{postId}
DELETE  blogs/{blogId}/posts/{postId}
PUT     blogs/{blogId}/posts/{postId}
POST 	blogs/{blogId}/posts/{postId}/comments
GET  	blogs/{blogId}/posts/{postId}/comments
DELETE  blogs/{blogId}/posts/{postId}/comments/{commentId}
PUT     blogs/{blogId}/posts/{postId}/comments/{commentId}
POST 	blogs/{blogId}/posts/{postId}/save
POST 	blogs/{blogId}/posts/{postId}/like
