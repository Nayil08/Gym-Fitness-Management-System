IF DB_ID('GymFitnessDB') IS NULL
BEGIN
    EXEC('CREATE DATABASE GymFitnessDB');
END
GO
USE GymFitnessDB;
GO

/* ================================================================
   GYM AND FITNESS MANAGEMENT SYSTEM
   SQL Server schema - 3NF design, exactly three login roles:
   SuperAdmin, Admin (Gym Owner), Customer.
   Trainer is a managed business entity, not a login role.
   ================================================================ */

IF OBJECT_ID('dbo.Payments','U') IS NULL
CREATE TABLE dbo.Payments(
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL UNIQUE,
    Amount DECIMAL(12,2) NOT NULL CHECK(Amount >= 0),
    Method VARCHAR(30) NOT NULL,
    ReferenceNo VARCHAR(60) NULL UNIQUE,
    PaidAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    Status VARCHAR(20) NOT NULL DEFAULT 'Paid' CHECK(Status IN('Pending','Paid','Failed','Refunded'))
);
GO

IF OBJECT_ID('dbo.Users','U') IS NULL
CREATE TABLE dbo.Users(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(120) NOT NULL,
    Email NVARCHAR(180) NOT NULL UNIQUE,
    PasswordHash CHAR(64) NOT NULL,
    Phone NVARCHAR(30) NULL,
    Address NVARCHAR(250) NULL,
    UserType VARCHAR(20) NOT NULL CHECK(UserType IN('SuperAdmin','Admin','Customer')),
    Status VARCHAR(20) NOT NULL DEFAULT 'Active' CHECK(Status IN('Active','Pending','Suspended')),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME()
);
GO

IF OBJECT_ID('dbo.Gyms','U') IS NULL
CREATE TABLE dbo.Gyms(
    GymId INT IDENTITY(1,1) PRIMARY KEY,
    OwnerUserId INT NOT NULL UNIQUE,
    GymName NVARCHAR(150) NOT NULL,
    Address NVARCHAR(250) NOT NULL,
    Contact NVARCHAR(50) NULL,
    LogoPath NVARCHAR(300) NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK(Status IN('Pending','Active','Suspended')),
    CommissionRate DECIMAL(5,4) NOT NULL DEFAULT 0.1000 CHECK(CommissionRate BETWEEN 0 AND 1),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Gyms_Users FOREIGN KEY(OwnerUserId) REFERENCES dbo.Users(UserId)
);
GO

IF OBJECT_ID('dbo.Categories','U') IS NULL
CREATE TABLE dbo.Categories(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(250) NULL
);
GO

IF OBJECT_ID('dbo.Trainers','U') IS NULL
CREATE TABLE dbo.Trainers(
    TrainerId INT IDENTITY(1,1) PRIMARY KEY,
    GymId INT NOT NULL,
    TrainerName NVARCHAR(120) NOT NULL,
    Specialization NVARCHAR(120) NULL,
    Phone NVARCHAR(30) NULL,
    ExperienceYears INT NOT NULL DEFAULT 0 CHECK(ExperienceYears >= 0),
    Availability NVARCHAR(120) NULL,
    CONSTRAINT FK_Trainers_Gyms FOREIGN KEY(GymId) REFERENCES dbo.Gyms(GymId)
);
GO

IF OBJECT_ID('dbo.Items','U') IS NULL
CREATE TABLE dbo.Items(
    ItemId INT IDENTITY(1,1) PRIMARY KEY,
    GymId INT NOT NULL,
    CategoryId INT NOT NULL,
    ItemName NVARCHAR(150) NOT NULL,
    ItemType VARCHAR(20) NOT NULL CHECK(ItemType IN('Product','Service','Membership')),
    Price DECIMAL(12,2) NOT NULL CHECK(Price >= 0),
    StockQty INT NOT NULL DEFAULT 0 CHECK(StockQty >= 0),
    MinStock INT NOT NULL DEFAULT 0 CHECK(MinStock >= 0),
    Description NVARCHAR(500) NULL,
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Items_Gyms FOREIGN KEY(GymId) REFERENCES dbo.Gyms(GymId),
    CONSTRAINT FK_Items_Categories FOREIGN KEY(CategoryId) REFERENCES dbo.Categories(CategoryId)
);
GO

