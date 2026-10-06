# Bài tập môn Lập trình Web — Hệ thống quản lý nhân sự

**Hệ thống quản lý nhân viên, phòng ban và tính lương** được xây dựng bằng **ASP.NET Core MVC**,
lưu dữ liệu trên **Microsoft SQL Server**, theo **kiến trúc 3 tầng** (PL – BLL – DAL)
kết hợp **Repository + Unit of Work**, **ASP.NET Identity** cho xác thực và phân quyền.
Toàn bộ giao diện và thông báo hiển thị bằng **tiếng Việt**.

---

## Nhóm thực hiện

| Mã sinh viên | Họ và tên        |
|--------------|------------------|
| V5250485     | Bùi Đức Quỳnh    |
| V5250489     | Ngô Văn Tân       |
| V5250496     | Phùng Quang Tùng |
| V5250498     | Lều Quang Tuyên  |

---

## 1. Công nghệ sử dụng

| Hạng mục            | Công nghệ / Phiên bản                                              |
|---------------------|--------------------------------------------------------------------|
| Runtime / Framework | .NET 10 (`net10.0`), ASP.NET Core MVC                              |
| ORM                 | Entity Framework Core 10.0.11 (Code First + Migrations)            |
| Database            | Microsoft SQL Server (chạy Docker hoặc cài đặt cục bộ, port 1433)  |
| Xác thực / Phân quyền | ASP.NET Core Identity (cookie, vai trò `Admin` / `Editor` / `User`) |
| Ánh xạ dữ liệu     | AutoMapper 16.2.0 (`MappingProfiles`)                              |
| Xuất Excel          | ClosedXML 0.105.1                                                  |
| Gửi email           | SMTP Gmail (`EmailSettings` trong `appsettings.json`)              |
| Giao diện           | Bootstrap, jQuery, jQuery Unobtrusive Validation / Unobtrusive Ajax |
| Chạy lại view khi dev | `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation`              |
| Ngôn ngữ hiển thị   | Đặt mặc định `vi-VN` (định dạng ngày, số, tiền theo locale Việt)  |

---

## 2. Kiến trúc & cấu trúc thư mục

```
MVC03 Soluation.sln
├── Demo.PL   — Presentation (giao diện & điều phối)
│   ├── Controllers/          # Account, Home, Department, Employee, Payroll, Report, User
│   ├── Views/                # Razor Views + Shared/_Layout (thanh điều hướng)
│   ├── ViewModels/           # ViewModel cho từng màn hình
│   ├── Helpers/              # Roles, DisplayNames, DocumentSettings, MappingProfiles,
│   │                         # VietnameseIdentityErrorDescriber, EmailSettings, Email...
│   ├── Extentions/           # Đăng ký DI (IUnitOfWork)
│   ├── wwwroot/              # css, js, lib, files/images (ảnh nhân viên)
│   ├── Program.cs            # Seed vai trò + Database.Migrate() khi khởi động
│   ├── Startup.cs            # Cấu hình services / middleware / localization / cookie
│   └── appsettings.json      # Chuỗi kết nối, EmailSettings, Logging
├── Demo.BLL   — Business Logic
│   ├── Interfaces/           # IUnitOfWork, IGenaricRepository, IEmployeeRepository,
│   │                         # IDepartmentRepository, IPayrollRepository
│   └── Reopsitories/         # UnitOfWork + các Repository triển khai
└── Demo.DAL   — Data Access
    ├── Data/Context/         # AppDbContext (kế thừa IdentityDbContext)
    ├── Data/Migrations/      # 8 migration (intialCreate → PayrollFeatures)
    ├── Data/Configruations/  # Cấu hình Entity (Fluent API) — tên folder theo repo
    └── Models/               # Department, Employee, PayrollPeriod, Payslip,
                              # ApplicationUser, Email...
```

**Dòng dữ liệu:** `Controller (PL)` → `UnitOfWork / Repository (BLL)` → `Entity Framework Core (DAL)` → `SQL Server`.
`AutoMapper` chuyển đổi giữa Entity ↔ ViewModel ở tầng PL.

---

## 3. Đặc tả sơ bộ (chi tiết từng phân hệ)

