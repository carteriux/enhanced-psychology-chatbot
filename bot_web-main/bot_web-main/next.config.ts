import type { NextConfig } from "next"

const nextConfig: NextConfig = {
    images: {
        remotePatterns: [
            {
                protocol: "https",
                hostname: "**.gravatar.com", // Any subdomain from gravatar.com
            },
            {
                protocol: "https",
                hostname: "ui-avatars.com", // Any subdomain from ui-avatars.com
            },
        ],
    },
}

export default nextConfig
