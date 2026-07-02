import { useCallback, useEffect, useState, type ChangeEvent } from 'react';
import { Link } from 'react-router-dom';
import { isAxiosError } from 'axios';
import { AdminLayout } from '../../auth';
import QuestionTable from '../components/QuestionTable';
import DeleteConfirmModal from '../../../shared/components/DeleteConfirmModal';
import Icon from '../../../shared/components/Icon';
import { getAllDomains } from '../../admin-categories/api';
import { bulkDeleteQuestions, deleteQuestion, getQuestions, importQuestions } from '../api';
import { formatApiErrors } from '../../../shared/api/errors';
import { QUESTION_TYPES } from '../utils/questionTypes';
import type { ApiId } from '../../../shared/api/primitives';
import type { DomainDto } from '../../admin-categories/types';
import type {
  ImportRowErrorDto,
  QuestionListItemDto,
  QuestionType,
} from '../types';
import { FormSkeleton, Spinner } from '../../../shared/components/loading';

interface ImportResultState {
  success: boolean;
  message: string;
  errors: ImportRowErrorDto[];
}

function formatCaughtError(err: unknown, fallback: string): string {
  const normalizedError = err instanceof Error ? err : new Error(String(err));
  return formatApiErrors(isAxiosError(err) ? err : normalizedError) || fallback;
}

