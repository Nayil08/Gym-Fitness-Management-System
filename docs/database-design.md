# Database Design and 3NF Explanation

## Tables
1. Users
2. Gyms
3. Categories
4. Trainers
5. Items
6. Offers
7. Carts
8. CartItems
9. Orders
10. OrderItems
11. Reviews
12. Payments

## Key Relationships
- One `User` with role Admin owns zero/one `Gym`.
- One `Gym` has many `Trainers` and many `Items`.
- One `Category` has many `Items`.
- One `Item` can have many `Offers` across time.
- One Customer has one `Cart`; a Cart has many Items through `CartItems`.
- One Customer has many `Orders`.
- An Order has many Items and an Item may appear in many Orders. `OrderItems` resolves this many-to-many relationship and stores transaction-specific quantity/price/subtotal.
- One Customer may review many Items and an Item may receive many customer reviews. A unique constraint prevents duplicate review rows from the same customer for the same item.
- One paid Order has one `Payment` record.

## Why the schema is in 3NF
**First Normal Form:** Every field stores one atomic value. Repeating lists such as cart items and order items are separated into child/junction tables.

**Second Normal Form:** Non-key attributes depend on the whole key. For example, transaction attributes such as `Quantity`, `UnitPrice`, and `Subtotal` belong to `OrderItems`, not to `Items` or `Orders` alone.

**Third Normal Form:** Transitive facts are separated. `GymName` is stored only in `Gyms` and referenced by `GymId`; `CategoryName` is stored only in `Categories`; owner details are stored in `Users`. This avoids update anomalies and duplicated descriptive data.

## Data Isolation
Admin-level operational queries include the logged-in `GymId`. This enforces marketplace tenant isolation at application/query level: one gym owner cannot view or modify another gym's products, trainers, offers, inventory or sales.