### 3.1. Đăng nhập / Đăng ký / Quên mật khẩu (`AccountController`)

- **Đăng ký** (`/Account/SignUp`): họ tên, email, mật khẩu, xác nhận mật khẩu, đồng ý điều khoản.
  Tài khoản đăng ký mới **tự động được gán vai trò `Admin`** (cài đặt demo — mọi tài khoản chưa có vai trò đều được gán `Admin` khi khởi động).
- **Đăng nhập** (`/Account/SignIn`): cookie xác thực, sliding expiration 10 phút; khi chưa đăng nhập sẽ được chuyển về `/Account/SignIn`.
- **Đăng xuất** (`/Account/SignOut`).
- **Quên mật khẩu** (`/Account/ForgetPassword`): gửi link đặt lại mật khẩu qua email → `CheckYourInbox` → `ResetPassword(email, token)`.
- **Chính sách mật khẩu** (cấu hình ở `Startup.cs`):
  - Ít nhất 1 ký tự hoa, 1 ký tự thường, 1 chữ số, 1 ký tự đặc biệt;
  - Tối thiểu **2 ký tự trùng lặp**;
  - Email bắt buộc duy nhất (`RequireUniqueEmail`);
  - **Khóa tài khoản 10 phút** sau **3 lần** đăng nhập sai.
- **Trang từ chối truy cập:** `/Account/AccessDenied` (người dùng không đủ quyền).

### 3.2. Bảng điều khiển — Dashboard (`HomeController`)

Yêu cầu đăng nhập. Các chỉ số hiển thị:

- Tổng số nhân viên / số nhân viên đang làm việc;
- Tổng số phòng ban;
- Số kỳ lương đã chốt;
- Lương trung bình;
- Kỳ lương mới nhất (tên kỳ + tổng quỹ lương);
- 5 kỳ lương gần nhất;
- **Biểu đồ quỹ lương theo phòng ban** (tên phòng ban đã hiển thị tiếng Việt, sắp xếp giảm dần theo tổng lương).

### 3.3. Quản lý phòng ban (`DepartmentController`)

| Chức năng              | Đường dẫn                 | Quyền        |
|------------------------|---------------------------|--------------|
| Danh sách               | `/Department`             | Đã đăng nhập |
| Thêm mới               | `/Department/Create`      | Admin, Editor|
| Chi tiết (kèm nhân viên)| `/Department/Details/{id}`| Đã đăng nhập |
| Sửa                    | `/Department/Edit/{id}`   | Admin, Editor|
| Xoá                    | `/Department/Delete/{id}` | Admin, Editor|

- Tên phòng ban hiển thị tiếng Việt ở mọi màn hình (map qua `Helpers/DisplayNames.cs`), dữ liệu lưu trong DB vẫn là giá trị gốc.
- Form phòng ban hiển thị/ghi lại tên gốc để tránh lưu nhầm tên đã dịch.
- Tất cả POST đều có `[ValidateAntiForgeryToken]`.

### 3.4. Quản lý nhân viên (`EmployeeController`)

| Chức năng              | Đường dẫn                  | Quyền        |
|------------------------|----------------------------|--------------|
| Danh sách + tìm kiếm   | `/Employee?searchInp=...`  | Đã đăng nhập |
| Thêm mới               | `/Employee/Create`         | Admin, Editor|
| Chi tiết               | `/Employee/Details/{id}`   | Đã đăng nhập |
| Sửa                    | `/Employee/Edit/{id}`      | Admin, Editor|
| Xoá (xoá mềm)          | `/Employee/Delete/{id}`    | Admin, Editor|

- **Tìm kiếm theo tên:** `Name.ToLower().Contains(từ khoá)` (SQL `LIKE`), có `.Include(Department)` để hiển thị tên phòng ban.
- **Ảnh đại diện:** upload file → lưu tại `wwwroot/files/images/` với tên
  `GUID + đuôi file gốc` (`DocumentSettings.UploadFile`), cột `ImageName` lưu tên file.