export default function AdminQuestionsPage() {
  const [domains, setDomains] = useState<DomainDto[]>([]);
  const [questions, setQuestions] = useState<QuestionListItemDto[]>([]);
  const [page, setPage] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [selectedQuestions, setSelectedQuestions] = useState<Set<ApiId>>(new Set());
  const [search, setSearch] = useState('');
  const [domainId, setDomainId] = useState<ApiId | ''>('');
  const [questionType, setQuestionType] = useState<QuestionType | ''>('');
  const [loading, setLoading] = useState(true);
  const [questionsLoading, setQuestionsLoading] = useState(true);
  const [importing, setImporting] = useState(false);
  const [importResult, setImportResult] = useState<ImportResultState | null>(null);
  const [deleteTarget, setDeleteTarget] = useState<QuestionListItemDto | null>(null);
  const [bulkDeleteOpen, setBulkDeleteOpen] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const [loadError, setLoadError] = useState<string | null>(null);
  const [actionError, setActionError] = useState<string | null>(null);

  const pageSize = 20;

  useEffect(() => {
    getAllDomains()
      .then((d) => setDomains(d))
      .catch(() => setDomains([]))
      .finally(() => setLoading(false));
  }, []);

  const loadQuestions = useCallback(async () => {
    setQuestionsLoading(true);
    setLoadError(null);
    try {
      const data = await getQuestions({
        pageNumber: page,
        pageSize,
        search: search.trim() || undefined,
        domainId: domainId || undefined,
        questionType: questionType || undefined,
      });
      setQuestions(data.items || []);
      setTotalCount(data.totalCount ?? 0);
      setHasNextPage(data.hasNextPage ?? false);
      setSelectedQuestions(new Set());
    } catch (err: unknown) {
      setLoadError(formatCaughtError(err, 'Failed to load questions'));
      setQuestions([]);
    } finally {
      setQuestionsLoading(false);
    }
  }, [page, search, domainId, questionType]);

  useEffect(() => {
    if (!loading) {
      const timer = setTimeout(loadQuestions, search ? 300 : 0);
      return () => clearTimeout(timer);
    }
  }, [loading, loadQuestions, search]);

  useEffect(() => {
    setPage(1);
  }, [search, domainId, questionType]);

  const handleImport = async (e: ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setImporting(true);
    setImportResult(null);
    try {
      const result = await importQuestions(file);
      setImportResult({
        success: result.success !== false,
        message: `Imported ${result.importedCount ?? 0} question(s).`,
        errors: result.errors || [],
      });
      setPage(1);
      await loadQuestions();
    } catch (err: unknown) {
      setImportResult({
        success: false,
        message: formatCaughtError(err, 'Import failed'),
        errors: [],
      });
    } finally {
      setImporting(false);
      e.target.value = '';
    }
  };

  const handleDeleteQuestion = async () => {
    if (!deleteTarget) return;
    setDeleting(true);
    setActionError(null);
    try {
      await deleteQuestion(deleteTarget.id);
      setDeleteTarget(null);
      await loadQuestions();
    } catch (err: unknown) {
      setActionError(formatCaughtError(err, 'Failed to delete question'));
    } finally {
      setDeleting(false);
    }
  };

  const handleBulkDelete = async () => {
    setDeleting(true);
    setActionError(null);
    try {
      await bulkDeleteQuestions([...selectedQuestions]);
      setBulkDeleteOpen(false);
      await loadQuestions();
    } catch (err: unknown) {
      setActionError(formatCaughtError(err, 'Failed to delete questions'));
    } finally {
      setDeleting(false);
    }
  };

  if (loading) {
    return (
      <AdminLayout title="Question Pool">
        <FormSkeleton fields={4} />
      </AdminLayout>
    );
  }

  return (
    <AdminLayout title="Question Pool">
      <div className="flex flex-col lg:flex-row justify-between gap-md mb-lg">
        <p className="text-on-surface-variant text-sm max-w-xl">
          Global question bank shared across exams. Questions are sampled by domain when a learner
          starts a session.
        </p>
        <div className="flex flex-wrap gap-md">
          <Link
            to="/admin/questions/new"
            className="flex items-center gap-sm px-md py-sm rounded-lg bg-secondary-container text-white font-bold text-sm hover:brightness-110"
          >
            <Icon name="add" style={{ fontSize: 18 }} />
            Add Question
          </Link>
          <label
            className={`flex items-center gap-sm px-md py-sm rounded-lg border border-outline-variant font-bold text-sm cursor-pointer hover:bg-surface-container-low transition-opacity ${importing ? 'opacity-70' : ''}`}
          >
            {importing ? (
              <Spinner size="sm" label="Importing" />
            ) : (
              <Icon name="upload_file" style={{ fontSize: 18 }} />
            )}
            {importing ? 'Importing…' : 'Import Excel'}
            <input
              type="file"
              accept=".xlsx,.xls"
              className="hidden"
              disabled={importing}
              onChange={handleImport}
            />
          </label>
        </div>
      </div>

      {loadError && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200 text-sm">
          {loadError}
          {loadError.includes('405') || loadError.toLowerCase().includes('method') ? (
            <p className="mt-sm text-xs">
              The API may be running an old build. Stop TestAPI and run <code>dotnet run</code>{' '}
              again.
            </p>
          ) : null}
        </div>
      )}

      {actionError && (
        <div className="mb-lg p-md bg-red-50 text-red-700 rounded-lg border border-red-200 flex justify-between gap-md text-sm">
          <span>{actionError}</span>
          <button type="button" onClick={() => setActionError(null)} className="shrink-0 font-bold">
            Dismiss
          </button>
        </div>
      )}

      {importResult && (
        <div className="mb-lg p-md rounded-lg border border-outline-variant bg-surface-container-low">
          <p
            className={`text-sm font-medium ${
              importResult.success ? 'text-green-600' : 'text-red-600'
            }`}
          >
            {importResult.message}
          </p>
          {importResult.errors?.length > 0 && (
            <ul className="mt-sm max-h-40 overflow-y-auto text-xs text-amber-800 bg-amber-50 border border-amber-200 rounded-lg p-sm space-y-xs">
              {importResult.errors.map((err, i) => (
                <li key={i}>
                  Row {err.row}: {err.reason}
                </li>
              ))}
            </ul>
          )}
        </div>
      )}

      <div className="bg-white rounded-xl border border-outline-variant shadow-sm overflow-hidden">
        <div className="p-lg border-b border-outline-variant flex flex-col gap-md">
          <div className="flex flex-col sm:flex-row justify-between gap-md">
            <div>
              <h2 className="font-headline-sm text-headline-sm font-bold">Questions</h2>
              <p className="text-xs text-on-surface-variant mt-xs">{totalCount} total</p>
            </div>
            {selectedQuestions.size > 0 && (
              <button
                type="button"
                onClick={() => setBulkDeleteOpen(true)}
                className="text-sm font-bold text-red-600 hover:underline"
              >
                Delete selected ({selectedQuestions.size})
              </button>
            )}
          </div>
          <div className="flex flex-col sm:flex-row gap-md">
            <input
              type="search"
              placeholder="Search questions…"
              value={search}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setSearch(e.target.value)}
              className="flex-1 border border-outline-variant rounded-lg px-md py-sm text-sm"
            />
            <select
              value={domainId}
              onChange={(e: ChangeEvent<HTMLSelectElement>) => setDomainId(e.target.value)}
              className="border border-outline-variant rounded-lg px-md py-sm text-sm"
            >
              <option value="">All domains</option>
              {domains.map((d) => (
                <option key={d.id} value={d.id}>
                  {d.title}
                </option>
              ))}
            </select>
            <select
              value={questionType}
              onChange={(e: ChangeEvent<HTMLSelectElement>) =>
                setQuestionType(e.target.value as QuestionType | '')
              }
              className="border border-outline-variant rounded-lg px-md py-sm text-sm"
            >
              <option value="">All types</option>
              {QUESTION_TYPES.map((t) => (
                <option key={t.value} value={t.value}>
                  {t.label}
                </option>
              ))}
            </select>
          </div>
        </div>
        <QuestionTable
          questions={questions}
          loading={questionsLoading}
          selected={selectedQuestions}
          onToggle={(id: ApiId) => {
            setSelectedQuestions((prev) => {
              const next = new Set(prev);
              if (next.has(id)) next.delete(id);
              else next.add(id);
              return next;
            });
          }}
          onToggleAll={() => {
            if (selectedQuestions.size === questions.length) {
              setSelectedQuestions(new Set());
            } else {
              setSelectedQuestions(new Set(questions.map((q) => q.id)));
            }
          }}
          onDeleteOne={setDeleteTarget}
        />
        <div className="flex justify-between items-center p-lg border-t border-outline-variant text-sm">
          <span>Page {page}</span>
          <div className="flex gap-md">
            <button
              type="button"
              disabled={page <= 1 || questionsLoading}
              onClick={() => setPage((p) => p - 1)}
              className="px-md py-sm border border-outline-variant rounded-lg disabled:opacity-50"
            >
              Previous
            </button>
            <button
              type="button"
              disabled={!hasNextPage || questionsLoading}
              onClick={() => setPage((p) => p + 1)}
              className="px-md py-sm border border-outline-variant rounded-lg disabled:opacity-50"
            >
              Next
            </button>
          </div>
        </div>
      </div>

      <DeleteConfirmModal
        open={!!deleteTarget}
        title="Delete question?"
        message="This will permanently delete the question."
        loading={deleting}
        onCancel={() => setDeleteTarget(null)}
        onConfirm={handleDeleteQuestion}
      />
      <DeleteConfirmModal
        open={bulkDeleteOpen}
        title="Delete selected questions?"
        message={`Delete ${selectedQuestions.size} question(s)?`}
        loading={deleting}
        onCancel={() => setBulkDeleteOpen(false)}
        onConfirm={handleBulkDelete}
      />
    </AdminLayout>
  );
}
