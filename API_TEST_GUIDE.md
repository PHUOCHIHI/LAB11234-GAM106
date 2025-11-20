# Hướng dẫn Test các API

## 1. API ForgotPassword
**Method:** POST  
**URL:** `https://localhost:7129/api/APIGame/ForgotPassword`  
**Body (JSON):**
```json
{
    "Email": "user@example.com"
}
```
**Expected Response:**
```json
{
    "IsSuccess": true,
    "Notification": "Gửi mã OTP thành công",
    "Data": "email sent to user@example.com"
}
```

## 2. API CheckOTP
**Method:** POST  
**URL:** `https://localhost:7129/api/APIGame/CheckOTP`  
**Body (JSON):**
```json
{
    "Email": "user@example.com",
    "OTP": "123456"
}
```
**Expected Response (OTP đúng):**
```json
{
    "IsSuccess": true,
    "Notification": "Mã OTP chính xác",
    "Data": "user@example.com"
}
```

## 3. API ResetPassword
**Method:** POST  
**URL:** `https://localhost:7129/api/APIGame/ResetPassword`  
**Body (JSON):**
```json
{
    "Email": "user@example.com",
    "OTP": "123456",
    "NewPassword": "newpassword123"
}
```
**Expected Response:**
```json
{
    "IsSuccess": true,
    "Notification": "Đổi mật khẩu thành công",
    "Data": "user@example.com"
}
```

## 4. API ChangeUserPassword
**Method:** PUT  
**URL:** `https://localhost:7129/api/APIGame/ChangeUserPassword`  
**Body (JSON):**
```json
{
    "UserId": "user-id-here",
    "OldPassword": "oldpassword",
    "NewPassword": "newpassword"
}
```

## 5. API UpdateUserInformation
**Method:** PUT  
**URL:** `https://localhost:7129/api/APIGame/UpdateUserInformation`  
**Body (FormData):**
- UserId: "user-id"
- Name: "Tên mới"
- RegionId: 1
- Avatar: [file] (optional)

## 6. API DeleteAccount
**Method:** DELETE  
**URL:** `https://localhost:7129/api/APIGame/DeleteAccount/{userId}`

## Lưu ý:
- Thay `https://localhost:7129` bằng URL thực tế khi ứng dụng chạy
- Kiểm tra console output để xem URL chính xác
- Đảm bảo database đã được tạo và migrations đã được áp dụng

