# Functional Requirements and User Stories

## Super Admin Functional Requirements
SA-FR01. The Super Admin shall log in using User ID/email and password and be routed to the Super Admin dashboard.  
SA-FR02. The Super Admin shall view all registered gym businesses and their owners.  
SA-FR03. The Super Admin shall approve pending gym businesses.  
SA-FR04. The Super Admin shall suspend a gym business and its owner account.  
SA-FR05. The Super Admin shall create and remove marketplace categories.  
SA-FR06. The Super Admin shall view platform-wide paid orders, gross sales and commission.  
SA-FR07. The Super Admin shall identify low-rated gyms using aggregated review data.  
SA-FR08. The Super Admin shall moderate abusive/inappropriate reviews by hiding them.  
SA-FR09. The Super Admin shall search/list platform data where relevant.  
SA-FR10. The Super Admin shall update profile and password information.

## Admin / Gym Owner Functional Requirements
AD-FR01. An Admin shall log in and be routed to the Gym Owner dashboard.  
AD-FR02. An Admin shall only access data for the gym mapped to the logged-in owner account.  
AD-FR03. An Admin shall update gym name, address and contact information.  
AD-FR04. An Admin shall create, read, update and delete products, services and membership packages.  
AD-FR05. An Admin shall manage trainers belonging to the gym.  
AD-FR06. An Admin shall view stock and receive a low-stock list when `StockQty <= MinStock`.  
AD-FR07. An Admin shall view sales details and net earnings after platform commission.  
AD-FR08. An Admin shall create date-bounded percentage discounts/offers.  
AD-FR09. An Admin shall view customer ratings and written reviews for its own gym.  
AD-FR10. An Admin shall update personal profile/password information.

## Customer Functional Requirements
CU-FR01. A Customer shall create an account with validated name, email and password.  
CU-FR02. A Customer shall log in and be routed to the Customer dashboard.  
CU-FR03. A Customer shall browse items from all approved gyms.  
CU-FR04. A Customer shall search by gym name, item name or keyword.  
CU-FR05. A Customer shall filter with dropdowns for category, item type and price range and may filter by location.  
CU-FR06. A Customer shall view item details, price, offer and existing reviews.  
CU-FR07. A Customer shall add items to a cart, change quantity by adding again, and remove cart lines.  
CU-FR08. A Customer shall checkout using a payment method and create a paid order.  
CU-FR09. Checkout shall reduce physical product stock while not reducing virtual service/membership availability.  
CU-FR10. A Customer shall view order history and an invoice-style order detail.  
CU-FR11. A Customer shall submit or update a 1-5 rating and written review.  
CU-FR12. A Customer shall view active discounts automatically in browse/details/cart calculations.  
CU-FR13. A Customer shall update profile/password information.

# User Stories

### US01 - Role-based login
As a registered user, I want to log in with my ID/email and password so that I am taken to the correct dashboard. The system hashes the entered password, validates it against `Users`, checks account status, reads `UserType`, and routes to Super Admin, Admin or Customer. Invalid or suspended accounts remain on the login screen with an error message.

### US02 - Customer registration
As a new customer, I want to create an account so that I can buy fitness services and products. The form validates required fields, checks that the email looks valid and requires at least six password characters. A successful registration inserts a `Customer` record with active status.

### US03 - Approve gym owner
As a Super Admin, I want to approve a pending gym so that a legitimate business can sell on the platform. I open Manage Gym Owners, select a pending gym, press Approve and the gym becomes Active. The owner account remains usable only when its status is Active.

### US04 - Suspend gym owner
As a Super Admin, I want to suspend a gym and its owner so that policy violations can be controlled. Selecting a business and pressing Suspend updates the gym and the linked owner account. Suspended owners cannot log in and suspended gyms disappear from customer browsing.

### US05 - Manage categories
As a Super Admin, I want to manage categories so that all gyms use a consistent classification. I can add a unique category name and description. Deletion is blocked by the database when a category is still referenced by an item.

### US06 - Platform commission report
As a Super Admin, I want to see gross sales and platform commission so that I can measure marketplace performance. The report aggregates paid orders with `SUM`, `COUNT` and joins. Commission is stored per order and gym rates remain visible in the business table.