- **Xoá mềm:** đặt `IsDeleted = true`, nhân viên bị xoá không tham gia tính lương và thống kê.
- **Dữ liệu hợp lệ khi tính lương:** nhân viên `IsActive = true` và `IsDeleted = false`.
- Kiểm tra `DepartmentId` có tồn tại trước khi lưu ("Phòng ban không tồn tại.").

### 3.5. Bảng lương (`PayrollController`)

| Chức năng                | Đường dẫn                         | Quyền        |
|--------------------------|-----------------------------------|--------------|
| Danh sách kỳ lương        | `/Payroll`                        | Đã đăng nhập |
| Tạo kỳ lương (tính lương)| `/Payroll/Create`                 | Admin, Editor|
| Chi tiết kỳ lương        | `/Payroll/Details/{id}`           | Đã đăng nhập |
| Chốt kỳ lương            | `/Payroll/Close/{id}`             | Admin, Editor|
| Xem phiếu lương          | `/Payroll/Payslip/{id}`           | Đã đăng nhập |
| In phiếu lương           | `/Payroll/Payslip/{id}` (In)      | Đã đăng nhập |
| Điều chỉnh phiếu lương   | `/Payroll/EditPayslip/{id}`       | Admin, Editor|
| Xuất Excel phiếu lương   | `/Payroll/ExportExcel/{id}`       | Admin, Editor|

**Tạo kỳ lương** — nhập tháng, năm và các tỷ lệ (%): phụ cấp, thưởng, khấu trừ, thuế.
Hệ thống tự động tính cho **mỗi nhân viên đang làm việc**:

```
Phụ cấp      = Lương × Phụ cấp%       (làm tròn 0 chữ số thập phân)
Thưởng       = Lương × Thưởng%
Khấu trừ     = Lương × Khấu trừ%
Tổng gross   = Lương + Phụ cấp + Thưởng − Khấu trừ
Thuế         = Tổng gross × Thuế%
Lương thực nhận = Lương + Phụ cấp + Thưởng − Khấu trừ − Thuế
Tổng quỹ lương  = Σ Lương thực nhận của tất cả nhân viên trong kỳ
```

- Ghi chú phiếu lương lưu các tỷ lệ đã dùng, ví dụ: `Phụ cấp 10% - Khấu trừ 5% - Thuế 10%`.
- **Chống trùng kỳ:** mỗi `Tháng/Năm` chỉ tồn tại một kỳ lương
  ("Kỳ lương M/N đã tồn tại.").
- Trạng thái kỳ lương (`PayrollStatus`): **`Draft` (nháp) → `Closed` (đã chốt)**.
  - **Chốt kỳ lương:** đặt `Status = Closed`, ghi `ClosedDate`, tổng lại `TotalNetSalary`.
  - **Phiếu lương của kỳ đã chốt không thể điều chỉnh** ("Kỳ lương đã chốt, không thể điều chỉnh.").
  - Chốt lại lần 2 sẽ báo "Kỳ lương này đã được chốt trước đó.".
- **Xuất Excel** bằng ClosedXML: danh sách phiếu lương của kỳ (nhân viên, lương, phụ cấp,
  thưởng, khấu trừ, thuế, thực nhận, phòng ban).

### 3.6. Báo cáo lương (`ReportController`)

| Chức năng              | Đường dẫn                            | Quyền        |
|------------------------|--------------------------------------|--------------|
| Xem báo cáo           | `/Report?payrollPeriodId=&departmentId=` | Đã đăng nhập |
| Xuất Excel            | `/Report/ExportExcel?...`            | Admin, Editor|

- Bộ lọc: **kỳ lương** (mặc định chọn kỳ mới nhất) và **phòng ban** (mặc định "Tất cả phòng ban").
- Tổng hợp theo kỳ: số nhân viên, tổng lương cơ bản, phụ cấp, thưởng, khấu trừ, thuế, tổng thực nhận.
- Xuất Excel kèm bộ lọc hiện tại.

### 3.7. Quản lý người dùng & phân quyền (`UserController`)

| Chức năng              | Đường dẫn               | Quyền        |
|------------------------|-------------------------|--------------|
| Danh sách người dùng   | `/User?email=...`       | Đã đăng nhập |
| Chi tiết               | `/User/Details/{id}`    | Đã đăng nhập |
| Tạo tài khoản          | `/User/Create`          | **Chỉ Admin**|
| Sửa / gán vai trò      | `/User/Edit/{id}`       | **Chỉ Admin**|
| Xoá tài khoản          | `/User/Delete/{id}`       | **Chỉ Admin**|

