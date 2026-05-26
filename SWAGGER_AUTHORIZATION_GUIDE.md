# Swagger JWT Authorization Guide

## 🔐 Cách Sử Dụng Authorize Button trong Swagger UI

### 1. Mở Swagger UI
- Truy cập: `https://localhost:7XXX/swagger/index.html` (port của bạn)
- Bạn sẽ thấy nút **"Authorize"** ở góc trên bên phải (màu xanh)

### 2. Đăng Nhập để Lấy Token
**Step 1**: Scroll down đến endpoint `POST /api/user/login`

**Step 2**: Click "Try it out" button

**Step 3**: Nhập thông tin đăng nhập:
```json
{
  "username": "john_doe",
  "password": "password123"
}
```

**Step 4**: Click "Execute" button

**Step 5**: Sao chép `token` từ Response:
```json
{
  "success": true,
  "message": "Đăng nhập thành công",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxIiwibmFtZSI6ImpvaG5fZG9lIiwibmFtZWlkIjoiMSIsImV4cCI6MTY5NTM0MjM0MCwiaXNzIjoiU2hvcEFQSSIsImF1ZCI6IlNob3BBUElVc2VycyJ9...",
  "user": { ... }
}
```

### 3. Authorize with Token
**Step 1**: Click nút **"Authorize"** (góc trên bên phải)

**Step 2**: Cửa sổ Authorization sẽ mở ra

**Step 3**: Dán token vào field, có 2 cách:

#### Cách 1: Dán toàn bộ token
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxIiwibmFtZSI6ImpvaG5fZG9lIiwibmFtZWlkIjoiMSIsImV4cCI6MTY5NTM0MjM0MCwiaXNzIjoiU2hvcEFQSSIsImF1ZCI6IlNob3BBUElVc2VycyJ9...
```

#### Cách 2: Dán với prefix "Bearer" (Được khuyên nghị)
```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VySWQiOiIxIiwibmFtZSI6ImpvaG5fZG9lIiwibmFtZWlkIjoiMSIsImV4cCI6MTY5NTM0MjM0MCwiaXNzIjoiU2hvcEFQSSIsImF1ZCI6IlNob3BBUElVc2VycyJ9...
```

**Step 4**: Click "Authorize" button (xanh)

**Step 5**: Bạn sẽ thấy thông báo:
- ✅ "Authorized" - Token hợp lệ
- ❌ "Unauthorised" - Token không hợp lệ

### 4. Test Protected Endpoints
Giờ bạn có thể test các endpoint được bảo vệ:
- ✅ `GET /api/user` - Lấy danh sách users
- ✅ `GET /api/user/{id}` - Lấy user theo ID
- ✅ `PUT /api/user/{id}` - Cập nhật user
- ✅ `DELETE /api/user/{id}` - Xóa user

Mỗi request sẽ tự động thêm header:
```
Authorization: Bearer {your_token}
```

### 5. Logout (Xóa Token)
**Step 1**: Click nút **"Authorize"** lại

**Step 2**: Click nút **"Logout"** (đỏ)

---

## 📝 Thông Tin Token

### JWT Token Structure
Token bao gồm 3 phần (cách nhau bằng dấu `.`):
```
header.payload.signature
```

### Claims trong Token
```json
{
  "UserId": "1",
  "name": "john_doe",
  "nameid": "1",
  "exp": 1695342340,
  "iss": "ShopAPI",
  "aud": "ShopAPIUsers"
}
```

### Token Expiration
- **Thời gian hết hạn**: Được cấu hình trong `appsettings.json` (mặc định 60 phút)
- **Hết hạn**: Phải đăng nhập lại để lấy token mới

---

## 🔧 Cấu Hình Swagger

Cấu hình JWT trong `Program.cs`:
```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
```

---

## 📊 Quy Trình Đầy Đủ

```
1. Register (Optional)
   POST /api/user/register
   → Tạo tài khoản mới

2. Login
   POST /api/user/login
   → Nhận JWT token

3. Authorize in Swagger
   Click "Authorize" button
   → Paste token

4. Access Protected Endpoints
   GET /api/user
   GET /api/user/{id}
   PUT /api/user/{id}
   DELETE /api/user/{id}

5. Logout (Optional)
   Click "Authorize" → "Logout"
   → Xóa token khỏi Swagger
```

---

## ⚠️ Ghi Chú Quan Trọng

1. **Token hết hạn**: Bạn sẽ nhận lỗi 401 Unauthorized
   - Giải pháp: Đăng nhập lại để lấy token mới

2. **Token không hợp lệ**: Format sai hoặc khóa bí mật không khớp
   - Giải pháp: Kiểm tra token và SecretKey

3. **CORS issue** (nếu frontend khác domain):
   - Cấu hình CORS trong `Program.cs`

4. **Password hashing**:
   - Mật khẩu được hash bằng **SHA256**
   - Không thể recover password từ hash

---

## 🧪 Test Endpoints

### Register
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

### Login
```
POST /api/user/login
Content-Type: application/json

{
  "username": "john_doe",
  "password": "password123"
}
```

### Get All Users (Require Authorization)
```
GET /api/user
Authorization: Bearer {token}
```

### Get User by ID (Require Authorization)
```
GET /api/user/1
Authorization: Bearer {token}
```

### Update User (Require Authorization)
```
PUT /api/user/1
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

### Delete User (Require Authorization)
```
DELETE /api/user/1
Authorization: Bearer {token}
```

---

## 🎯 Troubleshooting

| Problem | Solution |
|---------|----------|
| "Authorize" button không hiển thị | Kiểm tra cấu hình Swagger trong Program.cs |
| Token không được nhận | Dùng format "Bearer {token}" hoặc chỉ "{token}" |
| 401 Unauthorized error | Token hết hạn, đăng nhập lại |
| 403 Forbidden error | Token không có quyền truy cập resource |
| CORS error | Cấu hình CORS trong Program.cs |

---

Generated by GitHub Copilot
