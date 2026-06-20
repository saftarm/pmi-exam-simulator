import { useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import { getSettings, saveSettings } from '../../services/adminLocalSettings';

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
        setSaved(true);
    };

    return (
        <AdminLayout title="Settings">
            <div className="mb-lg p-md bg-amber-50 border border-amber-200 rounded-lg text-sm text-amber-800">
                Local preferences are stored in your browser only and are not synced to the server.
            </div>

            <div className="mb-lg p-md bg-surface-container-low border border-outline-variant rounded-lg text-sm text-on-surface-variant">
                <p className="font-bold mb-sm">Planned — needs API</p>
                <ul className="list-disc list-inside space-y-xs">
                    <li>Persisted platform settings (site name, registration toggle, maintenance mode)</li>
                    <li>Server-enforced exam defaults and pass threshold</li>
                    <li>Email notification delivery</li>
                </ul>
            </div>

            <form onSubmit={handleSubmit} className="max-w-2xl space-y-lg">
                <section className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-md">
                    <h2 className="font-headline-sm text-headline-sm font-bold">Local preferences (not synced to server)</h2>
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
                        <span className="text-sm font-medium">Allow new user registration (UI preference only)</span>
                    </label>
                    <label className="flex items-center gap-md cursor-pointer">
                        <input
                            type="checkbox"
                            checked={settings.maintenanceMode}
                            onChange={(e) => update('maintenanceMode', e.target.checked)}
                            className="rounded border-outline-variant"
                        />
                        <span className="text-sm font-medium">Maintenance mode (UI preference only)</span>
                    </label>
                </section>

                <section className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-md">
                    <h2 className="font-headline-sm text-headline-sm font-bold">Exam defaults (local)</h2>
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
                    <h2 className="font-headline-sm text-headline-sm font-bold">Notifications (local)</h2>
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
                        Save local preferences
                    </button>
                    {saved && (
                        <span className="text-sm text-green-600 font-medium">Saved in this browser.</span>
                    )}
                </div>
            </form>
        </AdminLayout>
    );
}
