# 🤖 Enhanced Psychology Chatbot System

An AI-powered chatbot system for psychology activities with enhanced admin features including cohort management and activity restoration.

## 🚀 **New Features Added**

### ✨ **Enhanced Admin System**
- **Cohort Filtering**: Filter students by cohort groups
- **Activity Restoration**: Reset student progress with one click
- **Improved User Management**: Better organized student data

### 🎯 **Key Components**

1. **Frontend (Next.js)** - `bot_web-main/`
   - React-based admin interface
   - Student activity management
   - Cohort filtering system
   - Activity restoration controls

2. **Backend (.NET API)** - `bot_backend-main/`
   - RESTful API with enhanced endpoints
   - User and activity management
   - JWT authentication
   - Database integration

3. **AI Bot (Python/Flask)** - `bot-main/`
   - LangChain integration with Google Vertex AI
   - Firestore chat history
   - PDF generation for activity reports
   - Google Cloud Storage integration

## 🛠️ **Technology Stack**

- **Frontend**: Next.js 15, React 19, TypeScript, Tailwind CSS
- **Backend**: .NET 8, C#, Entity Framework
- **AI Bot**: Python 3.13, Flask, LangChain, Google Vertex AI
- **Database**: MySQL (Google Cloud SQL)
- **Cloud**: Google Cloud Platform (Cloud Run, Firestore, Storage)

## 📊 **Database Schema Updates**

### Added to Users table:
```sql
ALTER TABLE Users ADD COLUMN Cohort VARCHAR(50) NULL;
```

## 🔧 **Environment Configuration**

### Backend (.NET)
- Database connection to Google Cloud SQL
- JWT authentication keys
- External API endpoints

### Frontend (Next.js)
- Backend API URL configuration
- Authentication cookie settings

### AI Bot (Python)
- Google Cloud project configuration
- Vertex AI settings
- Firestore and Storage bucket names

## 🚀 **Deployment**

This system is designed for Google Cloud Platform deployment:

1. **Backend**: Deploy to Cloud Run
2. **Frontend**: Deploy to Cloud Run  
3. **AI Bot**: Already deployed to Cloud Run

## 📝 **Admin Features**

### **Cohort Management**
- Filter students by cohort
- Organize users by enrollment periods
- Easy cohort assignment

### **Activity Restoration**
- Reset student progress with confirmation
- Allow students to retake activities
- Maintain activity sequence requirements

## 🔐 **Security**

- JWT-based authentication
- Role-based access control (Admin/Student)
- Secure database connections
- Environment-based configuration

## 📞 **Support**

For technical support or questions about the enhanced features, refer to the deployment and testing guides included in the project.

---

**Enhanced by AI Assistant** - October 2024
- Added cohort filtering system
- Implemented activity restoration
- Improved admin interface
- Enhanced database schema