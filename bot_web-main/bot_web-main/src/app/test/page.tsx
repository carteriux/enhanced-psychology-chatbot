"use client"

import { config } from "@/config/config"

export default function TestPage() {
    const testLogin = async () => {
        try {
            console.log("Backend URL:", config.BACKEND_URL)
            const response = await fetch(`${config.BACKEND_URL}/api/Security/login`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ id: "ADMIN001", password: "admin123" }),
            })
            const result = await response.json()
            console.log("Login result:", result)
            alert(`Login result: ${JSON.stringify(result, null, 2)}`)
        } catch (error) {
            console.error("Error:", error)
            alert(`Error: ${error}`)
        }
    }

    return (
        <div className="p-8">
            <h1>Test Page</h1>
            <p>Backend URL: {config.BACKEND_URL}</p>
            <button 
                onClick={testLogin}
                className="bg-blue-500 text-white px-4 py-2 rounded mt-4"
            >
                Test Login API
            </button>
        </div>
    )
}