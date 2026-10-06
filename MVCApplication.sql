INSERT INTO MVCApplication.dbo.AspNetRoles (Id,Name,NormalizedName,ConcurrencyStamp) VALUES
	 (N'1a753322-e493-48fe-bb16-7e77936a3e34',N'User',N'USER',N'3e59f76c-c887-414b-87b4-f6fb3aa6221a'),
	 (N'28237231-24b7-4520-8417-348ff0370601',N'Editor',N'EDITOR',N'a94d66d1-1f4a-49a9-97a0-2e5d8b1b31a9'),
	 (N'30a3acce-dacf-4c9f-9f39-61041727dfd9',N'HR',N'HR',N'52e2e372-707f-410d-a547-6380b2fd9455'),
	 (N'4846f223-1330-4a7b-a6b6-030657489a43',N'Employee',N'EMPLOYEE',N'a25c9452-7921-4a72-9fbf-776b6a6ef9fb'),
	 (N'beb07fb5-97a1-4f3b-bfb4-371094a2aa5a',N'Admin',N'ADMIN',N'13ceccf2-c836-4c64-aef6-80f9aecba74b');
INSERT INTO MVCApplication.dbo.AspNetUserRoles (UserId,RoleId) VALUES
	 (N'1b1a0543-2f37-40b5-85a9-5975ef0e15e3',N'1a753322-e493-48fe-bb16-7e77936a3e34'),
	 (N'2de6d3b8-c008-41a6-b5d0-df8fcf8a6f1e',N'1a753322-e493-48fe-bb16-7e77936a3e34'),
	 (N'50c05587-5f13-4ed8-8d20-13eee2cae11d',N'28237231-24b7-4520-8417-348ff0370601'),
	 (N'5054daec-4999-4c4b-b38b-60970a660468',N'beb07fb5-97a1-4f3b-bfb4-371094a2aa5a'),
	 (N'5f6e42c8-5593-45e4-8a54-48ad9532bb34',N'beb07fb5-97a1-4f3b-bfb4-371094a2aa5a'),
	 (N'fd0f0b96-924e-4c91-896e-8e7193cabc41',N'beb07fb5-97a1-4f3b-bfb4-371094a2aa5a');