### US07 - Low-rated gyms
As a Super Admin, I want to find low-rated gyms so that quality issues can be investigated. Review scores are grouped by gym and an SQL `HAVING` condition identifies averages below the threshold. Only visible reviews contribute to the result.

### US08 - Review moderation
As a Super Admin, I want to hide abusive reviews so that the public marketplace remains appropriate. The review remains in the database for audit history, but `IsVisible` is set to false. Gym owners can only view reviews and cannot perform moderation.

### US09 - Gym profile
As a Gym Owner, I want to edit my gym profile so that customers receive accurate information. The owner can update gym name, address and contact information. Every update targets the logged-in `GymId`, preventing access to another gym.

### US10 - Item CRUD
As a Gym Owner, I want to manage products, services and memberships so that my current offerings appear in the marketplace. I choose a category and type, provide price, stock thresholds and description, and save. The grid refreshes after create/update/delete operations.

### US11 - Trainer management
As a Gym Owner, I want to maintain trainer information so that customers can understand the expertise available at my gym. Trainers have name, specialization, phone, experience and availability. Trainer is a managed entity rather than a fourth system login role, preserving the required exactly-three-role model.

### US12 - Low stock
As a Gym Owner, I want to see products at or below minimum stock so that I can restock before sales are lost. The inventory dashboard filters physical products with `StockQty <= MinStock`. Services and memberships are not treated as physical stock.

### US13 - Gym sales report
As a Gym Owner, I want to see my own sales and net earnings so that I can evaluate the business. `OrderItems` is filtered by the owner gym's `GymId`. The report shows customer, item, quantity, unit price and subtotal, while net earnings subtract platform commission.

### US14 - Create offer
As a Gym Owner, I want to create a timed discount so that I can attract customers. I select one of my items, enter a percentage and start/end dates. Validation prevents invalid percentages and end dates before start dates.

### US15 - View reviews
As a Gym Owner, I want to read customer feedback so that I can improve service quality. The review list is restricted to items belonging to the current gym. The owner has no delete/hide button, separating business feedback from platform moderation.

### US16 - Browse marketplace
As a Customer, I want to browse all active gyms so that I can compare choices. The browse query only returns available items from active gyms. Gym name, category, price, stock/location, rating and current discount are displayed in a `DataGridView`.

### US17 - Search
As a Customer, I want to search by gym, item or keyword so that I can quickly find relevant fitness options. The application searches item name, gym name and description using a parameterized query. Clearing the search displays the full marketplace again.

### US18 - Multi-filter browsing
As a Customer, I want dropdown filters so that I can narrow choices without typing complex queries. Category, item type and price range are ComboBoxes, with location as an additional text filter. All selected conditions are applied together in the SQL query.

### US19 - Item details
As a Customer, I want to open an item and read details/reviews so that I can make an informed decision. The details screen shows gym, category, description, original price, active discount and final price. Existing visible customer reviews are shown below.

### US20 - Add to cart
As a Customer, I want to add an item and quantity to my cart so that I can purchase multiple choices together. The system creates one cart per customer if needed. Adding the same item again increases its quantity instead of creating a duplicate cart line.

### US21 - Remove from cart
As a Customer, I want to remove a selected cart line so that I can correct my purchase before payment. The selected `CartItemId` is deleted and the total is recalculated. The cart remains linked to only the logged-in customer.

### US22 - Checkout
As a Customer, I want to pay for my cart so that an order is created. Checkout runs inside a SQL transaction, validates physical stock, calculates discounts and platform commission, inserts `Orders`, `OrderItems` and `Payments`, reduces product stock, clears the cart, and commits only when all operations succeed.

### US23 - Order history and invoice
As a Customer, I want to view previous orders and invoice details so that I have a purchase record. The order history is filtered by the current customer. Selecting an order shows each item, gym, quantity and subtotal and calculates the total invoice amount.

### US24 - Rating and review
As a Customer, I want to rate an item from 1 to 5 and write a comment so that I can share my experience. The database has a `CHECK` constraint for the rating range and a unique customer-item rule. Submitting again updates the existing review rather than duplicating it.

### US25 - Profile and password
As any logged-in user, I want to update my profile and optionally change my password so that my account information remains current. Name is required and a new password must contain at least six characters. Passwords are stored as SHA-256 hashes rather than plain text.
