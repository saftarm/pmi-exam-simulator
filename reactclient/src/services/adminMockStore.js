const USERS_KEY = 'pmi_admin_users';
const ACTIVITY_KEY = 'pmi_admin_activity';
const SETTINGS_KEY = 'pmi_admin_settings';
const ANALYTICS_KEY = 'pmi_admin_analytics';

const DEFAULT_USERS = [
    { id: '1', name: 'Marcus Knight', email: 'marcus@example.com', role: 'Learner', status: 'Active', initials: 'MK' },
    { id: '2', name: 'Sarah Jenkins', email: 'sarah@example.com', role: 'Pro', status: 'Active', initials: 'SJ' },
    { id: '3', name: 'Alex Loomis', email: 'alex@example.com', role: 'Learner', status: 'Active', initials: 'AL' },
    { id: '4', name: 'David Tan', email: 'david@example.com', role: 'Learner', status: 'Active', initials: 'DT' },
];

const DEFAULT_ACTIVITY = [
    { id: '1', user: 'Marcus Knight', initials: 'MK', action: 'Completed PMP Full Mock', timestamp: '2 mins ago', status: 'Passed', statusType: 'success' },
    { id: '2', user: 'Sarah Jenkins', initials: 'SJ', action: 'New Subscription (Pro Plan)', timestamp: '15 mins ago', status: 'Success', statusType: 'info' },
    { id: '3', user: 'Alex Loomis', initials: 'AL', action: 'Failed CAPM Section 1', timestamp: '42 mins ago', status: 'Failed', statusType: 'error' },
    { id: '4', user: 'David Tan', initials: 'DT', action: 'Account Registered', timestamp: '1 hour ago', status: 'Active', statusType: 'neutral' },
];

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

const DEFAULT_ANALYTICS = {
    passRate: 74.2,
    revenueTrend: [12, 18, 10, 22, 28, 19],
    monthlyRevenue: 42910,
    attemptsThisMonth: 3842,
    completionRate: 68.5,
};

function load(key, fallback) {
    try {
        const raw = localStorage.getItem(key);
        return raw ? JSON.parse(raw) : fallback;
    } catch {
        return fallback;
    }
}

function save(key, value) {
    localStorage.setItem(key, JSON.stringify(value));
}

export function getUsers() {
    return load(USERS_KEY, DEFAULT_USERS);
}

export function saveUsers(users) {
    save(USERS_KEY, users);
}

export function getTotalUsers() {
    return getUsers().length;
}

export function getActivity() {
    return load(ACTIVITY_KEY, DEFAULT_ACTIVITY);
}

export function logActivity(entry) {
    const activity = getActivity();
    const newEntry = {
        id: crypto.randomUUID(),
        timestamp: 'Just now',
        statusType: 'info',
        status: 'Success',
        ...entry,
    };
    save(ACTIVITY_KEY, [newEntry, ...activity].slice(0, 50));
}

export function getSettings() {
    return load(SETTINGS_KEY, DEFAULT_SETTINGS);
}

export function saveSettings(settings) {
    save(SETTINGS_KEY, settings);
}

export function getAnalytics() {
    return load(ANALYTICS_KEY, DEFAULT_ANALYTICS);
}

export function saveAnalytics(analytics) {
    save(ANALYTICS_KEY, analytics);
}

export function getAvgPassRate() {
    return getAnalytics().passRate;
}