INSERT INTO MVCApplication.dbo.AspNetUsers (Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,FName,IsAgree,LName,EmployeeId,IsLockedOut,LockedAt,LockoutReason) VALUES
	 (N'1b1a0543-2f37-40b5-85a9-5975ef0e15e3',N'tungphung',N'TUNGPHUNG',N'tung.phung@demo.vn',N'TUNG.PHUNG@DEMO.VN',0,N'AQAAAAIAAYagAAAAEERbgiF+/CZCw4hXDZ9ae9CdkGdhIrCpwLUHZusGRADi0GZzhqezzvr2DphazdgFzg==',N'4J5WU5BGKPUZ7RNL36YU4JBAMGEPCUTJ',N'577f81fa-f1cf-432e-a580-da2b577e1e7c',NULL,0,0,NULL,1,0,N'Tung',1,N'Phung',NULL,0,NULL,NULL),
	 (N'2de6d3b8-c008-41a6-b5d0-df8fcf8a6f1e',N'tuyenle',N'TUYENLE',N'tuyen.le@demo.vn',N'TUYEN.LE@DEMO.VN',0,N'AQAAAAIAAYagAAAAECQRvMzPFoWvjxUFzb98zuqJJf97qul33AvYSTofGv9YIIkeYbPmXd78pEfB6dca8Q==',N'YYKDSPC34KL6FZFU3BJGXSRFHL7WKSQ2',N'f7e6c5f2-910d-4d03-ace8-aacac1baed41',NULL,0,0,NULL,1,0,N'Tuyen',1,N'Le',NULL,0,NULL,NULL),
	 (N'5054daec-4999-4c4b-b38b-60970a660468',N'quynhbui',N'QUYNHBUI',N'quynh.bui@demo.vn',N'QUYNH.BUI@DEMO.VN',0,N'AQAAAAIAAYagAAAAENNUD9nC3OcYTmY2d0TN/DRZjkPy1JHq4DyKzHq80hopoC8t2VLXy4t1Cfqu8xA3bw==',N'S2SEO7J374LXEBK7KGMYU54IDY7JK7AX',N'6f6f681b-8628-4bf1-a0cd-320ebce5cdf4',NULL,0,0,NULL,1,0,N'Quynh',1,N'Bui',NULL,0,NULL,NULL),
	 (N'50c05587-5f13-4ed8-8d20-13eee2cae11d',N'tanngo',N'TANNGO',N'tan.ngo@demo.vn',N'TAN.NGO@DEMO.VN',0,N'AQAAAAIAAYagAAAAELYa/XIKR87zjq4EDjxG45TeXvl7FeCu74IbNqbv7L31TuYL6Z7VbPUrTQ1o2B7SDA==',N'MMFZNLPLEBSIBD2DTF5HNBXJYMLX5EZO',N'c64f7e43-8be6-4ef8-9251-e0440c80aa3a',NULL,0,0,NULL,1,0,N'Tan',1,N'Ngo',NULL,0,NULL,NULL),
	 (N'5f6e42c8-5593-45e4-8a54-48ad9532bb34',N'Tester',N'TESTER',N'test@gmail.com',N'TEST@GMAIL.COM',0,N'AQAAAAIAAYagAAAAEMlWNVH78bAsRtQpFxuI/UWzaovLErODzMVEe9XHQyU37MBVhhqQq3j93tqAjX2ZHA==',N'SLR3LLPKYFXVNVW6QMIIGDWH2TCDFSGA',N'd5ddf81b-e81e-4e09-9180-c9f433e4553f',NULL,0,0,NULL,1,0,N'Test123',1,N'Test',NULL,0,NULL,NULL),
	 (N'fd0f0b96-924e-4c91-896e-8e7193cabc41',N'admin',N'ADMIN',N'admin@demo.vn',N'ADMIN@DEMO.VN',0,N'AQAAAAIAAYagAAAAEJHmSjr5+3HIWiQX1ZJU1cynpMn1zSSNoj3NGPVYzDmkIqFqB9zNcMxkHzHBrK/Unw==',N'VX7GU6II27IQ27OPTYMUBLH5TAFWIHSA',N'8d98d029-48d8-4ab6-b3fa-7da16996abd8',NULL,0,0,NULL,1,0,N'Admin',1,N'System',NULL,0,NULL,NULL);
INSERT INTO MVCApplication.dbo.Departments (Code,Name,DateOfCreation) VALUES
	 (N'IT',N'Information Technology','2024-01-15 00:00:00.0000000'),
	 (N'HR',N'Human Resources','2024-01-15 00:00:00.0000000'),
	 (N'SL',N'Sales','2024-06-01 00:00:00.0000000'),
	 (N'123',N'Maketing','2026-10-06 00:00:00.0000000');
