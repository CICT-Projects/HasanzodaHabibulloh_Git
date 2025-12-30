import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    port: 3000,
    proxy: {
      // Все запросы, начинающиеся с /api, будут уходить на ваш ASP.NET
      '/api': {
        target: 'http://localhost:5016',
        secure: false,
        changeOrigin: true,
        rewrite: (path) => path
      }
    }
  }
})