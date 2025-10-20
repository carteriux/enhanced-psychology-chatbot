# 🤖 bot_web

**bot_web** es una aplicación web construida con **Next.js**, diseñada como interfaz de usuario para interactuar con un backend tipo chatbot. Esta app sirve como panel de visualización y prueba para los servicios de inteligencia artificial y automatización integrados en el backend.

---

## 📝 Descripción del Proyecto

Este proyecto proporciona una experiencia de chat interactiva en tiempo real con respuestas generadas por inteligencia artificial. Se conecta a un servicio backend mediante una API REST y permite a los usuarios enviar preguntas y recibir respuestas de forma fluida.

Incluye soporte para:

- Conexión a servicios de backend configurables mediante variables de entorno
- Estilos modernos y personalizables usando Tailwind CSS
- Despliegue optimizado para plataformas como Vercel o Docker

---

## 🧪 Tecnologías Utilizadas

- **Next.js** — Framework React para SSR y SSG
- **TypeScript** — Tipado estático
- **Tailwind CSS** — Utilidades de estilo moderno
- **REST API** — Conexión a backend externo
- **.env** — Configuración de entorno

---

## 📁 Estructura del Proyecto

```
bot_web/
├── .env                     # Variables de entorno
├── public/                  # Archivos estáticos
├── src/                     # Código fuente de la app
│   ├── components/          # Componentes reutilizables
│   ├── pages/               # Rutas/páginas Next.js
│   └── styles/              # Estilos globales
├── package.json             # Dependencias y scripts
├── next.config.ts           # Configuración de Next.js
├── tailwind.config.ts       # Configuración de Tailwind
└── tsconfig.json            # Configuración TypeScript
```

---

## ⚙️ Configuración del Entorno

Antes de ejecutar la app, crea un archivo `.env` en la raíz del proyecto con la siguiente variable:

```env
NEXT_PUBLIC_BACKEND_URL="https://chatbot-back-141094916495.us-south1.run.app"
```

Puedes cambiar esta URL por la del backend local o remoto que prefieras.

---

## ▶️ Ejecutar Localmente

1. Instala dependencias:

```bash
npm install
```

2. Ejecuta en modo desarrollo:

```bash
npm run dev
```

3. Abre tu navegador en `http://localhost:3000`

---

## 📦 Scripts Disponibles

```bash
npm run dev       # Inicia el servidor de desarrollo
npm run build     # Compila la app para producción
npm run start     # Inicia la app compilada
npm run lint      # Corre el linter
```

---

## 📄 Licencia

MIT — Libre para uso personal y comercial.