INSERT INTO MVCApplication.dbo.Employees (Name,Age,Address,Salary,IsActive,Email,PhoneNumber,HiringDate,IsDeleted,CreationDate,DepartmentId,ImageName,PositionId) VALUES
	 (N'Nguyễn Văn An',28,N'123 Lê Lợi, TP. Hồ Chí Minh',19000000.00,1,N'an.nguyen@demo.com',N'0901234567','2023-03-01 00:00:00.0000000',0,'2026-10-06 15:13:25.9966667',10040,NULL,NULL),
	 (N'Trần Thị Bích',26,N'45 Nguyễn Huệ, Đà Nẵng',16500000.00,1,N'bich.tran@demo.com',N'0912345678','2024-01-15 00:00:00.0000000',0,'2026-10-06 15:13:25.9966667',10050,NULL,NULL),
	 (N'Lê Minh Tuấn',32,N'78 Trần Hưng Đạo, Hà Nội',22000000.00,1,N'tuan.le@demo.com',N'0987654321','2022-07-10 00:00:00.0000000',0,'2026-10-06 15:13:25.9966667',10040,NULL,NULL),
	 (N'Phạm Thu Hà',29,N'9 Võ Văn Kiệt, Cần Thơ',18000000.00,1,N'ha.pham@demo.com',N'0923456789','2023-09-05 00:00:00.0000000',0,'2026-10-06 15:13:25.9966667',10060,NULL,NULL),
	 (N'Hoàng Văn Nam',35,N'156 Điện Biên Phủ, TP. Hồ Chí Minh',25000000.00,1,N'nam.hoang@demo.com',N'0934567890','2021-02-20 00:00:00.0000000',0,'2026-10-06 15:13:25.9966667',10060,NULL,NULL),
	 (N'Đỗ Ngọc Lan',24,N'32 Lê Duẩn, Huế',14000000.00,0,N'lan.do@demo.com',N'0945678901','2025-04-01 00:00:00.0000000',0,'2026-10-06 15:13:25.9966667',10050,NULL,NULL);
INSERT INTO MVCApplication.dbo.PayrollPeriods ([Month],[Year],Status,CreatedDate,ClosedDate,TotalNetSalary) VALUES
	 (10,2026,0,'2026-10-06 15:13:26.0000000',NULL,99495000.00);
INSERT INTO MVCApplication.dbo.Payslips (PayrollPeriodId,EmployeeId,BaseSalary,Allowance,Bonus,Deduction,Tax,Note,CreatedDate) VALUES
	 (2,1004,19000000.00,1900000.00,950000.00,950000.00,2090000.00,N'Phụ cấp 10% - Khấu trừ 5% - Thuế 10%','2026-10-06 15:13:26.0000000'),
	 (2,1005,16500000.00,1650000.00,825000.00,825000.00,1815000.00,N'Phụ cấp 10% - Khấu trừ 5% - Thuế 10%','2026-10-06 15:13:26.0000000'),
	 (2,1006,22000000.00,2200000.00,1100000.00,1100000.00,2420000.00,N'Phụ cấp 10% - Khấu trừ 5% - Thuế 10%','2026-10-06 15:13:26.0000000'),
	 (2,1007,18000000.00,1800000.00,900000.00,900000.00,1980000.00,N'Phụ cấp 10% - Khấu trừ 5% - Thuế 10%','2026-10-06 15:13:26.0000000'),
	 (2,1008,25000000.00,2500000.00,1250000.00,1250000.00,2750000.00,N'Phụ cấp 10% - Khấu trừ 5% - Thuế 10%','2026-10-06 15:13:26.0000000');
INSERT INTO MVCApplication.dbo.[__EFMigrationsHistory] (MigrationId,ProductVersion) VALUES
	 (N'20230930185948_intialCreate',N'10.0.11'),
	 (N'20231007155339_EmployeeModel',N'10.0.11'),
	 (N'20231011061647_EmployeeDepartmentRelationShip',N'10.0.11'),
	 (N'20231015145729_EmployeeImage',N'10.0.11'),
	 (N'20231016094147_Security',N'10.0.11'),
	 (N'20231016192254_CustmizeIdentity',N'10.0.11'),
	 (N'20261004123732_FixSchemaAndIndexes',N'10.0.11'),
	 (N'20261004152916_AddHrModules',N'10.0.11'),
	 (N'20261004153706_AddLockoutColumns',N'10.0.11'),
	 (N'20261006070821_PendingModelChangesFix',N'10.0.11');
INSERT INTO MVCApplication.dbo.[__EFMigrationsHistory] (MigrationId,ProductVersion) VALUES
	 (N'20261006093355_PayrollFeatures',N'10.0.11');
