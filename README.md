# 🎰 Juego de la Ruleta

Proyecto que simula un juego de ruleta con frontend en **Vue 3 + Vuetify** y backend en **.NET 6 Web API** desplegable en **IIS**.

---

## ⚙️ Requisitos

### Backend

- .NET 6 SDK o superior
- SQL Server (local o remoto)
- IIS configurado con **Hosting Bundle de .NET**

### Frontend

- Node.js (v18 o superior recomendado)
- npm o yarn

---

## 🚀 Instalación y ejecución

### 🔹 Backend (API en .NET 6)

1. Entrar a la carpeta del backend:

   ```bash
   cd backend/RouletteApi
   ```

2. Restaurar dependencias:

   ```bash
   dotnet restore
   ```

3. Aplicar migraciones (si usas EF Core):

   ```bash
   dotnet ef database update
   ```

4. Ejecutar en modo desarrollo:

   ```bash
   dotnet run
   ```

   La API estará disponible en `https://localhost:5001` o `http://localhost:5000`.

#### 📦 Publicar en IIS

1. Generar publicación:

   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. Copiar la carpeta `publish/` a tu directorio configurado en IIS.
3. Asegurarte de tener configurado **Application Pool** con `.NET CLR v4.0` y habilitado **Hosting Bundle .NET 6**.

---

### 🔹 Frontend (Vue + Vuetify)

1. Entrar a la carpeta del frontend:

   ```bash
   cd frontend
   ```

2. Instalar dependencias:

   ```bash
   npm install
   ```

3. Ejecutar en desarrollo:

   ```bash
   npm run dev
   ```

   La app estará disponible en `http://localhost:5173`.

4. Construir para producción:

   ```bash
   npm run build
   ```

   El resultado quedará en `dist/`, el cual puede desplegarse en cualquier servidor web (IIS, Nginx, Apache, etc.).

---

## 📝 Notas

- Ajusta la cadena de conexión a SQL Server en `appsettings.json`.
- Si frontend y backend corren en hosts distintos, configura **CORS** en `Program.cs`.
- En IIS, puedes servir el frontend (`dist/`) desde el mismo sitio que el backend o en un sitio aparte.

---

## 👨‍💻 Autor

Proyecto desarrollado por **David Domínguez**.
