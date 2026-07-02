import { useEffect, useMemo, useState, type ChangeEvent, type FormEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { isAxiosError } from 'axios';
import { AdminLayout } from '../../auth';
import Icon from '../../../shared/components/Icon';
import { createExam } from '../api';
import { getCategories } from '../../admin-categories/api';
import { formatApiErrors } from '../../../shared/api/errors';
import type { ApiId } from '../../../shared/api/primitives';
import type { CategoryDto } from '../../admin-categories/types';
import LoadingButton from '../../../shared/components/loading/LoadingButton';

const EMPTY_DOMAIN = { title: '', description: '', weight: 1 };

interface CreateExamDomainForm {
  title: string;
  description: string;
  weight: number | string;
}

interface CreateExamFormState {
  categoryId: ApiId | '';
  title: string;
  context: string;
  durationInMinutes: number;
  numberOfQuestions: number;
  createDomainDtos: CreateExamDomainForm[];
}

type DomainFormField = keyof CreateExamDomainForm;

export default function AdminExamCreatePage() {
  const navigate = useNavigate();
  const [categories, setCategories] = useState<CategoryDto[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [form, setForm] = useState<CreateExamFormState>({
    categoryId: '',
    title: '',
    context: '',
    durationInMinutes: 230,
    numberOfQuestions: 180,
    createDomainDtos: [{ ...EMPTY_DOMAIN }],
  });

  useEffect(() => {
    getCategories()
      .catch(() => [])
      .then((data) => {
        setCategories(data);
        if (data.length > 0) {
          setForm((f) => ({ ...f, categoryId: data[0].id }));
        }
      });
  }, []);

  const updateDomain = (index: number, field: DomainFormField, value: string | number) => {
    setForm((f) => {
      const domains = [...f.createDomainDtos];
      domains[index] = { ...domains[index], [field]: value };
      return { ...f, createDomainDtos: domains };
    });
  };

  const addDomain = () => {
    setForm((f) => ({
      ...f,
      createDomainDtos: [...f.createDomainDtos, { ...EMPTY_DOMAIN }],
    }));
  };

  const removeDomain = (index: number) => {
    setForm((f) => ({
      ...f,
      createDomainDtos: f.createDomainDtos.filter((_, i) => i !== index),
    }));
  };

  const weightSum = useMemo(
    () => form.createDomainDtos.reduce((sum, d) => sum + (Number(d.weight) || 0), 0),
    [form.createDomainDtos],
  );
  const weightsValid = weightSum >= 99 && weightSum <= 101;

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!weightsValid) {
      setError('Domain weights must sum to approximately 100 (99–101).');
      return;
    }
    setError(null);
    setLoading(true);
    try {
      const payload = {
        ...form,
        categoryId: form.categoryId,
        createDomainDtos: form.createDomainDtos.map((d) => ({
          title: d.title,
          description: d.description,
          weight: Number(d.weight) || 1,
        })),
      };
      await createExam(payload);
      navigate('/admin/exams');
    } catch (err: unknown) {
      const normalizedError = err instanceof Error ? err : new Error(String(err));
      setError(formatApiErrors(isAxiosError(err) ? err : normalizedError));
    } finally {
      setLoading(false);
    }
  };

  return (
    <AdminLayout title="Create Exam">
      <button
        type="button"
        onClick={() => navigate('/admin/exams')}
        className="mb-lg text-sm text-secondary-container font-bold hover:underline flex items-center gap-xs"
      >
        <Icon name="arrow_back" style={{ fontSize: 18 }} />
        Back to exams
      </button>

      {error && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200">
          {error}
        </div>
      )}

      <form
        onSubmit={handleSubmit}
        className="bg-white rounded-xl border border-outline-variant shadow-sm p-lg space-y-lg max-w-3xl"
      >
        <div className="grid grid-cols-1 md:grid-cols-2 gap-lg">
          <div className="md:col-span-2">
            <label className="block text-sm font-bold mb-sm">Title</label>
            <input
              required
              maxLength={200}
              value={form.title}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setForm({ ...form, title: e.target.value })}
              className="w-full border border-outline-variant rounded-lg px-md py-sm"
            />
          </div>
          <div className="md:col-span-2">
            <label className="block text-sm font-bold mb-sm">Context / Description</label>
            <textarea
              value={form.context}
              onChange={(e: ChangeEvent<HTMLTextAreaElement>) => setForm({ ...form, context: e.target.value })}
              rows={3}
              className="w-full border border-outline-variant rounded-lg px-md py-sm"
            />
          </div>
          <div>
            <label className="block text-sm font-bold mb-sm">Category</label>
            <select
              required
              value={form.categoryId}
              onChange={(e: ChangeEvent<HTMLSelectElement>) => setForm({ ...form, categoryId: e.target.value })}
              className="w-full border border-outline-variant rounded-lg px-md py-sm"
              disabled={categories.length === 0}
            >
              {categories.length === 0 ? (
                <option value="">No categories — create one first</option>
              ) : (
                categories.map((c) => (
                  <option key={c.id} value={c.id}>
                    {c.title}
                  </option>
                ))
              )}
            </select>
            {categories.length === 0 && (
              <Link
                to="/admin/exams?tab=categories"
                className="text-sm text-secondary-container font-bold hover:underline mt-xs inline-block"
              >
                Create a category first →
              </Link>
            )}
          </div>
          <div>
            <label className="block text-sm font-bold mb-sm">Duration (minutes)</label>
            <input
              type="number"
              min={1}
              max={360}
              required
              value={form.durationInMinutes}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setForm({ ...form, durationInMinutes: Number(e.target.value) })
              }
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
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setForm({ ...form, numberOfQuestions: Number(e.target.value) })
              }
              className="w-full border border-outline-variant rounded-lg px-md py-sm"
            />
          </div>
        </div>

        <div>
          <div className="flex justify-between items-center mb-md">
            <div>
              <h2 className="font-headline-sm text-headline-sm font-bold">Domains</h2>
              <p
                className={`text-sm mt-xs ${weightsValid ? 'text-on-surface-variant' : 'text-red-600 font-medium'}`}
              >
                Weight total: {weightSum} (must be 99–101)
              </p>
            </div>
            <button
              type="button"
              onClick={addDomain}
              className="text-sm font-bold text-secondary-container hover:underline"
            >
              + Add domain
            </button>
          </div>
          <div className="space-y-md">
            {form.createDomainDtos.map((domain, index) => (
              <div key={index} className="border border-outline-variant rounded-lg p-md space-y-sm">
                <div className="flex justify-between">
                  <span className="text-sm font-bold">Domain {index + 1}</span>
                  {form.createDomainDtos.length > 1 && (
                    <button
                      type="button"
                      onClick={() => removeDomain(index)}
                      className="text-xs text-red-600 font-bold"
                    >
                      Remove
                    </button>
                  )}
                </div>
                <input
                  required
                  placeholder="Title"
                  value={domain.title}
                  onChange={(e: ChangeEvent<HTMLInputElement>) => updateDomain(index, 'title', e.target.value)}
                  className="w-full border border-outline-variant rounded-lg px-md py-sm"
                />
                <input
                  placeholder="Description"
                  value={domain.description}
                  onChange={(e: ChangeEvent<HTMLInputElement>) =>
                    updateDomain(index, 'description', e.target.value)
                  }
                  className="w-full border border-outline-variant rounded-lg px-md py-sm"
                />
                <input
                  type="number"
                  min={1}
                  placeholder="Weight"
                  value={domain.weight}
                  onChange={(e: ChangeEvent<HTMLInputElement>) => updateDomain(index, 'weight', e.target.value)}
                  className="w-32 border border-outline-variant rounded-lg px-md py-sm"
                />
              </div>
            ))}
          </div>
        </div>

        <div className="flex gap-md pt-md border-t border-outline-variant">
          <LoadingButton
            type="submit"
            loading={loading}
            loadingText="Creating…"
            disabled={categories.length === 0 || !weightsValid}
            className="bg-secondary-container text-white px-lg py-sm rounded-lg font-bold disabled:opacity-50"
          >
            Create Exam
          </LoadingButton>
          <button
            type="button"
            onClick={() => navigate('/admin/exams')}
            className="px-lg py-sm rounded-lg border border-outline-variant"
          >
            Cancel
          </button>
        </div>
      </form>
    </AdminLayout>
  );
}