IF OBJECT_ID('dbo.Offers','U') IS NULL
CREATE TABLE dbo.Offers(
    OfferId INT IDENTITY(1,1) PRIMARY KEY,
    ItemId INT NOT NULL,
    DiscountPercent DECIMAL(5,2) NOT NULL CHECK(DiscountPercent > 0 AND DiscountPercent < 100),
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT CK_Offers_Dates CHECK(EndDate >= StartDate),
    CONSTRAINT FK_Offers_Items FOREIGN KEY(ItemId) REFERENCES dbo.Items(ItemId)
);
GO

IF OBJECT_ID('dbo.Carts','U') IS NULL
CREATE TABLE dbo.Carts(
    CartId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerUserId INT NOT NULL UNIQUE,
    UpdatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    CONSTRAINT FK_Carts_Users FOREIGN KEY(CustomerUserId) REFERENCES dbo.Users(UserId)
);
GO

IF OBJECT_ID('dbo.CartItems','U') IS NULL
CREATE TABLE dbo.CartItems(
    CartItemId INT IDENTITY(1,1) PRIMARY KEY,
    CartId INT NOT NULL,
    ItemId INT NOT NULL,
    Quantity INT NOT NULL CHECK(Quantity > 0),
    CONSTRAINT UQ_CartItems UNIQUE(CartId,ItemId),
    CONSTRAINT FK_CartItems_Carts FOREIGN KEY(CartId) REFERENCES dbo.Carts(CartId) ON DELETE CASCADE,
    CONSTRAINT FK_CartItems_Items FOREIGN KEY(ItemId) REFERENCES dbo.Items(ItemId)
);
GO

IF OBJECT_ID('dbo.Orders','U') IS NULL
CREATE TABLE dbo.Orders(
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerUserId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    TotalAmount DECIMAL(12,2) NOT NULL CHECK(TotalAmount >= 0),
    PlatformCommission DECIMAL(12,2) NOT NULL DEFAULT 0 CHECK(PlatformCommission >= 0),
    PaymentMethod VARCHAR(30) NOT NULL,
    PaymentStatus VARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK(PaymentStatus IN('Pending','Paid','Failed','Refunded')),
    OrderStatus VARCHAR(20) NOT NULL DEFAULT 'Pending' CHECK(OrderStatus IN('Pending','Confirmed','Completed','Cancelled')),
    CONSTRAINT FK_Orders_Users FOREIGN KEY(CustomerUserId) REFERENCES dbo.Users(UserId)
);
GO

IF OBJECT_ID('dbo.OrderItems','U') IS NULL
CREATE TABLE dbo.OrderItems(
    OrderItemId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ItemId INT NOT NULL,
    GymId INT NOT NULL,
    Quantity INT NOT NULL CHECK(Quantity > 0),
    UnitPrice DECIMAL(12,2) NOT NULL CHECK(UnitPrice >= 0),
    DiscountAmount DECIMAL(12,2) NOT NULL DEFAULT 0 CHECK(DiscountAmount >= 0),
    Subtotal DECIMAL(12,2) NOT NULL CHECK(Subtotal >= 0),
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY(OrderId) REFERENCES dbo.Orders(OrderId),
    CONSTRAINT FK_OrderItems_Items FOREIGN KEY(ItemId) REFERENCES dbo.Items(ItemId),
    CONSTRAINT FK_OrderItems_Gyms FOREIGN KEY(GymId) REFERENCES dbo.Gyms(GymId)
);
GO

IF OBJECT_ID('dbo.Reviews','U') IS NULL
CREATE TABLE dbo.Reviews(
    ReviewId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerUserId INT NOT NULL,
    ItemId INT NOT NULL,
    Rating INT NOT NULL CHECK(Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(600) NULL,
    ReviewDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    IsVisible BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Reviews UNIQUE(CustomerUserId,ItemId),
    CONSTRAINT FK_Reviews_Users FOREIGN KEY(CustomerUserId) REFERENCES dbo.Users(UserId),
    CONSTRAINT FK_Reviews_Items FOREIGN KEY(ItemId) REFERENCES dbo.Items(ItemId)
);
GO

-- Add payment FK only after Orders exists.
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name='FK_Payments_Orders')
ALTER TABLE dbo.Payments ADD CONSTRAINT FK_Payments_Orders FOREIGN KEY(OrderId) REFERENCES dbo.Orders(OrderId);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Items_Gym_Category' AND object_id=OBJECT_ID('dbo.Items'))
CREATE INDEX IX_Items_Gym_Category ON dbo.Items(GymId,CategoryId,ItemType);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Orders_Customer_Date' AND object_id=OBJECT_ID('dbo.Orders'))
CREATE INDEX IX_Orders_Customer_Date ON dbo.Orders(CustomerUserId,OrderDate DESC);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Reviews_Item' AND object_id=OBJECT_ID('dbo.Reviews'))
CREATE INDEX IX_Reviews_Item ON dbo.Reviews(ItemId,Rating);
GO

