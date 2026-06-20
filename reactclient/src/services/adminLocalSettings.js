const SETTINGS_KEY = 'pmi_admin_settings';

const DEFAULT_SETTINGS = {
    siteName: 'PMI Exam Simulator',
    supportEmail: 'support@pmi-simulator.com',
    maintenanceMode: false,
    allowRegistration: true,
    defaultExamDuration: 230,
    passThreshold: 80,
    notifyOnNewUser: true,
    notifyOnExamComplete: false,
};

export function getSettings() {
    try {
        const raw = localStorage.getItem(SETTINGS_KEY);
        return raw ? { ...DEFAULT_SETTINGS, ...JSON.parse(raw) } : { ...DEFAULT_SETTINGS };
    } catch {
        return { ...DEFAULT_SETTINGS };
    }
}

export function saveSettings(settings) {
    localStorage.setItem(SETTINGS_KEY, JSON.stringify(settings));
}
