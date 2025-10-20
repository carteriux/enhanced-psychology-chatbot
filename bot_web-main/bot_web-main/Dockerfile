# Usa una imagen de Node.js como base
FROM node:18-alpine

# Configurar directorio de trabajo
WORKDIR /app

# Copiar los archivos del proyecto
COPY package.json package-lock.json ./

# Instalar dependencias
RUN npm install

# Copiar el resto del código
COPY . .

# Generar la aplicación de Next.js
RUN npm run build

# Exponer el puerto 3000
EXPOSE 3000

# Comando para iniciar el servidor Next.js
CMD ["npm", "run", "start"]
