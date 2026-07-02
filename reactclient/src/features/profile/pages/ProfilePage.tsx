import { useEffect, useState, type ChangeEvent, type FormEvent } from 'react';
import { isAxiosError } from 'axios';
import { Link } from 'react-router-dom';
import { formatApiErrors } from '../../../shared/api/errors';
import { fetchCurrentUser, updateProfile } from '../api';
import type { ProfileUser } from '../types';
import { useAuth } from '../../auth';
import StatusBadge from '../../../shared/components/StatusBadge';
import { formatDate, getInitials } from '../../../shared/utils/userDisplay';
import { ProfileSkeleton, ContentReveal, LoadingButton } from '../../../shared/components/loading';

export default function ProfilePage() {
  const { refreshUser } = useAuth();
  const [profile, setProfile] = useState<ProfileUser | null>(null);
  const [form, setForm] = useState({ displayName: '', firstName: '', email: '' });
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [saved, setSaved] = useState(false);

  useEffect(() => {
    fetchCurrentUser()
      .then((data) => {
        setProfile(data);
        setForm({
          displayName: data.displayName || '',
          firstName: data.firstName || '',
          email: data.email || '',
        });
      })
      .catch(() => setError('Failed to load profile.'))
      .finally(() => setLoading(false));
  }, []);

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setSaving(true);
    setError(null);
    setSaved(false);
    try {
      const updated = await updateProfile(form);
      setProfile(updated);
      await refreshUser();
      setSaved(true);
    } catch (err: unknown) {
      const normalizedError = err instanceof Error ? err : new Error(String(err));
      setError(formatApiErrors(isAxiosError(err) ? err : normalizedError));
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="max-w-lg mx-auto w-full">
      <h1 className="font-headline-xl text-headline-xl text-primary mb-xl">Your Profile</h1>

      {loading && <ProfileSkeleton />}
      {error && !profile && <p className="text-red-600 loading-enter">{error}</p>}

      {profile && (
        <ContentReveal show>
          <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-xl">
            <div className="flex items-center gap-lg mb-xl">
              <div className="w-16 h-16 rounded-full bg-primary/10 flex items-center justify-center text-xl font-bold text-primary">
                {getInitials(profile.displayName || profile.userName)}
              </div>
              <div>
                <h2 className="font-headline-md text-headline-md font-bold">{profile.displayName}</h2>
                <p className="text-on-surface-variant">@{profile.userName}</p>
              </div>
            </div>

            <form onSubmit={handleSubmit} className="space-y-md text-sm mb-xl">
              <div>
                <label className="block text-on-surface-variant mb-xs">Display name</label>
                <input
                  value={form.displayName}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setForm({ ...form, displayName: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-md py-sm"
                  required
                />
              </div>
              <div>
                <label className="block text-on-surface-variant mb-xs">First name</label>
                <input
                  value={form.firstName}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setForm({ ...form, firstName: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-md py-sm"
                  required
                />
              </div>
              <div>
                <label className="block text-on-surface-variant mb-xs">Email</label>
                <input
                  type="email"
                  value={form.email}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    setForm({ ...form, email: e.target.value })
                  }
                  className="w-full border border-outline-variant rounded-lg px-md py-sm"
                  required
                />
              </div>
              {error && profile && <p className="text-red-600 text-sm">{error}</p>}
              <LoadingButton
                type="submit"
                loading={saving}
                loadingText="Saving…"
                className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold disabled:opacity-50"
              >
                Save profile
              </LoadingButton>
              {saved && <p className="text-green-600 text-sm">Profile updated.</p>}
            </form>

            <dl className="space-y-md text-sm border-t border-outline-variant pt-md">
              <div className="flex justify-between border-b border-outline-variant pb-md">
                <dt className="text-on-surface-variant">Role</dt>
                <dd className="font-medium">{profile.role}</dd>
              </div>
              <div className="flex justify-between border-b border-outline-variant pb-md">
                <dt className="text-on-surface-variant">Status</dt>
                <dd>
                  <StatusBadge
                    status={profile.status ?? ''}
                    type={
                      profile.status === 'Active'
                        ? 'success'
                        : profile.status === 'Suspended'
                          ? 'error'
                          : 'warning'
                    }
                  />
                </dd>
              </div>
              <div className="flex justify-between">
                <dt className="text-on-surface-variant">Member since</dt>
                <dd className="font-medium">{formatDate(profile.createdAt)}</dd>
              </div>
            </dl>

            <Link
              to="/exams"
              className="mt-lg inline-block text-secondary-container font-bold text-sm hover:underline"
            >
              View your progress →
            </Link>
          </div>
        </ContentReveal>
      )}
    </div>
  );
}
