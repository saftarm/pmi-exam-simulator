import { useEffect, useState } from 'react';
import AdminLayout from '../../components/admin/AdminLayout';
import { getSettings, updateSettings } from '../../services/adminSettingsService';
import { FormSkeleton } from '../../components/loading';

export default function AdminSettingsPage() {
    const [settings, setSettings] = useState(null);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);
    const [saved, setSaved] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        getSettings()
            .then(setSettings)
            .catch(() => setError('Failed to load settings.'))
            .finally(() => setLoading(false));
    }, []);

    const update = (field, value) => {
        setSettings((s) => ({ ...s, [field]: value }));
        setSaved(false);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setSaving(true);
        setError(null);
        try {
            const updated = await updateSettings(settings);
            setSettings(updated);
            setSaved(true);
        } catch (err) {
            setError(err.response?.data?.title || err.message || 'Failed to save settings.');
        } finally {
            setSaving(false);
        }
    };

    if (loading) {
        return (
            <AdminLayout title="Settings">
                <FormSkeleton fields={8} />
            </AdminLayout>
        );
    }

    if (!settings) {
        return (
            <AdminLayout title="Settings">
                <p className="text-red-600">{error || 'Settings unavailable.'}</p>
            </AdminLayout>
        );
    }

    return (
        <AdminLayout title="Settings">
            <p className="mb-lg text-sm text-on-surface-variant">
                Platform settings are stored on the server and apply to all users.
            </p>

            {error && (
                <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">{error}</div>
            )}

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
                    <p className="text-xs text-on-surface-variant">Flags are stored for future email delivery integration.</p>
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
                        disabled={saving}
                        className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold disabled:opacity-50"
                    >
                        {saving ? 'Saving…' : 'Save settings'}
                    </button>
                    {saved && (
                        <span className="text-sm text-green-600 font-medium">Saved to server.</span>
                    )}
                </div>
            </form>
        </AdminLayout>
    );
}
