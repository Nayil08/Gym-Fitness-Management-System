# Gym and Fitness Management System

**Course:** Object Oriented Programming 2  
**Institution:** American International University-Bangladesh (AIUB)  
**Project Type:** 3-tier Gym & Fitness marketplace management system  
**Login Roles:** Super Admin, Admin (Gym Owner), Customer

## Project Overview
Gym and Fitness Management System is a C# Windows Forms marketplace application that connects customers with multiple approved gyms and fitness centres. A Super Admin controls the platform, Gym Owners manage only their own gym data, and Customers browse fitness memberships, services and products across all approved gyms. Trainer information is managed as a business entity under each gym; Trainer is intentionally **not** a fourth login role.

The solution includes a normalized SQL Server database, working role-based login routing, gym approval, product/service/membership CRUD, trainer management, inventory alerts, sales reporting, offers, browsing/search/filtering, cart, checkout, invoice/order history, reviews and profile management.

## Exactly Three Login Roles
1. **Super Admin** - approves/suspends gym businesses, manages categories, views platform-wide sales/commission, finds low-rated gyms and moderates reviews.
2. **Admin / Gym Owner** - manages one gym only. All operational queries are isolated by `GymId`.
3. **Customer** - browses all active gyms, searches/filters items, adds to cart, checks out, views orders/invoices and writes ratings/reviews.

## Main Features
### Super Admin
- Role-based sign in
- Approve or suspend gym owners
- Manage marketplace categories
- Platform sales and commission dashboard
- Low-rated gym report using `GROUP BY` + `HAVING`
- Review moderation
- Profile and password update

### Admin / Gym Owner
- Gym profile management
- Product, service and membership CRUD
- Trainer CRUD (trainer is an entity, not a login role)
- Inventory and low-stock alert
- Sales/earnings report
- Discount/offer management
- View customer reviews
- Profile and password update
- Data isolation with logged-in `GymId`

### Customer
- Sign up and sign in
- Browse all approved gyms/items
- Search by keyword/gym/item
- Filters: category, item type, price range and location
- Details + ratings/reviews
- Cart add/remove/quantity
- Checkout/payment simulation
- Invoice and order history
- 1-5 rating + written review
- Profile/password update

## Technology Stack
- C#
- Windows Forms
- .NET Framework 4.8
- SQL Server / LocalDB
- ADO.NET (`System.Data.SqlClient`)
- Visual Studio 2022 compatible

## Folder Structure
```text
Gym_and_Fitness_Management_System/
├── GymAndFitnessManagementSystem.sln
├── GymAndFitnessManagementSystem/
│   ├── Forms/
│   ├── Data/
│   ├── Models/
│   ├── Common/
│   └── App.config
├── database/
│   └── schema.sql
├── docs/
│   ├── Project_Report.docx
│   ├── Project_Report.pdf
│   ├── diagrams/
│   └── screenshots/
└── README.md
```

## Database Design
The database contains 12 normalized tables:
`Users`, `Gyms`, `Categories`, `Trainers`, `Items`, `Offers`, `Carts`, `CartItems`, `Orders`, `OrderItems`, `Reviews`, and `Payments`.

`OrderItems` is the required junction table that resolves the many-to-many relationship between Orders and Items. `CartItems` is another junction table for Cart and Items. PK, FK, UNIQUE and CHECK constraints are included.

## SQL Requirements Covered
The SQL script includes examples for:
- Login verification
- Search
- Price filtering
- Category + availability filter
- Cart details
- Checkout/order details
- Gym earnings
- Low-stock alerts
- Rating reports
- Low-rated gyms
- Gym suspension transaction
- Active offers
- Platform commission
- Top-selling items

It uses `JOIN`, `GROUP BY`, `HAVING`, `SUM`, `AVG`, and `COUNT`.

## Demo Accounts
| Role | Login | Password |
|---|---|---|
| Super Admin | `admin@gym.com` | `admin123` |
| Gym Owner 1 | `owner1@gym.com` | `owner123` |
| Gym Owner 2 | `owner2@gym.com` | `owner123` |
| Customer | `customer1@gym.com` | `cust123` |

Passwords are stored as SHA-256 hashes in the sample database.

## Run Instructions
1. Open `GymAndFitnessManagementSystem.sln` in Visual Studio 2022 on Windows.
2. Ensure **.NET Framework 4.8 desktop development** and **SQL Server LocalDB** are installed.
3. Run the project. It attempts to create `GymFitnessDB` and execute `database/schema.sql` automatically.
4. If automatic database initialization is blocked by SQL Server policy, run `database/schema.sql` once in SQL Server Management Studio, then run the application.

## Submission Material
The `docs` folder contains the report, diagrams and 12+ UI mockups/screenshots. The project root is GitHub-ready and follows consistent lower-case/hyphenated screenshot filenames.
