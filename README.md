# Quản lý món ăn – ASP.NET Core MVC (.NET 8) + EF Core (Code First)

## 1. Đổi MSSV (làm ĐẦU TIÊN)
Mã đang dùng MSSV mẫu `BIT240009`. Trong thư mục project chạy:

    PowerShell:  .\doi-mssv.ps1 -Mssv <MSSV_CUA_BAN>
    Git Bash:    ./doi-mssv.sh <MSSV_CUA_BAN>

Script đổi tên Model, tên file, tên bảng, tên cột và tên database (`<MSSV>_MID`).

## 2. Chạy
Cần .NET 8 SDK + SQL Server. Mặc định kết nối `localhost\SQLEXPRESS` (xem trong SSMS). Nếu máy bạn dùng
server khác (ví dụ `(localdb)\MSSQLLocalDB`), sửa `ConnectionStrings:Default` trong `appsettings.json`.

    dotnet restore
    dotnet run

Lần chạy đầu tự tạo database `<MSSV>_MID`, 3 bảng và dữ liệu mẫu (3 loại, 5 món). Mở địa chỉ in ra
trong terminal (ví dụ https://localhost:5001). Nếu đã từng chạy với schema cũ: xóa database rồi chạy lại.

## 3. Kịch bản demo (video)
1. SSMS: database, 3 bảng, các cột đều có tiền tố MSSV; mở code thấy Model `Dish_<MSSV>`...
2. Trang Dishes: tìm "phở", lọc loại / trạng thái / khoảng giá, sắp xếp; bấm tìm xong điều kiện vẫn còn.
3. Cột thời gian: "Quick" (≤ 15 phút) / "Regular"; thử Create, Edit (nhập sai để xem validation).
4. Detail: thêm ảnh bằng URL, xem nhiều ảnh, bấm "Set as thumbnail".
5. Menu Categories: xóa loại đang có món → báo lỗi. Gõ `/Dishes/Detail/999` → báo không tìm thấy.
6. Mở lần đầu = English; bấm "Tiếng Việt", đi qua List/Create/Edit/Detail, giữ điều kiện lọc.

## Cấu trúc
- `Models/` – 3 model + validation (key lỗi được dịch ở View)
- `Data/` – DbContext (quan hệ, chỉ mục duy nhất, Restrict khi xóa loại) + dữ liệu mẫu
- `Controllers/` – Dishes (list/CRUD/ảnh), Categories, Language (cookie `lang`)
- `Services/Localizer.cs` – bảng dịch EN/VI; mặc định English
- `Views/` – giao diện Bootstrap
