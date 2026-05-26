# JWT Authentication Setup - User Management

## Tổng Quan
Hệ thống xác thực JWT đã được triển khai hoàn chỉnh cho User Management API.

## API Endpoints

### 1. Register (Đăng ký)
```
POST /api/user/register
Content-Type: application/json

{
  "username": "john_doe",
  "password": "password123",
  "fullName": "John Doe",
  "email": "john@example.com",
  "phone": "0123456789"
}
```

**Response (201 Created):**
```json
{
  "userId": 1,
  "username": "john_doe",
  "fullName": "John Doe",
  "email": "john@example.com",
  "phone": "0123456789"
}
```

---

### 2. Login (Đăng nhập)
```
POST /api/user/login
Content-Type: application/json

{
  "username": "john_doe",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Đăng nhập thành công",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "userId": 1,
    "username": "john_doe",
    "fullName": "John Doe",
    "email": "john@example.com",
    "phone": "0123456789"
  }
}
```

**Response khi thất bại (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Username hoặc password không đúng",
  "token": null,
  "user": null
}
```

---

### 3. Get All Users (Lấy danh sách Users) - [Yêu cầu Authorization]
```
GET /api/user
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
[
  {
    "userId": 1,
    "username": "john_doe",
    "fullName": "John Doe",
    "email": "john@example.com",
    "phone": "0123456789"
  }
]
```

---

### 4. Get User by ID (Lấy User theo ID) - [Yêu cầu Authorization]
```
GET /api/user/{id}
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "userId": 1,
  "username": "john_doe",
  "fullName": "John Doe",
  "email": "john@example.com",
  "phone": "0123456789"
}
```

---

### 5. Update User (Cập nhật thông tin User) - [Yêu cầu Authorization]
```
PUT /api/user/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "userId": 1,
  "username": "john_doe",
  "fullName": "John Doe Updated",
  "email": "newemail@example.com",
  "phone": "9876543210"
}
```

**Response (200 OK):**
```json
{
  "userId": 1,
  "username": "john_doe",
  "fullName": "John Doe Updated",
  "email": "newemail@example.com",
  "phone": "9876543210"
}
```

---

### 6. Delete User (Xoá User) - [Yêu cầu Authorization]
```
DELETE /api/user/{id}
Authorization: Bearer {token}
```

**Response (200 OK):**
```json
{
  "message": "Xoá user thành công"
}
```

---

## Cấu Trúc Project

### DataAccessLayer
- **Models**: `User.cs` - Model User
- **DTOs**:
  - `User/UserDTO.cs` - DTO cho User (không chứa password)
  - `User/UserLoginDTO.cs` - DTO cho login request
  - `User/UserRegisterDTO.cs` - DTO cho register request
  - `User/LoginResponseDTO.cs` - DTO cho login response (chứa JWT token)
- **Repository**:
  - `IUserRepository.cs` - Interface
  - `UserRepository.cs` - Implementation (CRUD + GetByUsername)

### BusinessLayer
- **Services**:
  - `IUserService.cs` - Interface
  - `UserService.cs` - Implementation (Business Logic + JWT Generation + Password Hashing)

### ShopAPI_3Layer
- **Controllers**: `UserController.cs` - API Endpoints
- **Configuration**:
  - `appsettings.json` - JWT configuration
  - `Program.cs` - Dependency Injection + JWT Authentication Setup

---

## JWT Configuration

Cập nhật `appsettings.json`:
```json
"Jwt": {
  "SecretKey": "your-very-long-secret-key-for-jwt-authentication-here-must-be-at-least-32-characters",
  "ExpirationMinutes": 60,
  "Issuer": "ShopAPI",
  "Audience": "ShopAPIUsers"
}
```

**⚠️ QUAN TRỌNG**: Thay đổi `SecretKey` thành một khóa bí mật mạnh hơn!

---

## Tính Năng

✅ **User Registration** - Đăng ký tài khoản mới
✅ **User Login** - Đăng nhập và nhận JWT Token
✅ **Password Hashing** - Mật khẩu được hash bằng SHA256
✅ **JWT Authentication** - Các endpoint CRUD được bảo vệ bằng JWT
✅ **User CRUD** - Tạo, đọc, cập nhật, xóa user
✅ **Authorization** - Chỉ user đã xác thực mới có thể truy cập các endpoint được bảo vệ

---

## Test API với Postman

1. **Đăng ký**: POST `http://localhost:5000/api/user/register`
2. **Đăng nhập**: POST `http://localhost:5000/api/user/login`
3. **Copy token** từ login response
4. **Thêm vào Authorization header** cho các request khác:
   - Authorization: `Bearer {token}`

---

## Cài đặt Dependencies

Các NuGet packages đã được thêm:
- `Microsoft.AspNetCore.Authentication.JwtBearer` (v8.0.0)
- `Microsoft.Extensions.Configuration.Abstractions` (v8.0.0)

---

## Mã Lỗi & Messages

| Scenario | HTTP Status | Message |
|----------|------------|---------|
| Login thành công | 200 | "Đăng nhập thành công" |
| Username hoặc password sai | 401 | "Username hoặc password không đúng" |
| Username đã tồn tại | 400 | "Username đã tồn tại" |
| User không tồn tại | 404 | "User không tồn tại" |
| ID không khớp | 400 | "ID không khớp" |
| Không có token | 401 | Unauthorized |
| Token hết hạn | 401 | Unauthorized |

---

## Lưu Ý Bảo Mật

1. **Thay đổi SecretKey** - Sử dụng một khóa bí mật mạnh hơn trong production
2. **HTTPS** - Luôn sử dụng HTTPS trong production
3. **Token Expiration** - Thiết lập thời gian hết hạn token phù hợp
4. **Password Hashing** - Mật khẩu luôn được hash, không lưu clear text

---

## Tác Giả
Generated by GitHub Copilot
