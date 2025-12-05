# 🎬 AI Subtitle Translator

AI Subtitle Translator is a modern **.NET 9** web application that allows users to translate `.srt` subtitle files into any language using **OpenAI GPT models**.

It supports large subtitle files (with automated chunking), background processing via Hangfire, Stripe payments, a credits system, email notifications, public gallery of downloadable translations, and user auth built on ASP.NET Identity.

---

## 🚀 Features

📤 Upload `.srt` subtitle files

🤖 Translation using OpenAI GPT (with chunking for long files)

🌐 Automatic source language detection

👄 Wide selection of target languages including dialects
 
✎ Possible editing of the translated result
 
🔒 Option of making translations publicly (un)available for other users

🌍 Public gallery of translated subtitles

🔧 Background processing of the translation using Hangfire

📨 Email notifications via AWS SES

💳 Credit system (AI tokens ↔ credits ↔ EUR)

🧾 Stripe Checkout integration for purchasing credits

📄 PDF invoice generation (QuestPDF)

👤 User authentication with ASP.NET Identity

🛠️ Admin interface with user management (roles and permissions, locking users, generating their invoices)

---

## 🧰 Tech Stack

- **.NET 9 (ASP.NET Core MVC)**
- **Entity Framework Core 9**
- **Clean Architecture**
- **OpenAI GPT API**
- **Hangfire** (SQL storage)
- **Stripe .NET SDK**
- **AWS SES (SMTP)**
- **ASP.NET Identity**
- **QuestPDF**
- **SQL Server (LocalDB)**

---

## ⚙️ Development Setup (Run Locally)

### 1️⃣ Clone the repository
### 2️⃣ Open in Visual Studio 2022+
### 3️⃣ Restore NuGet packages
VS should do this automatically. If not, open cmd/powershell/terminal in the app main directory and use:
```
  Visual Studio → Build → Restore NuGet Packages
```
### 4️⃣ Configure User Secrets

- To make only the translations work, OpenAI API credentials are mandatory (OpenAI:ApiKey). 
- To make mailing work, set up your AWS SES mailing
- To make payments work, setup your Stripe account (test environment)
- To make Google auth work, setup your Google account (https://console.cloud.google.com/apis/credentials)

After creating your accounts and generating API keys and other secrets, right-click your Web project → Manage User Secrets.

Paste the following into secrets.json and fill the values with the real ones:
```
{
  // AI Translations
  "OpenAI:ApiKey": "YOUR_OPENAI_KEY",

  // Mailing
  "Email:Username": "YOUR_AWS_SES_USERNAME",
  "Email:Password": "YOUR_AWS_SES_PASSWORD",
  "Email:FromEmail": "sender@example.com",
  "Email:FromName": "AI Subtitle Translator",

  // Demo Payments
  "Stripe:SecretKey": "YOUR_STRIPE_SECRET",
  "Stripe:WebhookSecret": "YOUR_STRIPE_WEBHOOK",

  // Signing up with Google
  "Authentication:Google:ClientId": "YOUR_GOOGLE_ID",
  "Authentication:Google:ClientSecret": "YOUR_GOOGLE_SECRET"

  // !!! Before starting the app, set up your default admin email and password in order to gain access to Admin interface
  "DefaultAdmin:Email": "YOUR_REAL_EMAIL",
  "DefaultAdmin:Password": "CREATE_PASSWORD" (requires uppercase, lowercase, number, and a special char)
}
```

### 5️⃣ Create the database
Open cmd/powershell/terminal in the app directory and use:
```
dotnet ef database update
```

--- 

## 🛡 License

This project is licensed under the CC BY-NC 4.0 License.
You may use, modify, and distribute this software for non-commercial purposes only.
Commercial use is prohibited without explicit permission.

---

## 🤝 Credits

Developed by Vladimir Kivader, Slovakia.

If you like this project, feel free to ⭐ star the repo!
Also, your feedback is highly appreciated.


