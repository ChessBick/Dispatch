import type { CapacitorConfig } from '@capacitor/cli'

const config: CapacitorConfig = {
  appId: 'com.dispatch.mobile',
  appName: 'Dispatch',
  webDir: 'dist',
  server: {
    androidScheme: 'https',
    cleartext: true
  },
}

export default config