- Vai trò hiển thị tiếng Việt ("Quản trị viên", "Biên tập viên", ...) nhưng **giá trị lưu trong DB vẫn là** `Admin` / `Editor` / `User`.
- Sửa người dùng cho phép **đổi vai trò** (gỡ vai trò cũ, gán vai trò mới).
- **Xoá tài khoản** bằng `UserManager.DeleteAsync`; **không cho xoá chính tài khoản đang đăng nhập**.
- Tìm kiếm người dùng theo email.

### 3.8. Phân quyền tổng hợp

| Vai trò  | Quyền                                                        |
|----------|--------------------------------------------------------------|
| `Admin`  | Toàn quyền: quản lý nhân viên, phòng ban, bảng lương, báo cáo, quản lý người dùng |
| `Editor` | Thêm/sửa/xoá nhân viên & phòng ban, tính lương, chốt lương, xuất Excel |
| `User`   | Chỉ xem (danh sách, chi tiết, dashboard, báo cáo)           |

- Khai báo bằng `[Authorize]` ở cấp Controller và `[Authorize(Roles = Roles.CanEditContent)]`
  (`Admin,Editor`) / `[Authorize(Roles = Roles.Admin)]` ở từng Action.
- Hiển thị menu theo quyền: mục "Quản lý người dùng" chỉ hiện với Admin; các nút
  Thêm/Sửa/Xoá chỉ hiện với Admin/Editor (người dùng `User` chỉ thấy nút xem).

### 3.9. Tính năng giao diện chung

- **Thanh điều hướng tab động:** tab đang chọn (Trang chủ, Phòng ban, Nhân viên,
  Bảng lương, Báo cáo, Người dùng) được gắn class `active` theo `controller` hiện tại —
  kể cả trang con (Create/Edit/Details) vẫn sáng đúng tab cha.
- Thông báo lỗi validation của model binding và jQuery Validate đều **tiếng Việt**
  (cấu hình `ModelBindingMessageProvider` trong `Startup.cs`).
- Định dạng ngày `dd/MM/yyyy`, tiền theo locale `vi-VN`.
- Giao diện responsive bằng Bootstrap; `site.css` gắn `asp-append-version` chống cache.

---

## 4. Các bảng dữ liệu (Database Schema)

Database tên **`MVCApplication`**. Quan hệ chính:

```
Departments 1 ──── * Employees 1 ──── * Payslips * ──── 1 PayrollPeriods
```

### 4.1. Bảng nghiệp vụ

**`Departments`** — Phòng ban

| Cột            | Kiểu         | Ràng buộc / Ý nghĩa             |
|----------------|--------------|---------------------------------|
| Id             | int          | PK, tự tăng                     |
| Code           | nvarchar(20) | NOT NULL, mã phòng ban (VD: `SL`) |
| Name           | nvarchar(100)| NOT NULL, tên phòng ban          |
| DateOfCreation | datetime2    | NOT NULL, ngày thành lập         |

**`Employees`** — Nhân viên

| Cột           | Kiểu          | Ràng buộc / Ý nghĩa                        |
|---------------|---------------|---------------------------------------------|
| Id            | int           | PK, tự tăng                                 |
| Name          | nvarchar(100) | NOT NULL, họ và tên                         |
| Age           | int           | Tuổi (nullable)                             |
| Address       | nvarchar(400) | Địa chỉ                                     |
| Salary        | decimal(18,2)| NOT NULL, lương                                |
| IsActive      | bit           | Đang làm việc                               |
| Email         | nvarchar(200) | Email (nullable)                            |
| PhoneNumber   | nvarchar(40)  | Số điện thoại (nullable)                    |
| HiringDate    | datetime2     | Ngày vào làm                                |
| IsDeleted     | bit           | Cờ **xoá mềm** (mặc định 0)                 |
| CreationDate  | datetime2     | Ngày tạo bản ghi                           |
| ImageName     | nvarchar(200) | Tên file ảnh trong `wwwroot/files/images`   |
| DepartmentId  | int           | FK → `Departments.Id`, nullable (nhân viên chưa có phòng ban) |
| PositionId    | int           | Cột dư thừa từ giai đoạn trước (mã nguồn không dùng) |

