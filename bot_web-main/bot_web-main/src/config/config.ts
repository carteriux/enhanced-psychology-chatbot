export const config = {
    BACKEND_URL: process.env.NEXT_PUBLIC_BACKEND_URL || "https://enhanced-api-58720089427.us-south1.run.app",
    // JWT_SECRET: process.env.JWT_SECRET || "default_secret",
    COOKIE_SECURE: process.env.NODE_ENV === "production",
    COOKIE_MAX_AGE: 60 * 60 * 24, // 1 day
}