/* -------------------- Seed Users -------------------- */
IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email='admin@gym.com')
INSERT INTO dbo.Users(FullName,Email,PasswordHash,Phone,Address,UserType,Status)
VALUES('Platform Super Admin','admin@gym.com','240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9','01700000000','Dhaka','SuperAdmin','Active');

IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email='owner1@gym.com')
INSERT INTO dbo.Users(FullName,Email,PasswordHash,Phone,Address,UserType,Status)
VALUES('PowerFit Owner','owner1@gym.com','43a0d17178a9d26c9e0fe9a74b0b45e38d32f27aed887a008a54bf6e033bf7b9','01711111111','Dhanmondi, Dhaka','Admin','Active');

IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email='owner2@gym.com')
INSERT INTO dbo.Users(FullName,Email,PasswordHash,Phone,Address,UserType,Status)
VALUES('Iron Paradise Owner','owner2@gym.com','43a0d17178a9d26c9e0fe9a74b0b45e38d32f27aed887a008a54bf6e033bf7b9','01722222222','Banani, Dhaka','Admin','Active');

IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email='customer1@gym.com')
INSERT INTO dbo.Users(FullName,Email,PasswordHash,Phone,Address,UserType,Status)
VALUES('Customer One','customer1@gym.com','4f21b18a4c743a5da01bb3a4955dea0a0294a0b4f7977b454c7259e37b2e6c19','01811111111','Mirpur, Dhaka','Customer','Active');

IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email='customer2@gym.com')
INSERT INTO dbo.Users(FullName,Email,PasswordHash,Phone,Address,UserType,Status)
VALUES('Customer Two','customer2@gym.com','4f21b18a4c743a5da01bb3a4955dea0a0294a0b4f7977b454c7259e37b2e6c19','01822222222','Uttara, Dhaka','Customer','Active');

IF NOT EXISTS(SELECT 1 FROM dbo.Users WHERE Email='customer3@gym.com')
INSERT INTO dbo.Users(FullName,Email,PasswordHash,Phone,Address,UserType,Status)
VALUES('Customer Three','customer3@gym.com','4f21b18a4c743a5da01bb3a4955dea0a0294a0b4f7977b454c7259e37b2e6c19','01833333333','Mohakhali, Dhaka','Customer','Active');
GO

/* -------------------- Seed Gyms -------------------- */
DECLARE @owner1 INT=(SELECT UserId FROM dbo.Users WHERE Email='owner1@gym.com');
DECLARE @owner2 INT=(SELECT UserId FROM dbo.Users WHERE Email='owner2@gym.com');
IF NOT EXISTS(SELECT 1 FROM dbo.Gyms WHERE OwnerUserId=@owner1)
INSERT INTO dbo.Gyms(OwnerUserId,GymName,Address,Contact,Status,CommissionRate) VALUES(@owner1,'PowerFit Gym','Dhanmondi, Dhaka','02-55000001','Active',0.10);
IF NOT EXISTS(SELECT 1 FROM dbo.Gyms WHERE OwnerUserId=@owner2)
INSERT INTO dbo.Gyms(OwnerUserId,GymName,Address,Contact,Status,CommissionRate) VALUES(@owner2,'Iron Paradise','Banani, Dhaka','02-55000002','Active',0.10);
GO

