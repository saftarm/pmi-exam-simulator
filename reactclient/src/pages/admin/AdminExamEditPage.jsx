import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import AdminLayout from '../../components/admin/AdminLayout';
import Icon from '../../components/Icon';
import { getExamDetails, updateExam } from '../../services/adminExamService';
import {
  getDomainsByExam,
  updateDomain,
  deleteDomain,
  createDomain,
} from '../../services/adminDomainService';
import { formatApiErrors } from '../../services/authService';
import { FormSkeleton, LoadingButton } from '../../components/loading';

export default function AdminExamEditPage() {
  const { examId } = useParams();
  const navigate = useNavigate();
  const [exam, setExam] = useState(null);
  const [domains, setDomains] = useState([]);
  const [form, setForm] = useState({ durationInMinutes: 0, numberOfQuestions: 0 });
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [addingDomain, setAddingDomain] = useState(false);
  const [error, setError] = useState(null);
  const [domainError, setDomainError] = useState(null);

  useEffect(() => {
    Promise.all([getExamDetails(examId), getDomainsByExam(examId)])
      .then(([details, domainList]) => {
        setExam(details);
        setForm({
          durationInMinutes: details.durationInMinutes,
          numberOfQuestions: details.numberOfQuestions,
        });
        setDomains(domainList || []);
      })
      .catch(() => setError('Failed to load exam'))
      .finally(() => setLoading(false));
  }, [examId]);

  const handleSaveExam = async (e) => {
    e.preventDefault();
    setSaving(true);
    setError(null);
    try {
      await updateExam(examId, form);
      navigate(`/admin/exams/${examId}`);
    } catch (err) {
      setError(formatApiErrors(err));
    } finally {
      setSaving(false);
    }
  };

  const handleDomainSave = async (domain) => {
    setDomainError(null);
    try {
      await updateDomain(domain.id, {
        title: domain.title,
        description: domain.description,
        weight: Number(domain.weight) || 1,
      });
    } catch (err) {
      setDomainError(formatApiErrors(err));
    }
  };

  const handleDomainDelete = async (domain) => {
    if (!window.confirm(`Delete domain "${domain.title}"?`)) return;
    setDomainError(null);
    try {
      await deleteDomain(domain.id);
      setDomains((d) => d.filter((x) => x.id !== domain.id));
    } catch (err) {
      setDomainError(formatApiErrors(err));
    }
  };

  const updateDomainField = (id, field, value) => {
    setDomains((list) => list.map((d) => (d.id === id ? { ...d, [field]: value } : d)));
  };

  const handleAddDomain = async () => {
    setAddingDomain(true);
    setDomainError(null);
    try {
      await createDomain(examId, {
        title: 'New domain',
        description: '',
        weight: 1,
      });
      const domainList = await getDomainsByExam(examId);
      setDomains(domainList || []);
    } catch (err) {
      setDomainError(formatApiErrors(err));
    } finally {
      setAddingDomain(false);
    }
  };

  if (loading) {
    return (
      <AdminLayout title="Edit Exam">
        <FormSkeleton fields={5} />
      </AdminLayout>
    );
  }

  return (
    <AdminLayout title={`Edit: ${exam?.title || 'Exam'}`}>
      <button
        type="button"
        onClick={() => navigate(`/admin/exams/${examId}`)}
        className="mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs"
      >
        <Icon name="arrow_back" style={{ fontSize: 18 }} />
        Back to exam
      </button>

      {error && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">
          {error}
        </div>
      )}
      {domainError && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200 flex justify-between gap-md">
          <span>{domainError}</span>
          <button type="button" onClick={() => setDomainError(null)} className="shrink-0 font-bold">
            Dismiss
          </button>
        </div>
      )}

      <form
        onSubmit={handleSaveExam}
        className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg mb-lg max-w-xl space-y-md"
      >
        <h2 className="font-headline-sm text-headline-sm font-bold">Exam settings</h2>
        <p className="text-sm text-on-surface-variant">
          Only duration and question count can be updated via the API.
        </p>
        <div>
          <label className="block text-sm font-bold mb-sm">Duration (minutes)</label>
          <input
            type="number"
            min={1}
            required
            value={form.durationInMinutes}
            onChange={(e) => setForm({ ...form, durationInMinutes: Number(e.target.value) })}
            className="w-full border border-outline-variant rounded-lg px-md py-sm"
          />
        </div>
        <div>
          <label className="block text-sm font-bold mb-sm">Number of questions</label>
          <input
            type="number"
            min={1}
            required
            value={form.numberOfQuestions}
            onChange={(e) => setForm({ ...form, numberOfQuestions: Number(e.target.value) })}
            className="w-full border border-outline-variant rounded-lg px-md py-sm"
          />
        </div>
        <LoadingButton
          type="submit"
          loading={saving}
          loadingText="Saving…"
          className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold disabled:opacity-50"
        >
          Save Exam Settings
        </LoadingButton>
      </form>

      <div className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg">
        <div className="flex justify-between items-center mb-md">
          <h2 className="font-headline-sm text-headline-sm font-bold">Domains</h2>
          <button
            type="button"
            onClick={handleAddDomain}
            disabled={addingDomain}
            className="text-sm font-bold text-secondary-container hover:underline disabled:opacity-50"
          >
            {addingDomain ? 'Adding…' : '+ Add domain'}
          </button>
        </div>
        {domains.length === 0 ? (
          <p className="text-on-surface-variant text-sm">No domains.</p>
        ) : (
          <div className="space-y-md">
            {domains.map((domain) => (
              <div
                key={domain.id}
                className="border border-outline-variant rounded-lg p-md space-y-sm"
              >
                <input
                  value={domain.title || ''}
                  onChange={(e) => updateDomainField(domain.id, 'title', e.target.value)}
                  className="w-full border border-outline-variant rounded-lg px-md py-sm"
                />
                <input
                  value={domain.description || ''}
                  onChange={(e) => updateDomainField(domain.id, 'description', e.target.value)}
                  className="w-full border border-outline-variant rounded-lg px-md py-sm"
                />
                <input
                  type="number"
                  min={1}
                  value={domain.weight ?? 1}
                  onChange={(e) => updateDomainField(domain.id, 'weight', e.target.value)}
                  className="w-32 border border-outline-variant rounded-lg px-md py-sm"
                />
                <div className="flex gap-md">
                  <button
                    type="button"
                    onClick={() => handleDomainSave(domain)}
                    className="text-sm font-bold text-secondary-container hover:underline"
                  >
                    Save domain
                  </button>
                  <button
                    type="button"
                    onClick={() => handleDomainDelete(domain)}
                    className="text-sm font-bold text-red-600 hover:underline"
                  >
                    Delete
                  </button>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </AdminLayout>
  );
}
