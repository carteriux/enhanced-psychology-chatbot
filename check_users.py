#!/usr/bin/env python3
"""
Script to check existing users in the CPC_Chatbot database
"""
import mysql.connector
from mysql.connector import Error

def check_database_users():
    try:
        # Database connection parameters from your config
        connection = mysql.connector.connect(
            host='34.174.222.171',
            port=3306,
            database='CPC_Chatbot',
            user='root',
            password='-4j),cn]>pAFF6L6'
        )

        if connection.is_connected():
            cursor = connection.cursor()
            
            print("🔍 Checking existing users...")
            print("=" * 50)
            
            # Check if Users table exists and get its structure
            cursor.execute("SHOW TABLES LIKE 'Users'")
            table_exists = cursor.fetchone()
            
            if not table_exists:
                print("❌ Users table doesn't exist!")
                return
                
            # Get table structure
            cursor.execute("DESCRIBE Users")
            columns = cursor.fetchall()
            print("📋 Users table structure:")
            for col in columns:
                print(f"  - {col[0]} ({col[1]})")
            print()
            
            # Check existing users
            cursor.execute("""
                SELECT IdUser, FirstName, LastName, EnrollmentNumber, Email, IsAdmin 
                FROM Users 
                LIMIT 10
            """)
            
            users = cursor.fetchall()
            
            if users:
                print("👥 Existing users (first 10):")
                print("-" * 80)
                print(f"{'ID':<5} {'Name':<20} {'Enrollment':<15} {'Email':<25} {'Admin'}")
                print("-" * 80)
                
                for user in users:
                    user_id, first_name, last_name, enrollment, email, is_admin = user
                    full_name = f"{first_name} {last_name}"
                    admin_status = "✅ Yes" if is_admin else "❌ No"
                    print(f"{user_id:<5} {full_name:<20} {enrollment:<15} {email:<25} {admin_status}")
                
                print()
                print("🔑 Try logging in with:")
                for user in users:
                    user_id, first_name, last_name, enrollment, email, is_admin = user
                    if is_admin:
                        print(f"  📧 Admin: {enrollment} or {email}")
                    else:
                        print(f"  👤 User: {enrollment} or {email}")
                        
                print("\n💡 Common passwords to try: password, admin, 123456, test")
                        
            else:
                print("⚠️  No users found in database!")
                print("You'll need to create users manually.")
                
    except Error as e:
        print(f"❌ Database connection error: {e}")
        print("\n🔧 Possible solutions:")
        print("1. Check if your IP is whitelisted in the database firewall")
        print("2. Verify the database credentials are correct")
        print("3. Ensure the database server is running")
        
    finally:
        if connection and connection.is_connected():
            cursor.close()
            connection.close()

if __name__ == "__main__":
    check_database_users()