/* -------------------- Seed Categories -------------------- */
IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE CategoryName='Membership') INSERT INTO dbo.Categories(CategoryName,Description) VALUES('Membership','Gym membership packages');
IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE CategoryName='Personal Training') INSERT INTO dbo.Categories(CategoryName,Description) VALUES('Personal Training','One-to-one training services');
IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE CategoryName='Group Classes') INSERT INTO dbo.Categories(CategoryName,Description) VALUES('Group Classes','Yoga, HIIT and fitness classes');
IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE CategoryName='Supplements') INSERT INTO dbo.Categories(CategoryName,Description) VALUES('Supplements','Nutrition and supplement products');
IF NOT EXISTS(SELECT 1 FROM dbo.Categories WHERE CategoryName='Accessories') INSERT INTO dbo.Categories(CategoryName,Description) VALUES('Accessories','Gym and fitness accessories');
GO

/* -------------------- Seed Trainers -------------------- */
DECLARE @g1 INT=(SELECT GymId FROM dbo.Gyms WHERE GymName='PowerFit Gym');
DECLARE @g2 INT=(SELECT GymId FROM dbo.Gyms WHERE GymName='Iron Paradise');
IF NOT EXISTS(SELECT 1 FROM dbo.Trainers WHERE GymId=@g1 AND TrainerName='Rahim Khan') INSERT INTO dbo.Trainers(GymId,TrainerName,Specialization,Phone,ExperienceYears,Availability) VALUES(@g1,'Rahim Khan','Strength & Conditioning','01910000001',5,'Sun-Thu 5PM-10PM');
IF NOT EXISTS(SELECT 1 FROM dbo.Trainers WHERE GymId=@g1 AND TrainerName='Nabila Islam') INSERT INTO dbo.Trainers(GymId,TrainerName,Specialization,Phone,ExperienceYears,Availability) VALUES(@g1,'Nabila Islam','Yoga & Mobility','01910000002',4,'Sat-Wed 7AM-12PM');
IF NOT EXISTS(SELECT 1 FROM dbo.Trainers WHERE GymId=@g2 AND TrainerName='Arif Hasan') INSERT INTO dbo.Trainers(GymId,TrainerName,Specialization,Phone,ExperienceYears,Availability) VALUES(@g2,'Arif Hasan','Bodybuilding','01910000003',7,'Daily 4PM-9PM');
GO

/* -------------------- Seed Items -------------------- */
DECLARE @g1i INT=(SELECT GymId FROM dbo.Gyms WHERE GymName='PowerFit Gym');
DECLARE @g2i INT=(SELECT GymId FROM dbo.Gyms WHERE GymName='Iron Paradise');
DECLARE @cm INT=(SELECT CategoryId FROM dbo.Categories WHERE CategoryName='Membership');
DECLARE @cp INT=(SELECT CategoryId FROM dbo.Categories WHERE CategoryName='Personal Training');
DECLARE @cg INT=(SELECT CategoryId FROM dbo.Categories WHERE CategoryName='Group Classes');
DECLARE @cs INT=(SELECT CategoryId FROM dbo.Categories WHERE CategoryName='Supplements');
DECLARE @ca INT=(SELECT CategoryId FROM dbo.Categories WHERE CategoryName='Accessories');
IF NOT EXISTS(SELECT 1 FROM dbo.Items WHERE GymId=@g1i AND ItemName='Monthly Membership') INSERT INTO dbo.Items(GymId,CategoryId,ItemName,ItemType,Price,StockQty,MinStock,Description) VALUES(@g1i,@cm,'Monthly Membership','Membership',2500,9999,0,'Full gym access for one month.');
IF NOT EXISTS(SELECT 1 FROM dbo.Items WHERE GymId=@g1i AND ItemName='Personal Training Session') INSERT INTO dbo.Items(GymId,CategoryId,ItemName,ItemType,Price,StockQty,MinStock,Description) VALUES(@g1i,@cp,'Personal Training Session','Service',1200,9999,0,'One 60-minute personal training session.');
IF NOT EXISTS(SELECT 1 FROM dbo.Items WHERE GymId=@g1i AND ItemName='Whey Protein 1kg') INSERT INTO dbo.Items(GymId,CategoryId,ItemName,ItemType,Price,StockQty,MinStock,Description) VALUES(@g1i,@cs,'Whey Protein 1kg','Product',4200,4,5,'Protein supplement for post-workout nutrition.');
IF NOT EXISTS(SELECT 1 FROM dbo.Items WHERE GymId=@g2i AND ItemName='Yearly Membership') INSERT INTO dbo.Items(GymId,CategoryId,ItemName,ItemType,Price,StockQty,MinStock,Description) VALUES(@g2i,@cm,'Yearly Membership','Membership',24000,9999,0,'Twelve-month access with free fitness assessment.');
IF NOT EXISTS(SELECT 1 FROM dbo.Items WHERE GymId=@g2i AND ItemName='HIIT Group Class') INSERT INTO dbo.Items(GymId,CategoryId,ItemName,ItemType,Price,StockQty,MinStock,Description) VALUES(@g2i,@cg,'HIIT Group Class','Service',700,9999,0,'High-intensity interval training group class.');
IF NOT EXISTS(SELECT 1 FROM dbo.Items WHERE GymId=@g2i AND ItemName='Training Gloves') INSERT INTO dbo.Items(GymId,CategoryId,ItemName,ItemType,Price,StockQty,MinStock,Description) VALUES(@g2i,@ca,'Training Gloves','Product',950,12,4,'Breathable gloves with wrist support.');
GO

