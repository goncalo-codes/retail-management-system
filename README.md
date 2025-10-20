# 🏬 Retail Management System

![Retail Banner](./assets/banner02.png)

---

## 🏷️ Badges

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET Framework](https://img.shields.io/badge/.NET_Framework_4.7.2-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![Visual Studio 2022](https://img.shields.io/badge/Visual_Studio_2022-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows_Forms-0078D7?style=for-the-badge&logo=windows&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

---

## 📝 Short Description

**Retail Management System** is a Windows Forms application developed as an **academic project** that simulates the main functionalities of a real retail store management system.

It focuses on **inventory control, employee and client management, sales processing, and administrative operations**, implementing **CRUD functionalities** (Create, Read, Update, Delete) throughout its interface.

The system was designed with different user roles (**Admin, Manager, Employee, and Client**), each having distinct permissions and access levels - closely reflecting a real-world retail environment.

🧠 **Highlight:**  
This project was built to strengthen knowledge in **C# programming, SQL Server database integration, layered architecture**, and **realistic retail system simulation**.
<br><br>
> 🗒️ **Note:**  
> All code comments and database documentation inside `baseDadosLoja.txt` are written in **Portuguese**,  
> as this project was developed for academic purposes in Portugal.

---

## 🎞️ GIFs / Screenshots

**<h3>1️⃣ Login & Registration</h3>**
<p align="left">
  <img src="./assets/login.gif" width="600" alt="Login Screen">
</p>
<i>Shows the user logging in or creating an account for the first time. Supports multiple roles (Admin, Manager, Employee, Client).</i>
<br></br>

**<h3>2️⃣ Admin Dashboard & Data Management</h3>**
<p align="left">
  <img src="./assets/admin-panel.gif" width="600" alt="Admin Panel">
</p>
<i>Demonstrates the Admin panel where users can register clients, employees, products, and sales. Admins can also consult and manage all system data, including stock and transactions.</i>
<br></br>

**<h3>3️⃣ Shopping Cart & Stock Consultation</h3>**
<p align="left">
  <img src="./assets/cart.gif" width="600" alt="Cart Functionality">
</p>
<i>Shows the client browsing products, filtering results, adding items to the cart, and viewing total prices in real time.</i>

---

## 💻 Technologies

- **C#**
- **.NET Framework 4.7.2**
- **Windows Forms (GUI)**
- **SQL Server (Database)**
- **Visual Studio 2022**

---

## ⚙️ Installation & Usage

### Clone the Repository
```bash
git clone https://github.com/goncalo-codes/retail-management-system.git
```
### Open & Run

```bash
1. Open 'projetoLoja.sln' in Visual Studio 2022
2. Make sure .NET Framework 4.7.2 is installed
3. Build the solution (Build → Build Solution)
4. Run the app (Start ▶)
```

> ⚠️ **Important:** Before running the app, open `src/Data/Connection.cs` and update the connection string in the `Connection()` constructor to match your SQL Server setup:
>
> ```csharp
> conn.ConnectionString = "Data Source=localhost;Initial Catalog=lojaDB;Integrated Security=True;TrustServerCertificate=True;";
> ```
> Modify `Data Source`, `Initial Catalog`, or authentication (User ID/Password) if your SQL Server is on another machine, container, or uses SQL login.



## 🔹 Database Setup (SQL Server)

This project uses **SQL Server** as the database engine.  
All tables, relationships, and seed data are provided in the following file:

📄 **`baseDadosLoja.txt`**

<br>

### 🧭 Setup Steps:

1. Open **SQL Server Management Studio (SSMS)**  
2. Open or copy the content of the file **`baseDadosLoja.txt`**
3. Execute the script (press **F5**) — it will automatically create the database **`lojaDB`**
4. The script includes:
   - Tables for `Users`, `Clients`, `Employees`, `Categories`, `Products`, `Stock`, `Sales`, and `Sizes`
   - Example data and predefined relationships  
   - Clear comments describing each section and table purpose

---

## 🖼️ Images in Database (Optional)

The system supports **optional product images** stored as **BLOBs** in the database.  
Some sample images are already included in `src/Resources/` for reference, but they are **not linked to the database by default** (`Image` column is NULL).

⚠️ **Important:**  
- The images must be in a folder that SQL Server can **access with read permissions**.  
- Simply having them in `src/Resources/` is **not enough**, because SQL Server does not read Visual Studio resource folders automatically.

### Steps to add images to the database

1. **Create a folder accessible by SQL Server** (example: `C:\RetailImages\`) and copy your image files there.  
2. **Update each product’s `Image` column** using SQL Server `OPENROWSET`:

```sql
UPDATE Products
SET Image = (
    SELECT *
    FROM OPENROWSET(
        BULK 'C:\RetailImages\blueTshirt.png',  -- Path to your image
        SINGLE_BLOB
    ) AS ImageData
)
WHERE ProductID = 1;  -- Change the ProductID accordingly
```
<br>
---

## 🧩 System Overview

The application simulates a **real retail management environment**,  
including four different user roles and multiple **CRUD-based modules**.

<br>

### 👤 User Roles and Permissions

| **Role** | **Can Register** | **Can Consult** | **Can Update** | **Can Delete** |
|-----------|------------------|-----------------|----------------|----------------|
| **Admin** | Clients, Employees, Products, Sales | All tables | All entities | ✅ |
| **Manager** | Clients, Products, Sales | All except Employees | Limited | ✅ |
| **Employee** | Clients, Sales | Clients, Sales, Stock | Clients, Sales | ❌ |
| **Client** | N/A | Products, Cart | N/A | N/A |

<br>

### 🧠 CRUD Functionality Breakdown

The system is structured around **Create, Read, Update, Delete** operations,  
implemented throughout the `Presentation` layer.

#### 📁 Folder Overview

| **Folder** | **Purpose** | **Example Forms** |
|-------------|-------------|-------------------|
| `REGISTER/` | Create new records *(Insert)* | `frmRegisterSale`, `frmRegisterClient`, `frmRegisterProduct`, `frmRegisterEmployee`, `frmRegisterStock` |
| `CONSULT/`  | Read existing data *(View/Search/Delete)* | `frmConsultSale`, `frmConsultClient`, etc. |
| `UPDATE/`   | Update existing records *(Edit)* | `frmUpdateProduct`, `frmUpdateStock`, etc. |

**Additional standalone forms:**  
`frmLogin`, `frmCart`, `frmAdmin`, `frmManager`, `frmEmployee`, `frmClient`, `frmStock`

<br>

---

### 🗂️ Project Structure (Summary)

```text
retail-management-system/
│
├─ src/                     # Main application code
│   ├─ Control/
│   ├─ Data/
│   ├─ Presentation/
│   │   ├─ REGISTER/        # Insert clients, employees, products, sales
│   │   ├─ UPDATE/          # Edit existing records
│   │   └─ CONSULT/         # View, delete, or update records
│   ├─ Properties/
│   ├─ Resources/           # Internal images (e.g., products)
│   ├─ App.config
│   ├─ Program.cs
│   └─ projetoLoja.csproj
│
├─ assets/                  # GIFs and banner for README
│   ├─ login.gif
│   ├─ admin-panel.gif
│   └─ cart.gif
│
├─ baseDadosLoja.txt        # Database script and setup instructions
├─ projetoLoja.sln          # Visual Studio solution
├─ LICENSE                  # MIT License
└─ README.md                # This file
```
--- 

### 💡 Project Highlights

- Implemented **layered architecture** (`Data`, `Control`, `Presentation`)
- Built a fully functional **CRUD system**
- Simulated **real retail workflows** with user permissions
- Integrated **SQL Server backend** with realistic dataset
- Supported **BLOB image storage**
- Developed as an **academic project** to explore structured programming and database design

---

**Author:** Gonçalo Oliveira
