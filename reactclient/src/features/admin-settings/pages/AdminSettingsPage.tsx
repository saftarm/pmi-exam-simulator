import { useEffect, useState, type ChangeEvent, type FormEvent } from 'react';
import { isAxiosError } from 'axios';
import { AdminLayout } from '../../auth';
import { getSettings, updateSettings } from '../api';
import type { SiteSettingsDto } from '../types';
import { FormSkeleton } from '../../../shared/components/loading';

export default function AdminSettingsPage() {
  const [settings, setSettings] = useState<SiteSettingsDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [saved, setSaved] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    getSettings()
      .then(setSettings)
      .catch(() => setError('Failed to load settings.'))
      .finally(() => setLoading(false));
  }, []);

  const update = <K extends keyof SiteSettingsDto>(field: K, value: SiteSettingsDto[K]) => {
    setSettings((s) => (s ? { ...s, [field]: value } : s));
    setSaved(false);
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!settings) return;
    setSaving(true);
    setError(null);
    try {
      const updated = await updateSettings(settings);
      setSettings(updated);
      setSaved(true);
    } catch (err: unknown) {
      if (isAxiosError(err)) {
        const data = err.response?.data;
        const title =
          data && typeof data === 'object' && 'title' in data && typeof data.title === 'string'
            ? data.title
            : undefined;
        setError(title || err.message || 'Failed to save settings.');
      } else {
        const normalizedError = err instanceof Error ? err : new Error(String(err));
        setError(normalizedError.message || 'Failed to save settings.');
      }
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <AdminLayout title="Settings">
        <FormSkeleton fields={4} />
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
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit} className="max-w-2xl space-y-lg">
        <section className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-md">
          <h2 className="font-headline-sm text-headline-sm font-bold">General</h2>
          <div>
            <label className="block text-sm font-bold mb-sm">Site name</label>
            <input
              value={settings.siteName}
              onChange={(e: ChangeEvent<HTMLInputElement>) => update('siteName', e.target.value)}
              className="w-full border border-outline-variant rounded-lg px-md py-sm"
            />
          </div>
          <div>
            <label className="block text-sm font-bold mb-sm">Support email</label>
            <input
              type="email"
              value={settings.supportEmail}
              onChange={(e: ChangeEvent<HTMLInputElement>) => update('supportEmail', e.target.value)}
              className="w-full border border-outline-variant rounded-lg px-md py-sm"
            />
          </div>
          <label className="flex items-center gap-md cursor-pointer">
            <input
              type="checkbox"
              checked={settings.allowRegistration}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                update('allowRegistration', e.target.checked)
              }
              className="rounded border-outline-variant"
            />
            <span className="text-sm font-medium">Allow new user registration</span>
          </label>
          <label className="flex items-center gap-md cursor-pointer">
            <input
              type="checkbox"
              checked={settings.maintenanceMode}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                update('maintenanceMode', e.target.checked)
              }
              className="rounded border-outline-variant"
            />
            <span className="text-sm font-medium">Maintenance mode</span>
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
          {saved && <span className="text-sm text-green-600 font-medium">Saved to server.</span>}
        </div>
      </form>
    </AdminLayout>
  );
}