/* -------------------- Seed Offers -------------------- */
DECLARE @monthly INT=(SELECT ItemId FROM dbo.Items WHERE ItemName='Monthly Membership' AND GymId=(SELECT GymId FROM dbo.Gyms WHERE GymName='PowerFit Gym'));
DECLARE @gloves INT=(SELECT ItemId FROM dbo.Items WHERE ItemName='Training Gloves' AND GymId=(SELECT GymId FROM dbo.Gyms WHERE GymName='Iron Paradise'));
IF NOT EXISTS(SELECT 1 FROM dbo.Offers WHERE ItemId=@monthly) INSERT INTO dbo.Offers(ItemId,DiscountPercent,StartDate,EndDate,IsActive) VALUES(@monthly,10,DATEADD(day,-5,SYSDATETIME()),DATEADD(day,30,SYSDATETIME()),1);
IF NOT EXISTS(SELECT 1 FROM dbo.Offers WHERE ItemId=@gloves) INSERT INTO dbo.Offers(ItemId,DiscountPercent,StartDate,EndDate,IsActive) VALUES(@gloves,15,DATEADD(day,-2,SYSDATETIME()),DATEADD(day,20,SYSDATETIME()),1);
GO

/* -------------------- Seed Review -------------------- */
DECLARE @cust INT=(SELECT UserId FROM dbo.Users WHERE Email='customer1@gym.com');
DECLARE @monthly2 INT=(SELECT ItemId FROM dbo.Items WHERE ItemName='Monthly Membership' AND GymId=(SELECT GymId FROM dbo.Gyms WHERE GymName='PowerFit Gym'));
IF NOT EXISTS(SELECT 1 FROM dbo.Reviews WHERE CustomerUserId=@cust AND ItemId=@monthly2)
INSERT INTO dbo.Reviews(CustomerUserId,ItemId,Rating,Comment) VALUES(@cust,@monthly2,5,'Clean gym, useful equipment and friendly trainers.');
GO

/* ================================================================
   REQUIRED / DEMONSTRATION QUERIES
   ================================================================ */

-- Q1. Login verification (application passes @id and SHA-256 @passwordHash)
-- SELECT UserId,FullName,UserType,Status FROM Users
-- WHERE (Email=@id OR CAST(UserId AS varchar(20))=@id) AND PasswordHash=@passwordHash AND Status='Active';

-- Q2. Search by gym, item or keyword
SELECT i.ItemId,g.GymName,i.ItemName,i.ItemType,i.Price
FROM dbo.Items i JOIN dbo.Gyms g ON g.GymId=i.GymId
WHERE i.ItemName LIKE '%membership%' OR g.GymName LIKE '%membership%' OR i.Description LIKE '%membership%';
GO

-- Q3. Price filter
SELECT ItemId,ItemName,Price FROM dbo.Items WHERE Price BETWEEN 1000 AND 3000 ORDER BY Price;
GO

-- Q4. Category + availability filter using JOIN
SELECT i.ItemName,c.CategoryName,g.GymName,i.Price
FROM dbo.Items i
JOIN dbo.Categories c ON c.CategoryId=i.CategoryId
JOIN dbo.Gyms g ON g.GymId=i.GymId
WHERE c.CategoryName='Membership' AND i.IsAvailable=1 AND g.Status='Active';
GO

