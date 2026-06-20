import { useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import { getSettings, saveSettings, logActivity } from '../../services/adminMockStore';

export default function AdminSettingsPage() {
    const [settings, setSettings] = useState(() => getSettings());
    const [saved, setSaved] = useState(false);

    const update = (field, value) => {
        setSettings((s) => ({ ...s, [field]: value }));
        setSaved(false);
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        saveSettings(settings);
        logActivity({
            user: 'Admin',
            initials: 'AD',
            action: 'Updated platform settings',
            status: 'Saved',
            statusType: 'info',
        });
        setSaved(true);
    };

    return (
        <AdminLayout title="Settings">
            <div className="mb-lg p-md bg-amber-50 border border-amber-200 rounded-lg text-sm text-amber-800">
                Settings are saved locally in your browser until a backend settings API is available.
            </div>

            <form onSubmit={handleSubmit} className="max-w-2xl space-y-lg">
                <section className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-md">
                    <h2 className="font-headline-sm text-headline-sm font-bold">General</h2>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Site name</label>
                        <input
                            value={settings.siteName}
                            onChange={(e) => update('siteName', e.target.value)}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Support email</label>
                        <input
                            type="email"
                            value={settings.supportEmail}
                            onChange={(e) => update('supportEmail', e.target.value)}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        />
                    </div>
                    <label className="flex items-center gap-md cursor-pointer">
                        <input
                            type="checkbox"
                            checked={settings.allowRegistration}
                            onChange={(e) => update('allowRegistration', e.target.checked)}
                            className="rounded border-outline-variant"
                        />
                        <span className="text-sm font-medium">Allow new user registration</span>
                    </label>
                    <label className="flex items-center gap-md cursor-pointer">
                        <input
                            type="checkbox"
                            checked={settings.maintenanceMode}
                            onChange={(e) => update('maintenanceMode', e.target.checked)}
                            className="rounded border-outline-variant"
                        />
                        <span className="text-sm font-medium">Maintenance mode</span>
                    </label>
                </section>

                <section className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-md">
                    <h2 className="font-headline-sm text-headline-sm font-bold">Exam defaults</h2>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Default exam duration (minutes)</label>
                        <input
                            type="number"
                            min={1}
                            value={settings.defaultExamDuration}
                            onChange={(e) => update('defaultExamDuration', Number(e.target.value))}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        />
                    </div>
                    <div>
                        <label className="block text-sm font-bold mb-sm">Pass threshold (%)</label>
                        <input
                            type="number"
                            min={0}
                            max={100}
                            value={settings.passThreshold}
                            onChange={(e) => update('passThreshold', Number(e.target.value))}
                            className="w-full border border-outline-variant rounded-lg px-md py-sm"
                        />
                    </div>
                </section>

                <section className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-md">
                    <h2 className="font-headline-sm text-headline-sm font-bold">Notifications</h2>
                    <label className="flex items-center gap-md cursor-pointer">
                        <input
                            type="checkbox"
                            checked={settings.notifyOnNewUser}
                            onChange={(e) => update('notifyOnNewUser', e.target.checked)}
                            className="rounded border-outline-variant"
                        />
                        <span className="text-sm font-medium">Email on new user registration</span>
                    </label>
                    <label className="flex items-center gap-md cursor-pointer">
                        <input
                            type="checkbox"
                            checked={settings.notifyOnExamComplete}
                            onChange={(e) => update('notifyOnExamComplete', e.target.checked)}
                            className="rounded border-outline-variant"
                        />
                        <span className="text-sm font-medium">Email when exam is completed</span>
                    </label>
                </section>

                <div className="flex items-center gap-md">
                    <button
                        type="submit"
                        className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold"
                    >
                        Save Settings
                    </button>
                    {saved && <span className="text-sm text-green-600 font-medium">Settings saved.</span>}
                </div>
            </form>
        </AdminLayout>
    );
}
