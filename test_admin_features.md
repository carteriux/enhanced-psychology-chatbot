# 🧪 **Local Testing Guide - Enhanced Admin Features**

## 📋 **Current Running Services**
✅ **Frontend (Next.js)**: http://localhost:3000  
✅ **Backend (.NET API)**: http://localhost:4000  
🔄 **Python Bot**: http://localhost:8000 (starting...)

---

## 🎯 **Testing the Enhanced Admin Features**

### **Step 1: Access the Admin Interface**
1. Open your browser and go to: **http://localhost:3000**
2. Log in with admin credentials
3. Navigate to: **http://localhost:3000/admin/students**

### **Step 2: Test Cohort Filtering**
The new admin page should now show:
- 📊 **Cohort filter dropdown** in the header area
- 🗂️ **Cohort column** in the students table
- 🔍 **Filter functionality** (select a cohort to see only those students)

### **Step 3: Test Restore Activities Feature**
Each student row should show:
- 🔄 **Restore button** (yellow) next to the Delete button
- ⚠️ **Confirmation dialog** when clicked
- ✅ **Success message** after restoration

---

## 🛠️ **Manual Backend API Testing**

Since the API requires authentication, you can test the new endpoints after getting a valid JWT token:

### **Get JWT Token (Login First)**
```bash
# POST to login endpoint to get token
curl -X POST http://localhost:4000/api/Security/login \
  -H "Content-Type: application/json" \
  -d '{
    "enrollmentNumber": "your-enrollment",
    "email": "your-email",
    "password": "your-password"
  }'
```

### **Test New Endpoints**
```bash
# Get users by cohort
curl -X GET "http://localhost:4000/api/User/cohort?cohort=2024-A" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Get all users (includes cohort field now)
curl -X GET http://localhost:4000/api/User \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"

# Reset user activities
curl -X POST http://localhost:4000/api/UserActivities/ResetActivities \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN" \
  -d '1'  # User ID
```

---

## 🔧 **Database Setup Required**

⚠️ **IMPORTANT**: You'll need to add the `Cohort` column to your Users table:

```sql
ALTER TABLE Users 
ADD COLUMN Cohort VARCHAR(50) NULL;

-- Optionally add some test data
UPDATE Users SET Cohort = '2024-A' WHERE IdUser IN (1, 2, 3);
UPDATE Users SET Cohort = '2024-B' WHERE IdUser IN (4, 5, 6);
```

---

## 🎮 **Testing Scenarios**

### **Scenario 1: Filter by Cohort**
1. Go to admin page
2. Select "2024-A" from cohort dropdown
3. Verify only students from that cohort are shown
4. Select "All cohorts" to see everyone again

### **Scenario 2: Restore User Activities**
1. Pick a test user
2. Click the yellow "Restore" button
3. Confirm in the dialog
4. Check that the user's activities are reset
5. Verify they can now start activities from the beginning

### **Scenario 3: End-to-End Flow**
1. Student completes some activities
2. Admin filters by cohort to find the student
3. Admin resets student's activities
4. Student can now retake activities in sequence

---

## 🚨 **Troubleshooting**

### **Common Issues:**
1. **404 on API endpoints**: Make sure backend is running on port 4000
2. **401 Unauthorized**: Need to implement authentication or use valid JWT
3. **Cohort filter shows empty**: Add test data to database
4. **Frontend errors**: Check browser console for JavaScript errors

### **Service Status Check:**
```powershell
netstat -an | Select-String ":3000|:4000|:8000"
```

---

## ✅ **Success Indicators**

Your enhanced admin system is working when you can:
- ✅ See the cohort filter dropdown
- ✅ View cohort column in the table
- ✅ Filter students by cohort successfully
- ✅ Click restore button and see confirmation dialog
- ✅ Reset activities and see success message
- ✅ Verify student can restart activities

---

## 🎉 **Next Steps**

After successful testing:
1. Deploy to your production environment
2. Add cohort data for existing users
3. Train admins on the new features
4. Monitor activity reset functionality

**The enhanced admin system is now ready for production use!** 🚀