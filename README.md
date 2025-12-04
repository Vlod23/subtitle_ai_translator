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

👤 User authentication with ASP.NET Identity or Google OAuth

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
```
git clone https://github.com/<your-username>/<your-repo>
```
### 2️⃣ Open in Visual Studio 2022+
### 3️⃣ Restore NuGet packages
```
  Visual Studio → Build → Restore NuGet Packages
```
### 4️⃣ Configure User Secrets

In order to use all app features, you need to create your own accounts with all of the used services (OpenAi, Stripe, AWS, Google).
To make the translations work, OpenAI and AWS (mailing) is mandatory. You can avoid Stripe by manually adding credits in the DB and to avoid using Google login, just create you account without Google.

After creating your accounts and generating API keys and other secrets, right-click your Web project → Manage User Secrets.

Paste the following into secrets.json and fill the demo values with the real ones:
```
{
  "OpenAI:ApiKey": "YOUR_OPENAI_KEY",

  "Email:Username": "YOUR_AWS_SES_USERNAME",
  "Email:Password": "YOUR_AWS_SES_PASSWORD",
  "Email:FromEmail": "sender@example.com",
  "Email:FromName": "AI Subtitle Translator",

  "Stripe:SecretKey": "YOUR_STRIPE_SECRET",
  "Stripe:WebhookSecret": "YOUR_STRIPE_WEBHOOK",

  "Authentication:Google:ClientId": "YOUR_GOOGLE_ID",
  "Authentication:Google:ClientSecret": "YOUR_GOOGLE_SECRET"
}
```
(These values must NOT be stored in appsettings.json and should never be committed to GitHub.)

### 5️⃣ Create the database
```
dotnet ef database update
```

--- 

## 🛡 License

This project is licensed under the MIT License.
You are free to use, modify, and distribute it.

---

## 🤝 Credits

Developed by Vladimir Kivader, Slovakia.

If you like this project, feel free to ⭐ star the repo!


