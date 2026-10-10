/// <reference types="vitest/config" />
import react from '@vitejs/plugin-react';
import { defineConfig } from 'vite';

// Under Aspire the API address is injected through service discovery variables; otherwise use RACCORD_API_URL.
const apiUrl =
  process.env.services__web__https__0 ??
  process.env.services__web__http__0 ??
  process.env.RACCORD_API_URL ??
  'http://localhost:5194';

export default defineConfig({
  plugins: [react()],
  server: {
    port: Number(process.env.PORT ?? 5173),
    proxy: {
      '/api': { target: apiUrl, changeOrigin: true, secure: false },
      '/health': { target: apiUrl, changeOrigin: true, secure: false },
    },
  },
  build: {
    outDir: 'dist',
    sourcemap: true,
  },
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: ['./src/test/setup.ts'],
    css: false,
  },
});