-- Q5. Cart detail with calculated line total
SELECT u.FullName,i.ItemName,ci.Quantity,i.Price,(ci.Quantity*i.Price) AS LineTotal
FROM dbo.Carts ca
JOIN dbo.Users u ON u.UserId=ca.CustomerUserId
JOIN dbo.CartItems ci ON ci.CartId=ca.CartId
JOIN dbo.Items i ON i.ItemId=ci.ItemId;
GO

-- Q6. Checkout/order detail - Junction table demonstration
SELECT o.OrderId,u.FullName Customer,g.GymName,i.ItemName,oi.Quantity,oi.UnitPrice,oi.Subtotal
FROM dbo.Orders o
JOIN dbo.Users u ON u.UserId=o.CustomerUserId
JOIN dbo.OrderItems oi ON oi.OrderId=o.OrderId
JOIN dbo.Items i ON i.ItemId=oi.ItemId
JOIN dbo.Gyms g ON g.GymId=oi.GymId
ORDER BY o.OrderDate DESC;
GO

-- Q7. Gym earnings using GROUP BY and aggregate SUM
SELECT g.GymId,g.GymName,
       SUM(oi.Subtotal) GrossSales,
       SUM(oi.Subtotal*g.CommissionRate) PlatformCommission,
       SUM(oi.Subtotal*(1-g.CommissionRate)) GymEarnings
FROM dbo.Gyms g
JOIN dbo.OrderItems oi ON oi.GymId=g.GymId
JOIN dbo.Orders o ON o.OrderId=oi.OrderId AND o.PaymentStatus='Paid'
GROUP BY g.GymId,g.GymName,g.CommissionRate;
GO

-- Q8. Low stock alert
SELECT g.GymName,i.ItemName,i.StockQty,i.MinStock
FROM dbo.Items i JOIN dbo.Gyms g ON g.GymId=i.GymId
WHERE i.ItemType='Product' AND i.StockQty<=i.MinStock;
GO

-- Q9. Review report with average rating
SELECT g.GymName,COUNT(r.ReviewId) ReviewCount,AVG(CAST(r.Rating AS DECIMAL(5,2))) AverageRating
FROM dbo.Gyms g
JOIN dbo.Items i ON i.GymId=g.GymId
JOIN dbo.Reviews r ON r.ItemId=i.ItemId AND r.IsVisible=1
GROUP BY g.GymName;
GO

-- Q10. Low-rated gyms using GROUP BY + HAVING
SELECT g.GymName,AVG(CAST(r.Rating AS DECIMAL(5,2))) AverageRating
FROM dbo.Gyms g
JOIN dbo.Items i ON i.GymId=g.GymId
JOIN dbo.Reviews r ON r.ItemId=i.ItemId AND r.IsVisible=1
GROUP BY g.GymName
HAVING AVG(CAST(r.Rating AS DECIMAL(5,2))) < 3.5;
GO

-- Q11. Suspend a gym owner and gym (example transaction)
-- BEGIN TRAN;
-- UPDATE Gyms SET Status='Suspended' WHERE GymId=@GymId;
-- UPDATE Users SET Status='Suspended' WHERE UserId=(SELECT OwnerUserId FROM Gyms WHERE GymId=@GymId);
-- COMMIT;

-- Q12. Active offers
SELECT g.GymName,i.ItemName,o.DiscountPercent,o.StartDate,o.EndDate
FROM dbo.Offers o
JOIN dbo.Items i ON i.ItemId=o.ItemId
JOIN dbo.Gyms g ON g.GymId=i.GymId
WHERE o.IsActive=1 AND SYSDATETIME() BETWEEN o.StartDate AND o.EndDate;
GO

-- Q13. Platform commission summary
SELECT COUNT(*) PaidOrders,SUM(TotalAmount) GrossSales,SUM(PlatformCommission) PlatformCommission
FROM dbo.Orders WHERE PaymentStatus='Paid';
GO

-- Q14. Top-selling items using GROUP BY / HAVING / COUNT / SUM
SELECT i.ItemName,SUM(oi.Quantity) UnitsSold,SUM(oi.Subtotal) Sales
FROM dbo.OrderItems oi JOIN dbo.Items i ON i.ItemId=oi.ItemId
GROUP BY i.ItemName
HAVING SUM(oi.Quantity) >= 1
ORDER BY UnitsSold DESC;
GO