**`PayrollPeriods`** — Kỳ lương

| Cột            | Kiểu      | Ràng buộc / Ý nghĩa                            |
|----------------|-----------|-------------------------------------------------|
| Id             | int       | PK, tự tăng                                     |
| Month          | int       | Tháng của kỳ (1–12)                             |
| Year           | int       | Năm của kỳ                                      |
| Status         | int       | 0 = `Draft` (nháp), 1 = `Closed` (đã chốt)      |
| CreatedDate    | datetime2 | Ngày tính lương                                 |
| ClosedDate     | datetime2 | Ngày chốt (nullable, chỉ có khi `Status = 1`)   |
| TotalNetSalary | decimal   | Tổng quỹ lương của kỳ                           |

> Tên kỳ hiển thị dạng `Month/Year` (VD: `10/2026`) — thuộc tính `Name` (không lưu trong DB).

**`Payslips`** — Phiếu lương

| Cột             | Kiểu      | Ràng buộc / Ý nghĩa                          |
|-----------------|-----------|-----------------------------------------------|
| Id              | int       | PK, tự tăng                                   |
| PayrollPeriodId | int       | FK → `PayrollPeriods.Id`, NOT NULL            |
| EmployeeId      | int       | FK → `Employees.Id`, NOT NULL                 |
| BaseSalary      | decimal   | Lương cơ bản (nhân viên)                      |
| Allowance       | decimal   | Phụ cấp                                       |
| Bonus           | decimal   | Thưởng                                        |
| Deduction       | decimal   | Khấu trừ                                      |
| Tax             | decimal   | Thuế                                          |
| NetSalary       | (computed)| Lương thực nhận = Base + Allow + Bonus − Ded − Tax |
| Note            | nvarchar(MAX) | Ghi chú (các tỷ lệ % áp dụng)            |
| CreatedDate     | datetime2 | Ngày tính                                     |

### 4.2. Bảng hệ thống (ASP.NET Identity)

`AspNetUsers` (tài khoản), `AspNetRoles` (vai trò: Admin/Editor/User),
`AspNetUserRoles` (phân vai trò), `AspNetUserClaims`, `AspNetRoleClaims`,
`AspNetUserLogins` (đăng nhập ngoài), `AspNetUserTokens`.
Bảng `__EFMigrationsHistory` lưu lịch sử migration.

### 4.3. Dữ liệu demo hiện có

| Bảng            | Số bản ghi |
|-----------------|-----------|
| Departments     | 3 (IT, HR, Sales) |
| Employees       | 6 (5 đang làm việc, 1 đã nghỉ) |
| PayrollPeriods  | 1 (kỳ 10/2026, trạng thái nháp) |
| Payslips        | 5 (cho 5 nhân viên đang làm, tổng quỹ 99.495.000 ₫) |
| AspNetUsers     | 5 (2 Admin, 1 Editor, 2 User) |


### 4.4. Lịch sử Migration (EF Core Code First)

```
20230930185948_intialCreate
20231007155339_EmployeeModel
20231011061647_EmployeeDepartmentRelationShip
20231015145729_EmployeeImage
20231016094147_Security
20231016192254_CustmizeIdentity
20261006070821_PendingModelChangesFix
20261006093355_PayrollFeatures
```

---

## 5. Cách sử dụng project (cài đặt & chạy)

### 5.1. Yêu cầu

- **.NET SDK 10.0** (`dotnet --version`)
- **Microsoft SQL Server 2019+** (cài đặt cục bộ hoặc Docker)
- (Tùy chọn) SQL Server Management Studio / `sqlcmd` để kiểm tra dữ liệu

### 5.2. Chuẩn bị database

Ví dụ chạy SQL Server bằng Docker:

```bash
docker run -d --name mvc-sqlserver \
  -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Mvc@2026Pass" \
  -p 1433:1433 -v sqlserver-data:/var/opt/mssql \
  mcr.microsoft.com/mssql/server:2022-latest
```

### 5.3. Cấu hình kết nối

Trong `Demo.PL/appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=MVCApplication;User Id=sa;Password=Mvc@2026Pass;TrustServerCertificate=True;MultipleActiveResultSets=True"
}
```

Đổi `Server`, `User Id`, `Password` nếu môi trường khác.

### 5.4. Build & chạy

```bash
dotnet restore        # khôi phục package
dotnet build          # build solution
dotnet run --project Demo.PL   # chạy ứng dụng
```

- **Không cần tạo database thủ công**: khi khởi động, `Program.cs` tự gọi
  `Database.Migrate()` (áp dụng toàn bộ migration, tạo bảng nếu chưa có)
  và **seed 3 vai trò** `Admin`, `Editor`, `User`.
- Mở trình duyệt truy cập: **`http://localhost:5210`** (xem `Properties/launchSettings.json`).

### 5.5. Đăng nhập & tài khoản mẫu

- Truy cập `http://localhost:5210` → chuyển về `/Account/SignIn`.
- **Đăng ký tài khoản mới** tại `/Account/SignUp` — tài khoản mới tự động là `Admin`.
- Database có sẵn 5 tài khoản demo, **mật khẩu chung: `Abc@12345`**:

| Email               | Vai trò            | Quyền                                        |
|---------------------|--------------------|----------------------------------------------|
| `quynh.bui@demo.vn` | Admin              | Toàn quyền, quản lý người dùng              |
| `admin@demo.vn`     | Admin              | Toàn quyền, quản lý người dùng              |
| `tan.ngo@demo.vn`   | Editor             | CRUD nhân viên/phòng ban, bảng lương, báo cáo |
| `tung.phung@demo.vn`| User               | Chỉ xem                                     |
| `tuyen.le@demo.vn`  | User               | Chỉ xem                                     |


### 5.6. Cấu hình email (quên mật khẩu)

Điền thông tin SMTP vào mục `EmailSettings` trong `appsettings.json`:

```json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "UserName": "địa chỉ email gửi",
  "Password": "mật khẩu ứng dụng Gmail",
  "FromAddress": "địa chỉ email gửi"
}
```

### 5.7. Các lệnh thường dùng khác

```bash
# Tạo migration mới (khi sửa Model)
dotnet ef migrations add TenMigration --project Demo.DAL --startup-project Demo.PL

# Cập nhật database
dotnet ef database update --project Demo.DAL --startup-project Demo.PL

# Kiểm tra dữ liệu nhanh bằng sqlcmd (Docker)
docker exec -it mvc-sqlserver /opt/mssql-tools18/bin/sqlcmd -C -S localhost -U sa \
  -P 'Mvc@2026Pass' -d MVCApplication -Q "SELECT TOP 5 * FROM Employees"
```

---

## 6. Bảng điều hướng & quyền truy cập (tóm tắt route)

| Menu (tab)         | Controller    | Hành động                                                            |
|--------------------|---------------|----------------------------------------------------------------------|
| Trang chủ          | `Home`        | `Index` (Dashboard), `Error`                                         |
| Phòng ban          | `Department`  | `Index`, `Details` · *(thêm/sửa/xoá: Admin, Editor)*                 |
| Nhân viên          | `Employee`    | `Index` (tìm kiếm), `Details` · *(thêm/sửa/xoá: Admin, Editor)*      |
| Bảng lương         | `Payroll`     | `Index`, `Details`, `Payslip` · *(Create, Close, EditPayslip, ExportExcel: Admin, Editor)* |
| Báo cáo            | `Report`      | `Index` · *(ExportExcel: Admin, Editor)*                             |
| Người dùng         | `User`        | `Index`, `Details` · *(Create, Edit, Delete: Admin)*                 |
| —                  | `Account`     | `SignUp`, `SignIn`, `SignOut`, `ForgetPassword`, `ResetPassword`, `AccessDenied` |

- Mọi route đều theo mẫu mặc định `{controller=Home}/{action=Index}/{id?}`.
- Tất cả **POST** đều có `[ValidateAntiForgeryToken]` chống CSRF.
